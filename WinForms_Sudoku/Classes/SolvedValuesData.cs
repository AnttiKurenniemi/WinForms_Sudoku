using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms_Sudoku
{
    public class SolvedValuesData
    {
        public int[] SolvedCounts = new int[10];
        public int TotalSolvedCount = 0;


        /// <summary>
        /// Re-calculate all solved value counts on the board.
        /// </summary>
        /// <param name="Board"></param>
        public void Refresh(GameData Board)
        {
            // Start from 0 solved of each number:
            for (int i = 1; i < 10; i++)
                SolvedCounts[i] = 0;

            TotalSolvedCount = 0;

            // Go through board and increase each count:
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (Board.Data[x, y].Value > 0)
                    {
                        SolvedCounts[Board.Data[x, y].Value]++;
                        TotalSolvedCount++;
                    }
                }
            }
        }
    }
}
