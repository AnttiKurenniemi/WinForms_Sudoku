namespace WinForms_Sudoku
{
    partial class StatusDisplay
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


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbTime = new Label();
            lbMoves = new Label();
            lbSolved = new Label();
            SuspendLayout();
            // 
            // lbTime
            // 
            lbTime.AutoSize = true;
            lbTime.Location = new Point(0, 0);
            lbTime.Name = "lbTime";
            lbTime.Size = new Size(71, 25);
            lbTime.TabIndex = 0;
            lbTime.Text = "<time>";
            // 
            // lbMoves
            // 
            lbMoves.AutoSize = true;
            lbMoves.Location = new Point(0, 22);
            lbMoves.Name = "lbMoves";
            lbMoves.Size = new Size(89, 25);
            lbMoves.TabIndex = 1;
            lbMoves.Text = "<moves>";
            // 
            // lbSolved
            // 
            lbSolved.AutoSize = true;
            lbSolved.Location = new Point(0, 44);
            lbSolved.Name = "lbSolved";
            lbSolved.Size = new Size(88, 25);
            lbSolved.TabIndex = 2;
            lbSolved.Text = "<solved>";
            // 
            // StatusDisplay
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lbSolved);
            Controls.Add(lbMoves);
            Controls.Add(lbTime);
            Name = "StatusDisplay";
            Size = new Size(247, 338);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbTime;
        private Label lbMoves;
        private Label lbSolved;
    }
}
