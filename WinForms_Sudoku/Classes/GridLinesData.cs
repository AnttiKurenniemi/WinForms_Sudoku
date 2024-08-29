﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WinForms_Sudoku
{
    /// <summary>
    /// Clas containing all possible "lines" on the grid, as coordinates. Pre-defined constants for
    /// easier and faster access during runtime.
    /// </summary>
    public class GridLinesData
    {
        public int LineCount { get { return Lines.Length / 18; } }

        private int _LineNumber = 0;
        private int _ElementNumber = 0;

        public int[,,] Lines = { 
            // Horizontal lines
            { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 } },
            { { 0, 1 }, { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 } },
            { { 0, 2 }, { 1, 2 }, { 2, 2 }, { 3, 2 }, { 4, 2 }, { 5, 2 }, { 6, 2 }, { 7, 2 }, { 8, 2 } },
            { { 0, 3 }, { 1, 3 }, { 2, 3 }, { 3, 3 }, { 4, 3 }, { 5, 3 }, { 6, 3 }, { 7, 3 }, { 8, 3 } },
            { { 0, 4 }, { 1, 4 }, { 2, 4 }, { 3, 4 }, { 4, 4 }, { 5, 4 }, { 6, 4 }, { 7, 4 }, { 8, 4 } },
            { { 0, 5 }, { 1, 5 }, { 2, 5 }, { 3, 5 }, { 4, 5 }, { 5, 5 }, { 6, 5 }, { 7, 5 }, { 8, 5 } },
            { { 0, 6 }, { 1, 6 }, { 2, 6 }, { 3, 6 }, { 4, 6 }, { 5, 6 }, { 6, 6 }, { 7, 6 }, { 8, 6 } },
            { { 0, 7 }, { 1, 7 }, { 2, 7 }, { 3, 7 }, { 4, 7 }, { 5, 7 }, { 6, 7 }, { 7, 7 }, { 8, 7 } },
            { { 0, 8 }, { 1, 8 }, { 2, 8 }, { 3, 8 }, { 4, 8 }, { 5, 8 }, { 6, 8 }, { 7, 8 }, { 8, 8 } },

            // Vertical lines
            { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 0, 3 }, { 0, 4 }, { 0, 5 }, { 0, 6 }, { 0, 7 }, { 0, 8 } },
            { { 1, 0 }, { 1, 1 }, { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 5 }, { 1, 6 }, { 1, 7 }, { 1, 8 } },
            { { 2, 0 }, { 2, 1 }, { 2, 2 }, { 2, 3 }, { 2, 4 }, { 2, 5 }, { 2, 6 }, { 2, 7 }, { 2, 8 } },
            { { 3, 0 }, { 3, 1 }, { 3, 2 }, { 3, 3 }, { 3, 4 }, { 3, 5 }, { 3, 6 }, { 3, 7 }, { 3, 8 } },
            { { 4, 0 }, { 4, 1 }, { 4, 2 }, { 4, 3 }, { 4, 4 }, { 4, 5 }, { 4, 6 }, { 4, 7 }, { 4, 8 } },
            { { 5, 0 }, { 5, 1 }, { 5, 2 }, { 5, 3 }, { 5, 4 }, { 5, 5 }, { 5, 6 }, { 5, 7 }, { 5, 8 } },
            { { 6, 0 }, { 6, 1 }, { 6, 2 }, { 6, 3 }, { 6, 4 }, { 6, 5 }, { 6, 6 }, { 6, 7 }, { 6, 8 } },
            { { 7, 0 }, { 7, 1 }, { 7, 2 }, { 7, 3 }, { 7, 4 }, { 7, 5 }, { 7, 6 }, { 7, 7 }, { 7, 8 } },
            { { 8, 0 }, { 8, 1 }, { 8, 2 }, { 8, 3 }, { 8, 4 }, { 8, 5 }, { 8, 6 }, { 8, 7 }, { 8, 8 } },

            // 3x3 blocks:
            { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 0, 1 }, { 1, 1 }, { 2, 1 }, { 0, 2 }, { 1, 2 }, { 2, 2 } }, // Top-left
            { { 3, 0 }, { 4, 0 }, { 5, 0 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 3, 2 }, { 4, 2 }, { 5, 2 } }, // Top-center
            { { 6, 0 }, { 7, 0 }, { 8, 0 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 6, 2 }, { 7, 2 }, { 8, 2 } }, // Top-right

            { { 0, 3 }, { 1, 3 }, { 2, 3 }, { 0, 4 }, { 1, 4 }, { 2, 4 }, { 0, 5 }, { 1, 5 }, { 2, 5 } }, // Center-left
            { { 3, 3 }, { 4, 3 }, { 5, 3 }, { 3, 4 }, { 4, 4 }, { 5, 4 }, { 3, 5 }, { 4, 5 }, { 5, 5 } }, // Center-center
            { { 6, 3 }, { 7, 3 }, { 8, 3 }, { 6, 4 }, { 7, 4 }, { 8, 4 }, { 6, 5 }, { 7, 5 }, { 8, 5 } }, // Center-right

            { { 0, 6 }, { 1, 6 }, { 2, 6 }, { 0, 7 }, { 1, 7 }, { 2, 7 }, { 0, 8 }, { 1, 8 }, { 2, 8 } }, // Bottom-left
            { { 3, 6 }, { 4, 6 }, { 5, 6 }, { 3, 7 }, { 4, 7 }, { 5, 7 }, { 3, 8 }, { 4, 8 }, { 5, 8 } }, // Bottom-center
            { { 6, 6 }, { 7, 6 }, { 8, 6 }, { 6, 7 }, { 7, 7 }, { 8, 7 }, { 6, 8 }, { 7, 8 }, { 8, 8 } }  // Bottom-right
        };


        public GridLinesData LineData(int lineNumber, int elementNumber)
        {
            _LineNumber = lineNumber;
            _ElementNumber = elementNumber;
            return this;
        }

        public int X
        {
            get
            {
                return Lines[_LineNumber, _ElementNumber, 0];
            }
        }
        public int Y
        {
            get
            {
                return Lines[_LineNumber, _ElementNumber, 1];
            }
        }
    }
}