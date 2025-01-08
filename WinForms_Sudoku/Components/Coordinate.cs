using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms_Sudoku
{
    /// <summary>
    /// Coordinate is a reference to a cell on the board.
    /// </summary>
    public class Coordinate
    {
        public int X;
        public int Y;


        public Coordinate()
        {
            X = -1;
            Y = -1;
        }
    }
}
