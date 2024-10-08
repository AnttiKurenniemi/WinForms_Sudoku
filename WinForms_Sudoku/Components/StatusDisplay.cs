﻿using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WinForms_Sudoku
{
    public partial class StatusDisplay : UserControl
    {
        #region Variable declarations


        /// <summary>
        /// OwnerGrid is the visual sudoku grid, to which this status control belongs to.
        /// </summary>
        public SudokuGrid? OwnerGrid { get; set; }

        // Following variables are used by the double-buffered drawing:
        private bool InitializationComplete;
        private BufferedGraphicsContext? BackbufferContext;
        private BufferedGraphics? BackbufferGraphics;
        private Graphics? DrawingGraphics;

        /// <summary>
        /// The status display is updated on a timer, mainly for the game time label to update all the time
        /// </summary>
        private System.Windows.Forms.Timer? StatusUpdateTimer;

        /// <summary>
        /// Border pen for light color; light and dark color used to make the main game board seem slightly 3D-ish.
        /// </summary>
        private readonly Pen BorderPen_Light = new Pen(Color.LightGray, 1);
        
        /// <summary>
        /// Border pen for dark color; light and dark color used to make the main game board seem slightly 3D-ish.
        /// </summary>
        private readonly Pen BorderPen_Dark = new Pen(Color.DarkGray, 1);

        private readonly Font NumberFont = new Font("Tahoma", 9);
        private readonly Font SelectedNumberFont = new Font("Tahoma", 9, FontStyle.Bold);
        private readonly Font SolvedNumberFont = new Font("Tahoma", 9);
        private readonly Font SelectedSolvedNumberFont = new Font("Tahoma", 9, FontStyle.Bold);
        private readonly Brush NumberFontBrush = new SolidBrush(Color.DarkRed);
        private readonly Brush SolvedNumberFontBrush = new SolidBrush(Color.DarkGreen);

        /// <summary>
        /// Highlight the selected number (full row) in the display by setting background color to similar to selected cell
        /// </summary>
        private readonly Brush SelectedNumberBackgroundBrush = new SolidBrush(Color.FromArgb(200, 225, 255));

        private readonly StringFormat NumberFormat = new StringFormat();
        private readonly Brush TotalBrush = new SolidBrush(Color.Black);

        /// <summary>The space between the "numbers" area and the labels at the top</summary>
        private int TopPadding;

        #endregion


        #region Constructor and initialisation code

        public StatusDisplay()
        {
            InitializeComponent();

            // Set the control style to double buffer.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Assign our buffer context.
            BackbufferContext = BufferedGraphicsManager.Current;
            InitializationComplete = true;

            RecreateBuffers();

            NumberFormat.LineAlignment = StringAlignment.Center;
            NumberFormat.Alignment = StringAlignment.Center;

            InitialiseStatusUpdateTimer();

            Redraw();
        }


        /// <summary>
        /// Set up a timer to fire ever 1 second, which then updates the status labels.
        /// </summary>
        private void InitialiseStatusUpdateTimer()
        {
            StatusUpdateTimer = new System.Windows.Forms.Timer();
            StatusUpdateTimer.Interval = 1000;  // about 1 second
            StatusUpdateTimer.Tick += StatusUpdateTimer_Tick;
            StatusUpdateTimer.Enabled = true;
        }

        #endregion

        /// <summary>
        /// Status label update timer tick; does nothing else than simply update the labels.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusUpdateTimer_Tick(object? sender, EventArgs e)
        {
            UpdateStatusLabels();
        }


        #region Double-buffered drawing system methods

        private void RecreateBuffers()
        {
            // Check initialization has completed so we know backbufferContext has been assigned.
            // Check that we aren't disposing or this could be invalid.
            if (!InitializationComplete || BackbufferContext == null)
                return;

            // We recreate the buffer with a width and height of the control. The "+ 1" 
            // guarantees we never have a buffer with a width or height of 0. 
            BackbufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);

            // Dispose of old backbufferGraphics (if one has been created already)
            if (BackbufferGraphics != null)
                BackbufferGraphics.Dispose();

            // Create new backbufferGrpahics that matches the current size of buffer.
            BackbufferGraphics = BackbufferContext.Allocate(this.CreateGraphics(),
                new Rectangle(0, 0, Math.Max(this.Width, 1), Math.Max(this.Height, 1)));

            // Assign the Graphics object on backbufferGraphics to "drawingGraphics" for easy reference elsewhere.
            DrawingGraphics = BackbufferGraphics.Graphics;
            //drawingGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            // This is a good place to assign drawingGraphics.SmoothingMode if you want a better anti-aliasing technique.
            DrawingGraphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Invalidate the control so a repaint gets called somewhere down the line.
            this.Invalidate();
        }


        /// <summary>
        /// Redraw on resize; also calculate the font size.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecreateBuffers();

            Redraw();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            // If we've initialized the backbuffer properly, render it on the control. 
            // Otherwise, do just the standard control paint.
            if (BackbufferGraphics != null)
                BackbufferGraphics.Render(e.Graphics);
        }
        #endregion

        /// <summary>
        /// Main drawing method
        /// </summary>
        public void Redraw()
        {
            if (DrawingGraphics == null || Disposing || OwnerGrid == null)
                return;

            // Clear the background
            DrawingGraphics.Clear(BackColor);

            TopPadding = lbSolved.Top + lbSolved.Height;

            // Draw bottom half in white, and borders around it
            Rectangle BackGround = new Rectangle(0, TopPadding, Width, Height - TopPadding);
            DrawingGraphics.FillRectangle(Brushes.White, BackGround);

            // Borders:
            DrawingGraphics.DrawLine(BorderPen_Dark, 0, TopPadding, Width - 1, TopPadding);            // Top left - top right
            DrawingGraphics.DrawLine(BorderPen_Light, Width - 1, TopPadding, Width - 1, Height - 1);   // top right - bottom right
            DrawingGraphics.DrawLine(BorderPen_Dark, 0, Height - 1, 0, TopPadding);                    // bottom left - top left
            DrawingGraphics.DrawLine(BorderPen_Light, 0, Height - 1, Width - 1, Height - 1);           // bottom left - bottom right

            DrawValues(OwnerGrid.GetSolvedValues());

            this.Refresh();
        }


        /// <summary>
        /// Draw the values as counts of all solved / not soled cells
        /// </summary>
        /// <param name="SolvedValues"></param>
        private void DrawValues(SolvedValuesData SolvedValues)
        {
            int CellHeight = Convert.ToInt32((Height - TopPadding) / 9);
            for (int i = 1; i < 10; i++)
            {
                Rectangle Cell = new Rectangle(1, ((i - 1) * CellHeight) + TopPadding + 2, Width - 3, CellHeight);
                DrawSingleNumberSolved(i, SolvedValues.SolvedCounts[i], Cell);
            }

            UpdateStatusLabels();
        }

        /// <summary>
        /// Update labels with information about time used, moves used and number of cells solved.
        /// </summary>
        private void UpdateStatusLabels()
        {
            if (OwnerGrid == null)
                return;

            // Update labels:
            SolvedValuesData SolvedValues = OwnerGrid.GetSolvedValues();
            lbMoves.Text = OwnerGrid.MovesUsed.ToString() + " moves used";
            if (!OwnerGrid.Solved())
            {
                // Don't update time after game has been solved. Also, if the solved cells count is zero, the game has not
                // even started yet - don't increase counter.
                if (SolvedValues.TotalSolvedCount < 1)
                    OwnerGrid.StartTime = DateTime.Now;
                TimeSpan GameDuration = DateTime.Now - OwnerGrid.StartTime;
                lbTime.Text = "Time: " + GameDuration.ToString(@"hh\:mm\:ss");
            }
            lbSolved.Text = "Solved " + SolvedValues.TotalSolvedCount.ToString() + " / 81 cells";
        }


        /// <summary>
        /// Draw a line of single numbers; cross over the solved ones. Draw all in green if all are solved.
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="SolvedCount"></param>
        /// <param name="Cell"></param>
        private void DrawSingleNumberSolved(int Number, int SolvedCount, Rectangle Cell)
        {
            if (OwnerGrid == null || DrawingGraphics == null)
                return;
                
            
            // Draw the number at the left of the row:
            if (OwnerGrid.LastSelectedValue == Number)
            {
                DrawingGraphics.FillRectangle(SelectedNumberBackgroundBrush, Cell);
                DrawingGraphics.DrawString(Number.ToString() + " :", SelectedNumberFont, TotalBrush, 4, Cell.Top);
            }
            else
                DrawingGraphics.DrawString(Number.ToString() + " :", NumberFont, TotalBrush, 4, Cell.Top);

            // Then a mark for each solved or not solved cell
            for (int i = 1; i < 10; i++)
            {
                int left = Cell.Left + (i * 14) + 10;

                if (i > SolvedCount)
                {
                    // Unsolved number - draw a red question mark
                    if (OwnerGrid.LastSelectedValue == Number)
                        DrawingGraphics.DrawString("?", SelectedNumberFont, NumberFontBrush, left, Cell.Top);
                    else
                        DrawingGraphics.DrawString("?", NumberFont, NumberFontBrush, left, Cell.Top);
                }
                else
                {
                    // Solved number - draw a green, X 
                    if (OwnerGrid.LastSelectedValue == Number)
                        DrawingGraphics.DrawString("X", SelectedSolvedNumberFont, SolvedNumberFontBrush, left, Cell.Top);
                    else
                        DrawingGraphics.DrawString("X", SolvedNumberFont, SolvedNumberFontBrush, left, Cell.Top);
                }
            }
        }
    }
}
