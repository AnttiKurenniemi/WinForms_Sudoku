namespace WinForms_Sudoku
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Form events

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !MainGrid.CheckForModified();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if ((bool)Properties.Settings.Default["HowToPlayShown"] == false)
            {
                // Show help window, but only once automatically:
                HowToPlayForm howToPlay = new HowToPlayForm();
                howToPlay.Show();

                // Update the setting to not show the help again automatically:
                Properties.Settings.Default["HowToPlayShown"] = true;
                Properties.Settings.Default.Save();
            }
        }

        #endregion


        #region Buttons on main form
        private void btnFileMenu_Click(object sender, EventArgs e)
        {
            FileMenu.Show(btnFileMenu, new Point(0, btnFileMenu.Height));
        }

        private void btnSetRandomPreset_Click(object sender, EventArgs e)
        {
            if (MainGrid.CheckForModified())
                MainGrid.InitialiseRandomSampleGame();
        }

        private void btnSetRandomLayout_Click(object sender, EventArgs e)
        {
            if (MainGrid.CheckForModified())
                MainGrid.RandomizeNew();
        }

        private void btnHint_Click(object sender, EventArgs e)
        {
            if (!MainGrid.Solved())
            {
                if (!MainGrid.SolveSingleHintCell())
                {
                    MessageBox.Show("Sorry, can't figure out a single cell...");
                }
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            MainGrid.Undo();
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            if (!MainGrid.SolveAll())
                MessageBox.Show("Can't solve using simple logic, sorry.");
        }

        private void btnCheckIfSolvable_Click(object sender, EventArgs e)
        {
            MainGrid.CheckIfSolvable();
        }

        private void cbCheckPossibleValues_CheckedChanged(object sender, EventArgs e)
        {
            MainGrid.ShowPossibleValues = cbShowPossibleValues.Checked;
            MainGrid.Redraw();
            MainGrid.Focus();
        }

        #endregion

        #region Context menu items

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            if (MainGrid.CheckForModified())
                MainGrid.Clear();
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            if (MainGrid.CheckForModified())
                MainGrid.LoadGameFromFile();
        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            MainGrid.SaveGameToFile();
        }

        private void menuItemHelp_Click(object sender, EventArgs e)
        {
            HowToPlayForm howToPlay = new HowToPlayForm();
            howToPlay.Show();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
