using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms_Sudoku
{
    /// <summary>
    /// This class handles loading and saving games, to and from file.
    /// </summary>
    public class GameFileHandler
    {
        public string ErrorMessage = "";


        #region Loading game from file

        public bool LoadFromFile(GameData Board, string FileName)
        {
            // Check that a file exists
            //   if not, bail out - do not change current but return an error
            if ((FileName == null) || (!File.Exists(FileName)))
            {
                ErrorMessage = "File not found";
                return false;
            }

            // Load file to strings[]
            string[] FileContents = File.ReadAllLines(FileName);
            if (FileIsValid(FileContents, out string CellValues, out string FixedValues) != true)
            {
                ErrorMessage = "File is not valid Sudoku game file.";
                return false;
            }

            if (Board == null)
                Board = new GameData();
            else
                Board.Clear();

            ParseCellValuesToBoard(Board, CellValues, FixedValues);

            return true;
        }


        /// <summary>
        /// CellValues and FixedValues now contain correct strings of values; make a board out of them.
        /// </summary>
        /// <param name="Board"></param>
        public void ParseCellValuesToBoard(GameData Board, string cellValues, string fixedValues)
        {
            GameLayouts Layouts = new GameLayouts();
            Layouts.SetLayoutFromString(Board, cellValues);

            // Assign fixed cell values based on non-zero values in the FixedValues string:
            if (fixedValues == "")
                fixedValues = "000000000000000000000000000000000000000000000000000000000000000000000000000000000";

            int i = 0;
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    // Set the fixed value; if "0" -> false, otherwise true
                    Board.Data[x, y].Fixed = (fixedValues[i] == '1');
                    i++;
                }
            }
        }


        /// <summary>
        /// Validate file contents. Check that exactly 2 lines of text that are not empty and do not start
        /// with # character (comment) exist, and that they both contain only numbers.
        /// </summary>
        /// <param name="FileContent"></param>
        /// <returns></returns>
        public bool FileIsValid(string[] FileContent, out string cellValues, out string fixedValues)
        {
            cellValues = "";
            fixedValues = "";

            if (FileContent == null)
                return false;

            // Check that exactly 2 strings are found, which are non-empty and do not start with # character (which is a comment)
            // Check the 2 string lentghs, and that they only contain valid numbers

            foreach (string line in FileContent)
            {
                if (!line.Trim().StartsWith("#"))
                {
                    if (line.Trim().Length == 81)
                    {
                        if (cellValues == "")
                            cellValues = line.Trim();
                        else if (fixedValues == "")
                            fixedValues = line.Trim();
                        else
                            return false;  // Too many non-empty lines not starting with # character
                    }
                }
            }

            // If both strings have been set, validate them now:
            if (cellValues != "" && fixedValues != "")
            {
                HelperMethods helper = new HelperMethods();
                if ((helper.IsDigitsOnly(cellValues)) && (helper.IsZeroOrOneOnly(fixedValues)))
                    return true;
            }
            else if (cellValues != "")
            {
                // Having just the game cell values set is acceptable as well, no "givens" needed:
                HelperMethods helper = new HelperMethods();
                if (helper.IsDigitsOnly(cellValues))
                    return true;
            }

            // Either one or both strings not set, or one or both contain values other than numbers
            return false;
        }

        #endregion


        #region Saving a game to a file

        /// <summary>
        /// Save a game layout to a file
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveToFile(GameData Board, string FileName)
        {
            // This is basically board.tostring() and then another line of bits (1 or 0) for fixed cells
            List<string> FileData = new List<string>();
            FileData.Add("# Sudoku game");
            FileData.Add("# Saved on " + DateTime.Now.ToString());
            FileData.Add(" ");
            FileData.Add("# Cell values:");
            FileData.Add(Board.ToString());
            FileData.Add(" ");
            FileData.Add("# Fixed:");
            FileData.Add(Board.FixedToString());
            FileData.Add(" ");
            FileData.Add("# end");

            File.WriteAllLines(FileName, FileData);
        }

        #endregion
    }
}
