namespace WinForms_Sudoku
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Form events

        /// <summary>
        /// Closing the main form (closing the application). Check if there is an unmodified
        /// game, and if there is ask to save it first.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !MainGrid.CheckForModified();
        }

        /// <summary>
        /// On the first time the app is opened, show the help page. Update a property
        /// to remember that the help page has been shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Open the "File" menu with save and load related items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (MainGrid.InProgress)
                if (!MainGrid.SolveAll())
                    MessageBox.Show("Can't solve using simple logic, sorry.");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            HowToPlayForm howToPlay = new HowToPlayForm();
            howToPlay.Show();
        }

        private void btnCheckIfSolvable_Click(object sender, EventArgs e)
        {
            if (MainGrid.InProgress)
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
