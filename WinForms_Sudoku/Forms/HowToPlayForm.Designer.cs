namespace WinForms_Sudoku
{
    partial class HowToPlayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            browser = new WebBrowser();
            btnCloseHelp = new Button();
            SuspendLayout();
            // 
            // browser
            // 
            browser.AllowNavigation = false;
            browser.AllowWebBrowserDrop = false;
            browser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            browser.IsWebBrowserContextMenuEnabled = false;
            browser.Location = new Point(4, 4);
            browser.MinimumSize = new Size(20, 20);
            browser.Name = "browser";
            browser.Size = new Size(1075, 718);
            browser.TabIndex = 0;
            browser.TabStop = false;
            // 
            // btnCloseHelp
            // 
            btnCloseHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCloseHelp.Location = new Point(959, 728);
            btnCloseHelp.Name = "btnCloseHelp";
            btnCloseHelp.Size = new Size(112, 34);
            btnCloseHelp.TabIndex = 1;
            btnCloseHelp.Text = "Close";
            btnCloseHelp.UseVisualStyleBackColor = true;
            btnCloseHelp.Click += btnCloseHelp_Click;
            // 
            // HowToPlayForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1083, 768);
            Controls.Add(btnCloseHelp);
            Controls.Add(browser);
            Name = "HowToPlayForm";
            Text = "How To Play";
            Shown += HowToPlayForm_Shown;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.Button btnCloseHelp;
    }
}