namespace WinForms_Sudoku
{
    /// <summary>
    /// This class represents and action that can be undone; it has cell value, x and y values, the previous
    /// value it replaces and the action (place a number, delete a number). A stack of these values will be
    /// generated when putting values or deleting them during gameplay, and Undo functionality can be done by
    /// simply de-stacking actions from the list.
    /// </summary>
    public class UndoAction
    {
        public int x;
        public int y;
        public int Value;
    }
}
