using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms_Sudoku
{
    /// <summary>
    /// This class is simply used to hold the sudoku grid drawing dimensions, such as
    /// block sizes.
    /// </summary>
    public class GridDimensions
    {
        // Full grid dimensions, and the drawing rectangle
        public int GridWidth;
        public int GridHeight;
        public Rectangle BackgroundRectangle;

        // 4 big blocks of 3x3 squares with light gray background, to draw the grid for easier "reading"
        public RectangleF TopBlock;
        public RectangleF LeftBlock;
        public RectangleF RightBlock;
        public RectangleF BottomBlock;

        // Single cell size
        public float CellWidth;
        public float CellHeight;

        // Hint block is a 3 x 3 matrix inside a single cell, where possible available values can be drawn
        public float HintBlockWidth;
        public float HintBlockHeight;

        // X and Y coordinates of each cell in the grid
        public float[] CellXPosition = new float[9];
        public float[] CellYPosition = new float[9];



        /// <summary>
        /// Calculate the cell and cell block (3x3 cells) dimensions. Also calculate the cell positions so
        /// that they are easily and quickly usable in drawing methods, instead of having to calculate them
        /// there all the time.
        /// </summary>
        /// <param name="grid"></param>
        public void CalculateDimensions(SudokuGrid grid)
        {
            // Size. Use this method instead of just width and height, to make the size "snap" so that
            // the last row and column are the same size as the rest of the grid
            GridWidth = Convert.ToInt32((grid.Width / 9) * 9);
            GridHeight = Convert.ToInt32((grid.Height / 9) * 9);
            BackgroundRectangle = new Rectangle(0, 0, GridWidth, GridHeight);

            // Single cells:
            CellWidth = GridWidth / 9.0F;
            CellHeight = GridHeight / 9.0F;

            // Hint blocks within cells:
            HintBlockWidth = CellWidth / 3;
            HintBlockHeight = CellHeight / 3;

            // Cell X and Y positions:
            for (int i = 0; i < 9; i++)
            {
                CellYPosition[i] = (i * CellHeight);
                CellXPosition[i] = (i * CellWidth);
            }

            ResizeBlocks();
        }


        /// <summary>
        /// Calculate the larger 3x3 rectangles with gray background
        /// </summary>
        private void ResizeBlocks()
        {
            TopBlock = new RectangleF(CellWidth * 3, 0, CellWidth * 3, CellHeight * 3);
            LeftBlock = new RectangleF(0, CellHeight * 3, CellWidth * 3, CellHeight * 3);
            RightBlock = new RectangleF(CellWidth * 6, CellHeight * 3, CellWidth * 3, CellHeight * 3);
            BottomBlock = new RectangleF(CellWidth * 3, CellHeight * 6, CellWidth * 3, CellHeight * 3);
        }


        /// <summary>
        /// Calculate grid X and Y position from mouse coordinates. Note that because the grid right and
        /// bottom edge are rounded to nearest whole squares, there is a small gap in which it is possible
        /// to click, which would then cause the X or Y to be greater than 8; in such case, ignore the
        /// coordinate altogether.
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        public bool GetSelectedXYFromMouseCoords(int mouseX, int mouseY, Coordinate selectedCell)
        {
            int tempX = mouseX / (int)CellWidth;
            int tempY = mouseY / (int)CellHeight;
            if (tempX >= 0 && tempX < 9 && tempY >= 0 && tempY < 9)
            {
                selectedCell.X = tempX;
                selectedCell.Y = tempY;
                return true;
            }
            return false;
        }
    }
}
