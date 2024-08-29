namespace WinForms_Sudoku
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            MainGrid = new SudokuGrid();
            MainStatusDisplay = new StatusDisplay();
            btnFileMenu = new FocuslessButton();
            btnSetRandomPreset = new FocuslessButton();
            btnSetRandomLayout = new FocuslessButton();
            btnHint = new FocuslessButton();
            btnUndo = new FocuslessButton();
            btnSolve = new FocuslessButton();
            btnCheckIfSolvable = new FocuslessButton();
            cbShowPossibleValues = new CheckBox();
            FileMenu = new ContextMenuStrip(components);
            menuItemNew = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuItemOpen = new ToolStripMenuItem();
            menuItemSave = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            menuItemHelp = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            menuItemExit = new ToolStripMenuItem();
            FileMenu.SuspendLayout();
            SuspendLayout();
            // 
            // MainGrid
            // 
            MainGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MainGrid.GridStatusDisplay = MainStatusDisplay;
            MainGrid.Location = new Point(12, 12);
            MainGrid.MovesUsed = 0;
            MainGrid.Name = "MainGrid";
            MainGrid.Size = new Size(727, 742);
            MainGrid.TabIndex = 0;
            // 
            // MainStatusDisplay
            // 
            MainStatusDisplay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            MainStatusDisplay.Location = new Point(753, 410);
            MainStatusDisplay.Name = "MainStatusDisplay";
            MainStatusDisplay.OwnerGrid = MainGrid;
            MainStatusDisplay.Size = new Size(219, 337);
            MainStatusDisplay.TabIndex = 1;
            // 
            // btnFileMenu
            // 
            btnFileMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFileMenu.Location = new Point(753, 12);
            btnFileMenu.Name = "btnFileMenu";
            btnFileMenu.Size = new Size(219, 46);
            btnFileMenu.TabIndex = 2;
            btnFileMenu.TabStop = false;
            btnFileMenu.Text = "File...";
            btnFileMenu.TooltipText = "";
            btnFileMenu.UseVisualStyleBackColor = true;
            btnFileMenu.Click += btnFileMenu_Click;
            // 
            // btnSetRandomPreset
            // 
            btnSetRandomPreset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSetRandomPreset.Location = new Point(753, 64);
            btnSetRandomPreset.Name = "btnSetRandomPreset";
            btnSetRandomPreset.Size = new Size(219, 46);
            btnSetRandomPreset.TabIndex = 3;
            btnSetRandomPreset.TabStop = false;
            btnSetRandomPreset.Text = "Set random preset";
            btnSetRandomPreset.TooltipText = "";
            btnSetRandomPreset.UseVisualStyleBackColor = true;
            btnSetRandomPreset.Click += btnSetRandomPreset_Click;
            // 
            // btnSetRandomLayout
            // 
            btnSetRandomLayout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSetRandomLayout.Location = new Point(753, 115);
            btnSetRandomLayout.Name = "btnSetRandomLayout";
            btnSetRandomLayout.Size = new Size(219, 46);
            btnSetRandomLayout.TabIndex = 4;
            btnSetRandomLayout.TabStop = false;
            btnSetRandomLayout.Text = "Set random layout";
            btnSetRandomLayout.TooltipText = "";
            btnSetRandomLayout.UseVisualStyleBackColor = true;
            btnSetRandomLayout.Click += btnSetRandomLayout_Click;
            // 
            // btnHint
            // 
            btnHint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHint.Location = new Point(753, 167);
            btnHint.Name = "btnHint";
            btnHint.Size = new Size(219, 46);
            btnHint.TabIndex = 5;
            btnHint.TabStop = false;
            btnHint.Text = "Give a hint";
            btnHint.TooltipText = "";
            btnHint.UseVisualStyleBackColor = true;
            btnHint.Click += btnHint_Click;
            // 
            // btnUndo
            // 
            btnUndo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUndo.Location = new Point(753, 219);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(219, 46);
            btnUndo.TabIndex = 6;
            btnUndo.TabStop = false;
            btnUndo.Text = "Undo last move";
            btnUndo.TooltipText = "";
            btnUndo.UseVisualStyleBackColor = true;
            btnUndo.Click += btnUndo_Click;
            // 
            // btnSolve
            // 
            btnSolve.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSolve.Location = new Point(753, 271);
            btnSolve.Name = "btnSolve";
            btnSolve.Size = new Size(219, 46);
            btnSolve.TabIndex = 7;
            btnSolve.TabStop = false;
            btnSolve.Text = "Solve the game";
            btnSolve.TooltipText = "";
            btnSolve.UseVisualStyleBackColor = true;
            btnSolve.Click += btnSolve_Click;
            // 
            // btnCheckIfSolvable
            // 
            btnCheckIfSolvable.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCheckIfSolvable.Location = new Point(753, 323);
            btnCheckIfSolvable.Name = "btnCheckIfSolvable";
            btnCheckIfSolvable.Size = new Size(219, 46);
            btnCheckIfSolvable.TabIndex = 8;
            btnCheckIfSolvable.TabStop = false;
            btnCheckIfSolvable.Text = "Check if solvable";
            btnCheckIfSolvable.TooltipText = "";
            btnCheckIfSolvable.UseVisualStyleBackColor = true;
            btnCheckIfSolvable.Click += btnCheckIfSolvable_Click;
            // 
            // cbShowPossibleValues
            // 
            cbShowPossibleValues.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbShowPossibleValues.AutoSize = true;
            cbShowPossibleValues.Location = new Point(757, 375);
            cbShowPossibleValues.Name = "cbShowPossibleValues";
            cbShowPossibleValues.Size = new Size(207, 29);
            cbShowPossibleValues.TabIndex = 9;
            cbShowPossibleValues.Text = "Show possible values";
            cbShowPossibleValues.UseVisualStyleBackColor = true;
            cbShowPossibleValues.CheckedChanged += cbCheckPossibleValues_CheckedChanged;
            // 
            // FileMenu
            // 
            FileMenu.ImageScalingSize = new Size(24, 24);
            FileMenu.Items.AddRange(new ToolStripItem[] { menuItemNew, toolStripSeparator1, menuItemOpen, menuItemSave, toolStripSeparator2, menuItemHelp, toolStripSeparator3, menuItemExit });
            FileMenu.Name = "FileMenu";
            FileMenu.Size = new Size(258, 182);
            // 
            // menuItemNew
            // 
            menuItemNew.Name = "menuItemNew";
            menuItemNew.Size = new Size(257, 32);
            menuItemNew.Text = "New Game";
            menuItemNew.Click += menuItemNew_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(254, 6);
            // 
            // menuItemOpen
            // 
            menuItemOpen.Name = "menuItemOpen";
            menuItemOpen.Size = new Size(257, 32);
            menuItemOpen.Text = "Load game from file...";
            menuItemOpen.Click += menuItemOpen_Click;
            // 
            // menuItemSave
            // 
            menuItemSave.Name = "menuItemSave";
            menuItemSave.Size = new Size(257, 32);
            menuItemSave.Text = "Save game to file...";
            menuItemSave.Click += menuItemSave_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(254, 6);
            // 
            // menuItemHelp
            // 
            menuItemHelp.Name = "menuItemHelp";
            menuItemHelp.Size = new Size(257, 32);
            menuItemHelp.Text = "Show help";
            menuItemHelp.Click += menuItemHelp_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(254, 6);
            // 
            // menuItemExit
            // 
            menuItemExit.Name = "menuItemExit";
            menuItemExit.Size = new Size(257, 32);
            menuItemExit.Text = "Exit game";
            menuItemExit.Click += menuItemExit_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 760);
            Controls.Add(cbShowPossibleValues);
            Controls.Add(btnCheckIfSolvable);
            Controls.Add(btnSolve);
            Controls.Add(btnUndo);
            Controls.Add(btnHint);
            Controls.Add(btnSetRandomLayout);
            Controls.Add(btnSetRandomPreset);
            Controls.Add(btnFileMenu);
            Controls.Add(MainStatusDisplay);
            Controls.Add(MainGrid);
            Name = "MainForm";
            Text = "Sudoku";
            FormClosing += MainForm_FormClosing;
            Shown += MainForm_Shown;
            FileMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SudokuGrid MainGrid;
        private StatusDisplay MainStatusDisplay;
        private FocuslessButton btnFileMenu;
        private FocuslessButton btnSetRandomPreset;
        private FocuslessButton btnSetRandomLayout;
        private FocuslessButton btnHint;
        private FocuslessButton btnUndo;
        private FocuslessButton btnSolve;
        private FocuslessButton btnCheckIfSolvable;
        private CheckBox cbShowPossibleValues;
        private ContextMenuStrip FileMenu;
        private ToolStripMenuItem menuItemNew;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem menuItemOpen;
        private ToolStripMenuItem menuItemSave;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem menuItemHelp;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem menuItemExit;
    }
}
