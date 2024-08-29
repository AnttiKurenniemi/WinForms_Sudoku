using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms_Sudoku
{
    public partial class FocuslessButton : System.Windows.Forms.Button
    {
        private ToolTip TooltipControl = new ToolTip();
        private string _TooltipText = "";
        public string TooltipText
        {
            get
            {
                return _TooltipText;
            }
            set
            {
                _TooltipText = value;
                TooltipControl.SetToolTip(this, value);
            }
        }

        public FocuslessButton()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}