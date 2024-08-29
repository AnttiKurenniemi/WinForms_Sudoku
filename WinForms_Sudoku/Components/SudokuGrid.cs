using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WinForms_Sudoku
{
    public partial class SudokuGrid : UserControl
    {
        /// <summary>
        /// Status display control; a reference is needed here to be able to update it.
        /// </summary>
        private StatusDisplay _GridStatusDisplay = null;
        [Description("Status display, if assigned, shows the number of solved and remaining numbers of a board"), Category("Data")]
        public StatusDisplay GridStatusDisplay
        {
            get
            {
                return _GridStatusDisplay;
            }
            set
            {
                _GridStatusDisplay = value;
                if (value != null)
                {
                    // Give the grid status display this as parameter:
                    _GridStatusDisplay.OwnerGrid = this;
                }
            }
        }

        private GridDimensions Dimensions = new GridDimensions();

        /// <summary>
        /// Property MovesUsed - make an event fire whenever changed
        /// </summary>
        private int _MovesUsed = 0;
        public int MovesUsed
        {
            get { return _MovesUsed; }
            set
            {
                _MovesUsed = value;
                if (_GridStatusDisplay != null)
                    _GridStatusDisplay.Redraw();
            }
        }

        public DateTime StartTime = DateTime.Now;

        // Colors:
        private readonly Brush _backgroundBrush = new SolidBrush(Color.White);
        private readonly Brush _grayBackgroundBrush = new SolidBrush(Color.FromArgb(250, 250, 250));
        private readonly Brush _solvedBackgroundBrush = new SolidBrush(Color.LightGreen);
        private readonly Brush _solvedLineBackgroundBrush = new SolidBrush(Color.FromArgb(220, 255, 220));


        Pen borderPen_Light = new Pen(Color.LightGray, 1);
        Pen borderPen_Dark = new Pen(Color.DarkGray, 1);
        Pen GridLinePen = new Pen(Color.LightGray, 1);
        Pen GridLineDarkPen = new Pen(Color.Black, 1);

        Font NumberFont = new Font("Tahoma", 10);
        StringFormat NumberFormat = new StringFormat();

        // Colors for drawing numbers in different colors:
        Brush _numberBrush = new SolidBrush(Color.Black);
        Brush _errorNumberBrush = new SolidBrush(Color.Red);
        Brush _fixedNumberBrush = new SolidBrush(Color.DimGray);
        Brush _solvedNumberBrush = new SolidBrush(Color.DarkGreen);
        Brush _sameAsSelectedBrush = new SolidBrush(Color.Blue);
        Brush _hintCellBrush = new SolidBrush(Color.Green);
        Brush _selectedCellBrush = new SolidBrush(Color.LightSkyBlue);

        /// <summary>This font is used to show which numbers are still possible</summary>
        Font PossibleValueFont = new Font("Tahoma", 8);
        public bool ShowPossibleValues = false;

        // Following variables are used by the double-buffered drawing:
        private readonly bool _initializationComplete;
        private bool _isDisposing;
        private BufferedGraphicsContext backbufferContext;
        private BufferedGraphics backbufferGraphics;
        private Graphics drawingGraphics;

        private SolvedValuesData SolvedValues = new SolvedValuesData();
        private GameData Board = new GameData();

        // Fading "solvable" text related variables:
        Font SolvableFont = new Font("Tahoma", 32);
        Brush SolvableFontBrush = new SolidBrush(Color.Blue);
        string SolvableText = "";
        System.Windows.Forms.Timer SolvableTimer = new System.Windows.Forms.Timer();

        /// <summary>This value is used to perform a fade-out of the solvable/not sovlable text</summary>
        private int _solvableTimerStep;

        // "Solved" text across the grid after whole grid has been solved
        Font SolvedFont = new Font("Tahoma", 32);
        Brush SolvedBrush = new SolidBrush(Color.Blue);

        Coordinate SelectedCell;

        /// <summary>
        /// Last selected value which does NOT get updated if a 0-value cell is selected. This is
        /// used to enable setting a value by double-clicking a cell, after some cell has been 
        /// selected with a value
        /// </summary>
        public int LastSelectedValue;

        /// <summary>Return the value of currently selected cell, if it is set</summary>
        private int CurrentlySelectedValue
        {
            get
            {
                if ((SelectedCell.X >= 0) && (SelectedCell.Y >= 0))
                    return Board.Data[SelectedCell.X, SelectedCell.Y].Value;
                else
                    return -1;
            }
        }

        /// <summary>
        /// Constructor of the grid component. Set up some drawing variables and finally draw the initial grid.
        /// </summary>
        public SudokuGrid()
        {
            InitializeComponent();

            // Set the control style to double buffer.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Assign our buffer context.
            backbufferContext = BufferedGraphicsManager.Current;
            _initializationComplete = true;

            RecreateBuffers();

            SelectedCell = new Coordinate();
            SelectedCell.X = -1;
            SelectedCell.Y = -1;
            MovesUsed = 0;
            LastSelectedValue = -1;

            SolvableTimer.Interval = 3000;  // 3 seconds
            SolvableTimer.Tick += SolvableTimer_Tick;
            SolvableTimer.Enabled = false;

            NumberFormat.LineAlignment = StringAlignment.Center;
            NumberFormat.Alignment = StringAlignment.Center;

            CalculateGridDimensions();

            Redraw();
        }


            #region Double-buffered drawing methods

        private void RecreateBuffers()
        {
            // Check initialization has completed so we know backbufferContext has been assigned.
            // Check that we aren't disposing or this could be invalid.
            if (!_initializationComplete || _isDisposing)
                return;

            // We recreate the buffer with a width and height of the control. The "+ 1" 
            // guarantees we never have a buffer with a width or height of 0. 
            backbufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);

            // Dispose of old backbufferGraphics (if one has been created already)
            if (backbufferGraphics != null)
                backbufferGraphics.Dispose();

            // Create new backbufferGrpahics that matches the current size of buffer.
            backbufferGraphics = backbufferContext.Allocate(this.CreateGraphics(),
                new Rectangle(0, 0, Math.Max(this.Width, 1), Math.Max(this.Height, 1)));

            // Assign the Graphics object on backbufferGraphics to "drawingGraphics" for easy reference elsewhere.
            drawingGraphics = backbufferGraphics.Graphics;
            drawingGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            // This is a good place to assign drawingGraphics.SmoothingMode if you want a better anti-aliasing technique.
            drawingGraphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Invalidate the control so a repaint gets called somewhere down the line.
            this.Invalidate();
        }


        /// <summary>
        /// Redraw on resize; also calculate the font size.
        /// </summary>
        /// <param name="e"></param>
        /// <inheritdoc />
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecreateBuffers();

            CalculateGridDimensions();

            Redraw();
        }


        
        protected override void OnPaint(PaintEventArgs e)
        {
            // If we've initialized the backbuffer properly, render it on the control. 
            // Otherwise, do just the standard control paint.
            if (!_isDisposing && backbufferGraphics != null)
                backbufferGraphics.Render(e.Graphics);
        }
        #endregion


        #region Drawing code

        /// <summary>
        /// After resize, recalculate the grid dimensions; resize fonts accordingly.
        /// The grid square sizes are used in several places around this class, so it makes sense to only
        /// calculate them when needed. As long as the main window remains the same size, the dimensions
        /// won't change.
        /// </summary>
        private void CalculateGridDimensions()
        {
            Dimensions.CalculateDimensions(this);


            // Fonts and blocks of 3x3 squares with slightly gray background:
            ResizeFonts();
        }

        /// <summary>
        /// Resize fonts; only called from calcluateGridDimensions when the drawing canvas size changes.
        /// </summary>
        private void ResizeFonts()
        {
            float newFontSize = (Width / 9.0F) * 0.4f;
            if (newFontSize > 0.1f)
            {
                NumberFont = new Font("Tahoma", newFontSize);

                newFontSize = Width / 10.0F;
                SolvedFont = new Font("Tahoma", newFontSize, FontStyle.Bold);

                if (SolvableText == "SOLVABLE")
                    SolvableFont = new Font("Tahoma", Width / 12, FontStyle.Bold);
                else if (SolvableText == "Not solvable")
                    SolvableFont = new Font("Tahoma", Width / 15, FontStyle.Bold);
            }
        }



        /// <summary>
        /// Main drawing method
        /// </summary>
        public void Redraw()
        {
            if (drawingGraphics == null)
                return;
            if ((Disposing) || (_isDisposing))
                return;

            drawingGraphics.Clear(SystemColors.Control);

            // Clear the background
            if (Board.Solved)
                drawingGraphics.FillRectangle(_solvedBackgroundBrush, Dimensions.BackgroundRectangle);
            else
            {
                drawingGraphics.FillRectangle(_backgroundBrush, Dimensions.BackgroundRectangle);

                // Draw the slightly gray 3x3 blocks
                DrawBackgroundBlocks();
            }

            // Borders:
            drawingGraphics.DrawLine(borderPen_Dark, 0, 0, Dimensions.GridWidth, 0);                                           // Top left - top right
            drawingGraphics.DrawLine(borderPen_Light, Dimensions.GridWidth, 0, Dimensions.GridWidth, Dimensions.GridHeight);   // top right - bottom right
            drawingGraphics.DrawLine(borderPen_Dark, 0, Dimensions.GridHeight, 0, 0);                                          // bottom left - top left
            drawingGraphics.DrawLine(borderPen_Light, 0, Dimensions.GridHeight, Dimensions.GridWidth, Dimensions.GridHeight);  // bottom left - bottom right

            SolvedValues.Refresh(Board);

            DrawSelectedCell();
            DrawGridLines();
            DrawSolvedCellBackgrounds();
            DrawSolvableText();  // This probably should be before "DrawNumbers" because the numbers are otherwise not visible. BUT,
                                 // in that case the "solvable" text is lost whenever there is green solved cells. So the cell bacgounds
                                 // should be drawn separately, BEFORE the "DrawSolvableText" and "DrawNumbers" methods...
            DrawNumbers();
            DrawSolvedText();

            if (_GridStatusDisplay != null)
                _GridStatusDisplay.Redraw();

            Refresh();
        }


        /// <summary>
        /// Draw the 3x3 slightly darker background blocks; one on top row and bottom row, and two on the middle row
        /// </summary>
        /// <param name="GridWidth"></param>
        /// <param name="GridHeight"></param>
        private void DrawBackgroundBlocks()
        {
            // Top row:
            drawingGraphics.FillRectangle(_grayBackgroundBrush, Dimensions.TopBlock);

            // Center left
            drawingGraphics.FillRectangle(_grayBackgroundBrush, Dimensions.LeftBlock);

            // Center right:
            drawingGraphics.FillRectangle(_grayBackgroundBrush, Dimensions.RightBlock);

            // Center bottom:
            drawingGraphics.FillRectangle(_grayBackgroundBrush, Dimensions.BottomBlock);
        }

        /// <summary>
        /// Draw the grid lines dividing the area to 9 x 9 squares
        /// </summary>
        private void DrawGridLines()
        {
            for (int x = 1; x < 9; x++)
            {
                float lineX = (Dimensions.GridWidth / 9) * x;
                if (x % 3 == 0)
                    drawingGraphics.DrawLine(GridLineDarkPen, lineX, 1, lineX, Dimensions.GridHeight - 1);
                else
                    drawingGraphics.DrawLine(GridLinePen, lineX, 1, lineX, Dimensions.GridHeight - 1);
            }

            for (int y = 1; y < 9; y++)
            {
                float lineY = (Dimensions.GridHeight / 9) * y;
                if (y % 3 == 0)
                    drawingGraphics.DrawLine(GridLineDarkPen, 1, lineY, Dimensions.GridWidth - 1, lineY);
                else
                    drawingGraphics.DrawLine(GridLinePen, 1, lineY, Dimensions.GridWidth - 1, lineY);
            }
        }


        /// <summary>
        /// Highlight selected cell, if it is set
        /// </summary>
        private void DrawSelectedCell()
        {
            if ((SelectedCell.X < 0) || (SelectedCell.Y < 0))
                return;

            if (Board.Solved)
                return;

            RectangleF rct = GetCellRectangle(SelectedCell.X, SelectedCell.Y);
            drawingGraphics.FillRectangle(_selectedCellBrush, rct);
        }


        /// <summary>
        /// Any cells that have been solved, are drawn with light green background
        /// </summary>
        private void DrawSolvedCellBackgrounds()
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (Board.Data[x, y].Solved)
                    {
                        RectangleF rct = GetCellRectangle(x, y);

                        if (Board.Data[x, y].Solved)
                        {
                            // Cell belongs to a solved line; draw the background in green solved color
                            // but not if this is the currently selected cell, to not override the blue
                            // color:
                            if (x != SelectedCell.X || y != SelectedCell.Y)
                                drawingGraphics.FillRectangle(_solvedLineBackgroundBrush, rct);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Draw the numbers
        /// </summary>
        private void DrawNumbers()
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if ((Board.Data[x, y].Value > 0) || (Board.Data[x, y].Solved) || (ShowPossibleValues))
                    {
                        RectangleF rct = GetCellRectangle(x, y);

                        if (Board.Data[x, y].Value > 0)
                        {
                            // Highlight errors in red
                            if (Board.Data[x, y].Error)
                                drawingGraphics.DrawString(Board.Data[x, y].Value.ToString(), NumberFont, _errorNumberBrush, rct, NumberFormat);

                            // If a number is completely solved, draw it in gray to indicate there's no mor eneed to bother with that number
                            else if (SolvedValues.SolvedCounts[Board.Data[x, y].Value] == 9)
                                drawingGraphics.DrawString(Board.Data[x, y].Value.ToString(), NumberFont, _solvedNumberBrush, rct, NumberFormat);

                            // Highlight cells with the same value as the current one, in blue
                            else if ((Board.Data[x, y].Value == CurrentlySelectedValue) && (!Board.Solved))
                                drawingGraphics.DrawString(Board.Data[x, y].Value.ToString(), NumberFont, _sameAsSelectedBrush, rct, NumberFormat);

                            // Highlight last given hint in green:
                            else if ((Board.Data[x, y].HintCell == true) && (!Board.Solved))
                                drawingGraphics.DrawString(Board.Data[x, y].Value.ToString(), NumberFont, _hintCellBrush, rct, NumberFormat);

                            // Fixed cells, i.e. ones set by the engine as preset
                            else if ((Board.Data[x, y].Fixed == true) && (!Board.Solved))
                                drawingGraphics.DrawString(Board.Data[x, y].Value.ToString(), NumberFont, _fixedNumberBrush, rct, NumberFormat);

                            // Draw numbers normally, black
                            else
                                drawingGraphics.DrawString(Board.Data[x, y].Value.ToString(), NumberFont, _numberBrush, rct, NumberFormat);
                        }
                        if (ShowPossibleValues)
                            DrawPossibleValues(x, y, rct);
                    }
                }
            }
        }

        /// <summary>
        /// Helper method for creating a rectangle from given cell coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private RectangleF GetCellRectangle(int x, int y)
        {
            return new RectangleF(
                Dimensions.CellXPosition[x],
                Dimensions.CellYPosition[y],
                Dimensions.CellWidth,
                Dimensions.CellHeight);

        }

        /// <summary>
        /// Draw a string "under" numbers, indicating solvable / unsolvable status.
        /// </summary>
        private void DrawSolvableText()
        {
            if (SolvableText == "")
                return;

            drawingGraphics.DrawString(SolvableText, SolvableFont, SolvableFontBrush, this.ClientRectangle, NumberFormat);
        }


        /// <summary>
        /// Draw an overlay on top of everything else, showing that the puzzle has been "SOLVED".
        /// </summary>
        private void DrawSolvedText()
        {
            if (Solved())
                drawingGraphics.DrawString("SOLVED", SolvedFont, SolvedBrush, this.ClientRectangle, NumberFormat);
        }




        /// <summary>
        /// Draw all "possible" values of a cell, with small font
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rct"></param>
        private void DrawPossibleValues(int x, int y, RectangleF rct)
        {
            // X and Y of each "piece" of a cell, in which a number is drawn
            int pieceX = 0;
            int pieceY = 0;

            for (int i = 1; i < 10; i++)
            {
                if (Board.Data[x, y].PossibleValues[i] == true)
                {
                    RectangleF pieceRectangle = new RectangleF((pieceX * Dimensions.HintBlockWidth) + rct.Left,
                        (pieceY * Dimensions.HintBlockHeight) + rct.Top,
                        Dimensions.HintBlockWidth,
                        Dimensions.HintBlockHeight);
                    drawingGraphics.DrawString(i.ToString(), PossibleValueFont, _numberBrush, pieceRectangle, NumberFormat);
                }

                // Advance x and y as needed:
                pieceX++;
                if (pieceX > 2)
                {
                    pieceX = 0;
                    pieceY++;
                }
            }
        }

        #endregion


        #region Public methods for interfacing with the owning form

        /// <summary>
        /// Clear the grid. Set all timers and counters to zero.
        /// </summary>
        public void Clear()
        {
            Board.Clear();
            MovesUsed = 0;
            SolvableText = "";
            SolvableTimer.Enabled = false;
            StartTime = DateTime.Now;
            Redraw();
        }


        /// <summary>
        /// Initialise a new game, using one of the hard-coded preset layouts.
        /// </summary>
        public void InitialiseRandomSampleGame()
        {
            Clear();

            GameLayouts layouts = new GameLayouts();
            layouts.SetRandomLayout(Board);

            Redraw();
        }


        /// <summary>
        /// Randomize a new board.
        /// </summary>
        public void RandomizeNew()
        {
            Clear();
            Board.RandomizeNew();
            Redraw();
        }


        public bool Solved()
        {
            return Board.Solved;
        }

        #endregion

        #region Mouse and keyboard handling

        /// <summary>
        /// Mouse click; set active cell and redraw to indicate it
        /// </summary>
        private void SudokuGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (Dimensions.GetSelectedXYFromMouseCoords(e.X, e.Y, SelectedCell))
            {
                UpdateLastSelectedValue();
                Redraw();
            }
        }

        /// <summary>
        /// Mouse double click. If current cell value is 0 AND LastSelectedValue is not 0,
        /// set a value
        /// </summary>
        private void SudokuGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Dimensions.GetSelectedXYFromMouseCoords(e.X, e.Y, SelectedCell))
            {
                if ((CurrentlySelectedValue == 0) && (LastSelectedValue > 0))
                {
                    AssignCurrentCellValue(LastSelectedValue);
                    Redraw();
                }
            }
        }

        private void UpdateLastSelectedValue()
        {
            if (CurrentlySelectedValue > 0)
                LastSelectedValue = CurrentlySelectedValue;
        }


        /// <summary>
        /// Capture ALL key presses
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                SelectedCell.Y--;
                if (SelectedCell.Y < 0)
                    SelectedCell.Y = 8;
                if (SelectedCell.X < 0)  // This will bring the selection to visibility 
                    SelectedCell.X = 0;

                UpdateLastSelectedValue();

                Redraw();
            }
            else if (keyData == Keys.Down)
            {
                SelectedCell.Y++;
                if (SelectedCell.Y > 8)
                    SelectedCell.Y = 0;
                if (SelectedCell.X < 0)  // This will bring the selection to visibility 
                    SelectedCell.X = 0;

                UpdateLastSelectedValue();

                Redraw();
            }
            else if (keyData == Keys.Left)
            {
                SelectedCell.X--;
                if (SelectedCell.X < 0)
                    SelectedCell.X = 8;
                if (SelectedCell.Y < 0)
                    SelectedCell.Y = 0;

                UpdateLastSelectedValue();

                Redraw();
            }
            else if (keyData == Keys.Right)
            {
                SelectedCell.X++;
                if (SelectedCell.X > 8)
                    SelectedCell.X = 0;
                if (SelectedCell.Y < 0)
                    SelectedCell.Y = 0;

                UpdateLastSelectedValue();

                Redraw();
            }
            else if ((keyData == Keys.NumPad0) || (keyData == Keys.D0) || (keyData == Keys.Back) || (keyData == Keys.Delete))
                AssignCurrentCellValue(0);
            else if ((keyData == Keys.NumPad1) || (keyData == Keys.D1))
                AssignCurrentCellValue(1);
            else if ((keyData == Keys.NumPad2) || (keyData == Keys.D2))
                AssignCurrentCellValue(2);
            else if ((keyData == Keys.NumPad3) || (keyData == Keys.D3))
                AssignCurrentCellValue(3);
            else if ((keyData == Keys.NumPad4) || (keyData == Keys.D4))
                AssignCurrentCellValue(4);
            else if ((keyData == Keys.NumPad5) || (keyData == Keys.D5))
                AssignCurrentCellValue(5);
            else if ((keyData == Keys.NumPad6) || (keyData == Keys.D6))
                AssignCurrentCellValue(6);
            else if ((keyData == Keys.NumPad7) || (keyData == Keys.D7))
                AssignCurrentCellValue(7);
            else if ((keyData == Keys.NumPad8) || (keyData == Keys.D8))
                AssignCurrentCellValue(8);
            else if ((keyData == Keys.NumPad9) || (keyData == Keys.D9))
                AssignCurrentCellValue(9);

            return true;  // true = handled; prevents anything else from getting the key press
        }


        #endregion

        /// <summary>
        /// Set a given value to a cell; 0 (zero) to empty cell. Causes a validation of
        /// all cell values, and a redraw
        /// </summary>
        /// <param name="NewValue"></param>
        private void AssignCurrentCellValue(int NewValue)
        {
            if ((SelectedCell.X >= 0) && (SelectedCell.Y >= 0))
            {
                Board.ClearHintValues();
                Board.SetCellValue(SelectedCell.X, SelectedCell.Y, NewValue);

                UpdateLastSelectedValue();

                MovesUsed++;

                Board.ValidateAndMarkErrors();
                Redraw();
            }
        }


        #region Solving and hint code

        /// <summary>
        /// Attempt to solve a single hint cell; stop if can't find any.
        /// TODO: Randomize the order in which lines are processed so it won't always
        /// start from top to bottom lines...
        /// </summary>
        public bool SolveSingleHintCell()
        {
            if (Board.SolveSingleCell())
            {
                MovesUsed++;
                Redraw();
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// Solve the whole grid using logic. Return true / false based on if it was a success or not.
        /// </summary>
        /// <returns></returns>
        public bool SolveAll()
        {
            bool tmpResult = Board.SolveAll();
            Redraw();
            return tmpResult;
        }


        /// <summary>
        /// Try to solve the grid without actuall solving it.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfSolvable()
        {
            bool tmpResult = Board.TrySolveAll();

            // If solvable, set the indicator to write the "solvable" text on the board.
            SetSolvableStatus(tmpResult);

            return tmpResult;
        }

        #endregion



        #region "Solvable" text drawing, fading etc

        /// <summary>
        /// Write a large text "SOLVABLE" across the board background. Set up a timer to fade that text away.
        /// </summary>
        private void SetSolvableStatus(bool isSolvable)
        {
            _solvableTimerStep = 0;
            SolvableTimer.Interval = 3000;
            SolvableTimer.Enabled = true;

            // TODO: call a method to set font sizes, this same logic is now in two places...

            if (isSolvable)
            {
                SolvableText = "SOLVABLE";
                SolvableFontBrush = new SolidBrush(Color.LightGreen);
                if (Width > 0)
                    SolvableFont = new Font("Tahoma", Width / 12, FontStyle.Bold);
            }
            else
            {
                SolvableText = "Not solvable";
                SolvableFontBrush = new SolidBrush(Color.Salmon);
                if (Width > 0)
                    SolvableFont = new Font("Tahoma", Width / 15, FontStyle.Bold);
            }

            Redraw();
        }


        /// <summary>
        /// Calculate a new color from an existing one, decreasing the Alpha value to make it more "invisible"
        /// </summary>
        /// <param name="OriginalColor"></param>
        /// <param name="brightnessFactor"></param>
        /// <returns></returns>
        private Color AdjustBrightness(Color OriginalColor)
        {
            int newAlpha = OriginalColor.A - 25;
            if (newAlpha > 255)
                newAlpha = 255;

            Color adjustedColour = Color.FromArgb(newAlpha,
                OriginalColor.R, OriginalColor.G, OriginalColor.B);
            return adjustedColour;
        }

        /// <summary>
        /// SolvableTimer is used to draw the big overlay "Solvable" / "Not solvable" text; when the timer ticks, the
        /// text is removed (grid is redrawn).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolvableTimer_Tick(object sender, EventArgs e)
        {
            // Stop timer, disable the text and redraw
            _solvableTimerStep++;
            if (_solvableTimerStep > 8)
            {
                // 10 steps should be enough
                SolvableTimer.Stop();
                SolvableText = "";
            }
            else if (_solvableTimerStep == 1)
            {
                SolvableTimer.Stop();
                SolvableTimer.Interval = 200;  // 5 ticks per secong
                SolvableTimer.Start();
            }
            else
            {
                // Fade out. Calculate RGB value of current font
                SolvableFontBrush = new SolidBrush(AdjustBrightness(new Pen(SolvableFontBrush).Color));
            }
            Redraw();
        }

        #endregion

        /// <summary>
        /// Access method for the private SolvedValues array.
        /// </summary>
        /// <returns></returns>
        public SolvedValuesData GetSolvedValues()
        {
            return SolvedValues;
        }


        /// <summary>
        /// Undo a move; rewind last move from undo-list, and redraw if there was anything to undo - if not, just skip.
        /// </summary>
        public void Undo()
        {
            if (Board.Undo())
            {
                MovesUsed++;
                Redraw();
            }
        }


        #region Save and Load, and checking if modified

        /// <summary>
        /// Check if the board has been modified, before a destructive event (exit application, start new game etc). 
        /// Prompt to save and return true if saved, or if user says to ignore.
        /// </summary>
        /// <returns></returns>
        public bool CheckForModified()
        {
            // Don't prompt when solved, otherwise there is always a prompt after finishing a game and exiting game.
            if ((Board.Modified) && (!Board.Solved))
            {
                DialogResult res = MessageBox.Show("Game has been modified. Do you wish to save the game before continuing?", "Confirm", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Cancel)
                    return false;  // User cancels -> not ok to proceed
                else if (res == DialogResult.No)
                    return true;  // Don't want to save -> ok to proceed
                else
                {
                    // Save the game; return true only if it was actually saved:
                    return SaveGameToFile();
                }
            }
            else
                return true;  // Not modified, ok to continue
        }

        /// <summary>
        /// Save a game to file
        /// </summary>
        /// <param name="FileName"></param>
        public bool SaveGameToFile()
        {
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Filter = "Sudoku game files (*.sud)|*.sud|All files (*.*)|*.*";
            sDialog.DefaultExt = "sud";
            if (sDialog.ShowDialog() == DialogResult.OK)
            {
                GameFileHandler fileHandler = new GameFileHandler();
                fileHandler.SaveToFile(Board, sDialog.FileName);
                Board.Modified = false;
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// Load a game from a file
        /// </summary>
        /// <param name="FileName"></param>
        public void LoadGameFromFile()
        {
            OpenFileDialog OpenDialog = new OpenFileDialog();
            OpenDialog.Filter = "Sudoku game files (*.sud)|*.sud|All files (*.*)|*.*";
            OpenDialog.DefaultExt = "sud";
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                GameFileHandler fileHandler = new GameFileHandler();
                if (fileHandler.LoadFromFile(Board, OpenDialog.FileName))
                {
                    Board.ValidateAndMarkErrors();
                    Board.Modified = false;
                    Redraw();
                }
            }
        }

        #endregion
    }
}
