using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms_Sudoku.Tests
{
    [TestClass()]
    public class GameFileHandlerTests
    {
        // Several compiler warnings suppressed on purpose, because I specifically want to do stupid
        // stuff in tests, with NULLs especially:

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

        [TestMethod()]
        public void FileIsValid_returns_false_if_string_array_is_null()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            string[] testFile = null;
            Assert.AreEqual(false, fileHandler.FileIsValid(testFile, out string cellValues, out string fixedValues));
        }

        [TestMethod()]
        public void FileIsValid_returns_false_if_string_array_is_empty()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            string[] testFile = new string[] { };
            Assert.AreEqual(false, fileHandler.FileIsValid(testFile, out string cellValues, out string fixedValues));
        }

        [TestMethod()]
        public void FileIsValid_returns_false_if_string_array_contains_only_comment_lines()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            string[] testFile = new string[]
            {
                "#comment line 1",
                "#12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                "#12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"
            };
            Assert.AreEqual(false, fileHandler.FileIsValid(testFile, out string cellValues, out string fixedValues));
        }


        /// <summary>
        /// It is also acceptable that there is only one valid string, if it seems to contain cell values
        /// </summary>
        [TestMethod()]
        public void FileIsValid_returns_true_if_string_array_contains_only_one_valid_string()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            string[] testFile = new string[]
            {
                "#comment line 1",
                "123456789012345678901234567890123456789012345678901234567890123456789012345678901"
            };
            Assert.IsTrue(fileHandler.FileIsValid(testFile, out string cellValues, out string fixedValues));
        }

        [TestMethod()]
        public void FileIsValid_returns_false_if_string_array_contains_more_than_two_valid_strings()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            string[] testFile = new string[]
            {
                "#comment line 1",
                "123456789012345678901234567890123456789012345678901234567890123456789012345678901",
                "010101010101010101010101010101010101010101010101010101010101010101010101010101010",
                "010101010101010101010101010101010101010101010101010101010101010101010101010101010"
            };
            Assert.AreEqual(false, fileHandler.FileIsValid(testFile, out string cellValues, out string fixedValues));
        }

        [TestMethod()]
        public void FileIsValid_returns_false_if_string_array_contains_two_strings_but_one_has_alphabet_characters_in_it()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            string[] testFile = new string[]
            {
                "#comment line 1",
                "123456789012345678901234567890123456789012345678901234567890123456789012345678901",
                "010101010101010101010101010101010101X10101010101010101010101010101010101010101010"
            };
            Assert.AreEqual(false, fileHandler.FileIsValid(testFile, out string cellValues, out string fixedValues));
        }

        [TestMethod()]
        public void FileIsValid_returns_true_if_string_array_contains_correct_lines()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            string[] testFile = new string[]
            {
                "#comment line 1",
                "123456789012345678901234567890123456789012345678901234567890123456789012345678901",
                "# This is another comment line",
                "010101010101010101010101010101010101010101010101010101010101010101010101010101010"
            };
            Assert.AreEqual(true, fileHandler.FileIsValid(testFile, out string cellValues, out string fixedValues));
        }

        [TestMethod()]
        public void FileIsValid_returns_correct_string_values_if_string_array_contains_correct_lines()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            string[] testFile = new string[]
            {
                "#comment line 1",
                "123456789012345678901234567890123456789012345678901234567890123456789012345678901",
                "# This is another comment line",
                "010101010101010101010101010101010101010101010101010101010101010101010101010101010"
            };
            fileHandler.FileIsValid(testFile, out string cellValues, out string fixedValues);

            Assert.AreEqual("123456789012345678901234567890123456789012345678901234567890123456789012345678901", cellValues);
            Assert.AreEqual("010101010101010101010101010101010101010101010101010101010101010101010101010101010", fixedValues);
        }



        [TestMethod()]
        public void LoadFromFile_returns_false_if_file_name_is_null()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            Assert.AreEqual(false, fileHandler.LoadFromFile(null, null));
        }

        [TestMethod()]
        public void LoadFromFile_returns_false_if_file_name_is_impossible()
        {
            GameFileHandler fileHandler = new GameFileHandler();
            Assert.AreEqual(false, fileHandler.LoadFromFile(null, "T:\\this\\path\\hopefully\\does\\not\\exist.xyz"));
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}
