using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms_Sudoku
{
    public partial class HowToPlayForm : Form
    {
        public HowToPlayForm()
        {
            InitializeComponent();
        }

        private void btnCloseHelp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HowToPlayForm_Shown(object sender, EventArgs e)
        {
            string HelpFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Help\\HowToPlay.html");
            browser.Navigate(@"file://" + HelpFilePath);
        }
    }
}
