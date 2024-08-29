using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinForms_Sudoku;

namespace WinForms_Sudoku.Tests
{
    [TestClass()]
    public class GridDimensionsTests
    {
        [TestMethod()]
        public void GridDimensions_background_rectangle_is_roughly_of_correct_size()
        {
            SudokuGrid Grid = new SudokuGrid();
            GridDimensions Dimensions = new GridDimensions();
            Dimensions.CalculateDimensions(Grid);

            int WidthDelta = Math.Abs(Grid.Width - Dimensions.GridWidth);
            int HeightDelta = Math.Abs(Grid.Height - Dimensions.GridHeight);

            Assert.IsTrue(WidthDelta < 10);
            Assert.IsTrue(HeightDelta < 10);
        }

        [TestMethod()]
        public void GridDimensions_cell_size_is_roughly_correct()
        {
            SudokuGrid Grid = new SudokuGrid();
            GridDimensions Dimensions = new GridDimensions();
            Dimensions.CalculateDimensions(Grid);

            int WidthDelta = Math.Abs(Grid.Width - (int)(Dimensions.CellWidth * 9));
            int HeightDelta = Math.Abs(Grid.Height - (int)(Dimensions.CellHeight * 9));

            Assert.IsTrue(WidthDelta < 10);
            Assert.IsTrue(HeightDelta < 10);
        }


        #region X and Y coordinate calculations from mouse coordinates

        [TestMethod()]
        public void GetSelectedXYFromMouseCoords_returns_top_left_cell_for_small_basic_values()
        {
            GridDimensions Dimensions = new GridDimensions();
            Dimensions.CellWidth = 10;
            Dimensions.CellHeight = 10;

            Coordinate coord = new Coordinate();

            // Mouse pos 5, 5 fits top left square
            bool res = Dimensions.GetSelectedXYFromMouseCoords(5, 5, coord);

            Assert.IsTrue(res);
            Assert.AreEqual(0, coord.X);
            Assert.AreEqual(0, coord.Y);
        }


        [TestMethod()]
        public void GetSelectedXYFromMouseCoords_returns_bottom_last_cell_for_max_values()
        {
            GridDimensions Dimensions = new GridDimensions();
            Dimensions.CellWidth = 10;
            Dimensions.CellHeight = 10;

            Coordinate coord = new Coordinate();

            // Mouse pos 89, 89 fits bottom right square
            bool res = Dimensions.GetSelectedXYFromMouseCoords(89, 89, coord);

            Assert.IsTrue(res);
            Assert.AreEqual(8, coord.X);
            Assert.AreEqual(8, coord.Y);
        }

        [TestMethod()]
        public void GetSelectedXYFromMouseCoords_returns_false_when_X_coordinate_outside_grid()
        {
            GridDimensions Dimensions = new GridDimensions();
            Dimensions.CellWidth = 10;
            Dimensions.CellHeight = 10;

            Coordinate coord = new Coordinate();

            // Mouse pos 90, 5 goes outside right edge
            bool res = Dimensions.GetSelectedXYFromMouseCoords(90, 5, coord);

            Assert.IsFalse(res);

            // Make sure the default -1, -1 is not altered
            Assert.AreEqual(-1, coord.X);
            Assert.AreEqual(-1, coord.Y);
        }

        [TestMethod()]
        public void GetSelectedXYFromMouseCoords_returns_false_when_Y_coordinate_outside_grid()
        {
            GridDimensions Dimensions = new GridDimensions();
            Dimensions.CellWidth = 10;
            Dimensions.CellHeight = 10;

            Coordinate coord = new Coordinate();

            // Mouse pos 5, 90 goes outside right edge
            bool res = Dimensions.GetSelectedXYFromMouseCoords(5, 90, coord);

            Assert.IsFalse(res);

            // Make sure the default -1, -1 is not altered
            Assert.AreEqual(-1, coord.X);
            Assert.AreEqual(-1, coord.Y);
        }

        #endregion
    }
}
