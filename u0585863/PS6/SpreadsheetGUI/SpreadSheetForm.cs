/*
 * U0585863
 * Author: Michael Swenson
 * 10/18/18
 * CS3500
 */
using SpreadsheetUtilities;
using SS;
using System;
using System.Speech.Synthesis;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// A spreadsheet GUI that has all the basic evaluation functionality of any spreadsheet application.
    /// Files are opened/saved as .sprd files.  The size of the spreadsheet ranges from A1 - Z99.
    /// Multiple instances of spreadsheets can be opened and if you forget to save your data the application
    /// will catch it.
    /// </summary>
    public partial class SpreadSheetForm : Form
    {
        private Spreadsheet spreadsheet;
        private string filePath;
        private TutorialForm extraHelp = new TutorialForm();

        public SpreadSheetForm()
        {
            InitializeComponent();
            spreadsheetPanel1.SelectionChanged += OnSelectionChanged;
            AcceptButton = setButton;
            filePath = null;
            spreadsheet = new Spreadsheet(s => Regex.IsMatch(s, @"[a-zA-Z]+[1-9]+"), s => s.ToUpper(), "ps6");
            //Start by Default at "A1"
            this.ActiveControl = ContentsTxtBx;
        }

        private void OnSelectionChanged(SpreadsheetPanel ss)
        {
            //Sets Text boxes to red text
            IsTextRed();
            ContentsTxtBx.Focus();

            spreadsheetPanel1.GetSelection(out int col, out int row);
            //Adjust Column to a Letter where 'A' = 65 and row is 0 indexed so +1
            SelectedTxtBx.Text = ("" + ((Char)(col + 65)) + (row + 1));
            char columnLetter = (Char)(col + 65);
            string adjName = "" + columnLetter + (row + 1);

            object value = spreadsheet.GetCellValue(adjName);

            //Label needs to have = appended depending on type
            CheckValueAndSetLabels(adjName);
        }
        /// <summary>
        /// Sets the contents of the spreadsheet panel based on setting the contents  of the spreadsheet.
        /// Uses SetContentsOfCell and then does type checking to see how to handle the result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetButton_Click(object sender, EventArgs e)
        {
            IsTextRed();

            spreadsheetPanel1.GetSelection(out int col, out int row);

            string currentTextInput = ContentsTxtBx.Text;
            char columnLetter = (Char)(col + 65);
            row++;
            string name = "" + (columnLetter) + (row);
            Object value = spreadsheet.GetCellValue(name);
            Object contents = spreadsheet.GetCellContents(name);
            bool exceptionThrown = false;

            //When one cell is updated we need to check all dependents for updating
            ISet<string> allCellsNeedingUpdate = null;

            try
            {
                allCellsNeedingUpdate = spreadsheet.SetContentsOfCell(name, currentTextInput);
            }
            //Just one catch that forces them to retype the bad input
            catch (Exception ex)
            {
                string caption = "Format Exception or Circular Exception";
                MessageBox.Show(ex.Message.ToString(), caption, MessageBoxButtons.OK);
                exceptionThrown = true;
                spreadsheet.SetContentsOfCell(name, "");
            }
            if (exceptionThrown) return;

            //Update all cells depending on the selected cell
            foreach (string dependentCell in allCellsNeedingUpdate)
            {
                int.TryParse(dependentCell.Substring(1), out row);
                col = dependentCell[0] - 65;
                row--;

                CheckTypeAndSetPanel(col, row, dependentCell);
            }
            ValueTxtBx.Clear();
            ContentsTxtBx.Clear();
        }


        /************* EVENTS OUTSIDE MAIN FORM CONTROL ****************/

        /// <summary>
        /// Runs a new instance of a blank spreadsheet and keeps track of
        /// how many instances are running so that forms close individually
        /// rather than the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Taken from Daniel Kopta's Example in PS6Skeleton
            DemoApplicationContext.GetAppContext().RunForm(new SpreadSheetForm());

            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SpeakAsync("I am a new Spreadsheet beep beep");
        }

        /// <summary>
        /// Closes the related spreadsheet form but also checks the state
        /// of the related spreadsheet to see if it has changed by not been saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseToolStripMenuItem_Click(object sender, FormClosingEventArgs e)
        {
            CloseWithoutSaveCheck(e);
        }

        /// <summary>
        /// Opens a .sprd(.XML) file to retrieve a saved spreadsheet and loads it
        /// into a new instance of a spreadsheet form. (Note currently broken)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Defaults to .sprd files but allows a search of all files
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "SpreadSheet Files (*.sprd)|*.sprd|All files(*.*)|*.*",
                DefaultExt = ".sprd",
                Title = "Open Spreadsheet",
                FileName = "",
            };
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            if (dialog.FilterIndex == 1) dialog.AddExtension = true;
            string openPath = dialog.FileName;
            try
            {
                SpreadSheetForm openedSpreadsheetF = new SpreadSheetForm();
                //Professor's Code helping me out
                DemoApplicationContext.GetAppContext().RunForm(openedSpreadsheetF);

                spreadsheet = new Spreadsheet(dialog.FileName, s => Regex.IsMatch(s, @"[a - zA - Z]+[1-9]+"), s => s.ToUpper(), "ps6");
                string value;
                foreach (string s in spreadsheet.GetNamesOfAllNonemptyCells())
                {
                    int.TryParse(s.Substring(1), out int row);
                    int.TryParse(s[0].ToString(), out int col);
                    //Row and Column are zero based indices, the gui Starts at 1
                    row--;

                    if ((spreadsheet.GetCellValue(s) is FormulaError))
                        value = ((FormulaError)spreadsheet.GetCellValue(s)).Reason;
                    else
                        value = spreadsheet.GetCellValue(s).ToString();

                    openedSpreadsheetF.spreadsheetPanel1.SetValue(col, row, value);
                }
            }
            catch (SpreadsheetReadWriteException)
            {
                MessageBox.Show("A file read error has occured, make sure it is a spreadsheet file and the path exists", "File Read Error", MessageBoxButtons.OK);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Filter = "Spreadsheet Files (*.sprd)|*.sprd|All Files (*.*)|*.*",
                DefaultExt = ".sprd",
                Title = "Save",
            };
            if (saveDialog.ShowDialog() == DialogResult.Cancel) return;

            if (saveDialog.FilterIndex == 1)
                saveDialog.AddExtension = true;

            spreadsheet.Save(this.Text);
            filePath = saveDialog.FileName;
        }

        private void ExtraFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DemoApplicationContext.GetAppContext().RunForm(extraHelp);
        }

        private void SpreadSheetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseWithoutSaveCheck(e);
        }


        /***************************************         HELPERS ************************/

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CloseWithoutSaveCheck(FormClosingEventArgs e)
        {
            if (spreadsheet.Changed)
            {
                DialogResult dialogResult = MessageBox.Show("You're about to erase unsaved data. Do you want to continue?", "Unsaved Data", MessageBoxButtons.YesNo);
                e.Cancel = (dialogResult == DialogResult.No);
            }
            extraHelp.Close();
        }


        private void CheckTypeAndSetPanel(int col, int row, string dependentCell)
        {
            if (spreadsheet.GetCellValue(dependentCell) is string)
            {
                //Casting to string is safe because we made sure it was a string first
                spreadsheetPanel1.SetValue(col, row, (string)spreadsheet.GetCellValue(dependentCell));
            }
            else if (spreadsheet.GetCellValue(dependentCell) is FormulaError error)
            {
                spreadsheetPanel1.SetValue(col, row, error.Reason);
            }
            else if (spreadsheet.GetCellContents(dependentCell) is Formula)
            {
                spreadsheetPanel1.SetValue(col, row, spreadsheet.GetCellValue(dependentCell).ToString());
            }
            else
            {
                spreadsheetPanel1.SetValue(col, row, spreadsheet.GetCellValue(dependentCell).ToString());
            }
        }

        private void CheckValueAndSetLabels(string adjName)
        {
            if (spreadsheet.GetCellValue(adjName) is Formula)
            {
                ContentsTxtBx.Text = "=" + spreadsheet.GetCellContents(adjName);
                ValueTxtBx.Text = spreadsheet.GetCellValue(adjName).ToString();
            }
            else if (Double.TryParse(spreadsheet.GetCellValue(adjName).ToString(), out double result))
            {
                ContentsTxtBx.Text = "=" + spreadsheet.GetCellContents(adjName).ToString();
                ValueTxtBx.Text = spreadsheet.GetCellValue(adjName).ToString();
            }
            else
            {
                ContentsTxtBx.Text = spreadsheet.GetCellContents(adjName).ToString();
                ValueTxtBx.Text = spreadsheet.GetCellValue(adjName).ToString();
            }
        }

        private void IsTextRed()
        {
            if (checkBoxRed.Checked) ContentsTxtBx.ForeColor = Color.Red;
            else ContentsTxtBx.ForeColor = Color.Black;
        }

    }
}
