/*
 * Author: Michael Swenson
 * U0585863
 * 10/7/18
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;



namespace SS
{
    // PARAGRAPHS 2 and 3 modified for PS5.
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters,
    /// followed by one or more digits AND it satisfies the predicate IsValid.
    /// For example, "A15", "a15", "XY032", and "BC7" are cell names so long as they
    /// satisfy IsValid.  On the other hand, "Z", "X_", and "hello" are not cell names,
    /// regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized with the Normalize method before it is used by or saved in 
    /// this spreadsheet.  For example, if Normalize is s => s.ToUpper(), then
    /// the Formula "x3+a5" should be converted to "X3+A5" before use.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private DependencyGraph DependencyGraph;
        private Dictionary<string, Cell> cellMap;
        public override bool Changed
        { get; protected set; }

        // ADDED FOR PS5
        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public Spreadsheet()
            : base(s => true, s => s, "Default Ctor Version")
        {
            cellMap = new Dictionary<string, Cell>();
            DependencyGraph = new DependencyGraph();
            Changed = false;
        }
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            cellMap = new Dictionary<string, Cell>();
            DependencyGraph = new DependencyGraph();
            Changed = false;

        }


        // ADDED FOR PS5
        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            cellMap = new Dictionary<string, Cell>();
            DependencyGraph = new DependencyGraph();
            Changed = false;
            if (version != GetSavedVersion(filePath)) throw new SpreadsheetReadWriteException("Filepath provided an incorrect version");
            GetSavedSpreadSheet(filePath);
        }

        // ADDED FOR PS5
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            Dictionary<Type, int> filterMap = new Dictionary<Type, int>()
            {
                {typeof(double), 0},
                {typeof(string), 1},
                {typeof(Formula),2}
            };
            //Validation
            if (filename is null || filename.Equals("")) throw new SpreadsheetReadWriteException("Issue with the file name");


            try
            {
                //from class
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };

                //Using implements Idisoposable meaning we don't have to worry about closing the writer
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    //Like writing the title, every "title will be the same/known except for the version, which will change
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    //Body of the xml file, it will define every cell name and it's contents
                    foreach (string s in cellMap.Keys)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", s);

                        //Need to learn more about var
                        var cellContents = cellMap[s].Contents;

                        //Cool way of filtering data types but may not have been necessary because double and string have the same execution
                        //Additionally, Stackoverflow suggested Enums instead of ints to prevent magic numbers
                        switch (filterMap[cellContents.GetType()])
                        {
                            //double
                            case 0:
                                cellContents = cellMap[s].Contents.ToString();
                                break;
                            //string
                            case 1:
                                cellContents = cellMap[s].Contents.ToString();
                                break;
                            //formula
                            case 2:
                                cellContents = "=" + cellMap[s].Contents;
                                break;
                            default:
                                throw new SpreadsheetReadWriteException("Write couldn't filter the cell type");
                        }
                        writer.WriteElementString("contents", cellContents.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (XmlException)
            {
                throw new SpreadsheetReadWriteException("XML Exception occured while writing to file");
            }
            catch (IOException)
            {
                throw new SpreadsheetReadWriteException("An issue with the file path / name occured while writing");
            }
        }

        /// <summary>
        /// Reads the given file path and will only pull the version number from the given
        /// xml file.  Generally used for checking if a parameter version matches the saved 
        /// version.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                //Multiple catches, no closing needed
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            //Find the first entry, spreadsheet, then get just the version, if anything else happens an error should occur
                            if (reader.Name.Equals("spreadsheet")) return reader["version"];
                            else throw new SpreadsheetReadWriteException("Couldn't retrieve the version");
                        }
                    }
                }
            }
            catch (IOException)
            {
                throw new SpreadsheetReadWriteException("Error Occured getting version");
            }
            catch (XmlException)
            {
                throw new SpreadsheetReadWriteException("Error Occured getting version");
            }
            throw new Exception();
        }


        // ADDED FOR PS5
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content == null) throw new ArgumentNullException();
            CheckNameValidNull(name);

            //If it is empty, no need to track it
            if (content.Equals(String.Empty))
            {
                cellMap.Remove(name);
                DependencyGraph.ReplaceDependents(name, new HashSet<string>());
                return new HashSet<string>(GetCellsToRecalculate(name));
            }
            //This is the gateway to the formula SetCellContents which will deal with variables/updating
            else if (content[0].Equals('=')) return SetCellContents(name, new Formula(content.Substring(1, content.Count() - 1), Normalize, IsValid));

            //Leads to the handler of just setting a cell equal to a number/updating
            else if (Double.TryParse(content, out double result)) return SetCellContents(name, result);

            //If it's not a formula or a double it must be a string, handles SetCells(string,string)
            else return SetCellContents(name, content);

            //If it isn't a double,formula, or string something is wrong
            throw new InvalidNameException();
        }

        // MODIFIED PROTECTION FOR PS5
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            CheckNameValidNull(name);

            //If it exists modify, otherwise add
            if (cellMap.ContainsKey(name)) cellMap[name].Contents = number;
            else cellMap.Add(name, new Cell(name, number, PseudoLookup));

            Changed = true;
            //Update all related dependencies
            RecalculateDependencies(name);

            HashSet<string> dents = new HashSet<string>(GetCellsToRecalculate(name));
            Changed = true;
            return dents;
        }

        // MODIFIED PROTECTION FOR PS5
        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null) throw new ArgumentNullException();
            CheckNameValidNull(name);

            //Same pattern, if it has it modify it, otherwise add it
            if (cellMap.ContainsKey(name)) cellMap[name].Contents = text;
            else cellMap.Add(name, new Cell(name, text, PseudoLookup));

            RecalculateDependencies(name);

            Changed = true;
            HashSet<string> dents = new HashSet<string>(GetCellsToRecalculate(name)) { name };
            return dents;
        }


        // MODIFIED PROTECTION FOR PS5
        /// <summary>
        /// If formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null) throw new ArgumentNullException();
            CheckNameValidNull(name);

            //Each Variable in the formula must be accounted for in the DependencyGraph
            foreach (string var in formula.GetVariables())
                DependencyGraph.AddDependency(name, var);

            //If it has one modify it otherwise add one
            if (cellMap.ContainsKey(name)) cellMap[name].Contents = formula;
            else cellMap.Add(name, new Cell(name, formula, PseudoLookup));

            Changed = true;
            //All dependencies involving updated cell must be re-evaluated
            RecalculateDependencies(name);

            HashSet<string> toreturn = new HashSet<string>(GetCellsToRecalculate(name)) { name };
            return toreturn;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        ///------------------protected-----------------------------------/// 
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            CheckNameValidNull(name);
            return DependencyGraph.GetDependees(name);
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            //Interesting note is that yield returns an Ienumerable
            foreach (string s in cellMap.Keys)
                if (!s.Equals("")) yield return s;
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            CheckNameValidNull(name);
            //If it isn't defined it is empty
            if (!cellMap.ContainsKey(name)) return string.Empty;
            return cellMap[name].Contents;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            CheckNameValidNull(name);
            if (!cellMap.ContainsKey(name)) return string.Empty;

            return cellMap[name].Value;
        }



        /************************ HELPER METHODS ****************************************************************/



        /// <summary>
        /// Basic Input validation Helper used in many cases when a name is 
        /// passed.  Checks to see if it is null, matches the defintion of a cell name (letters followed by numbers),
        /// and is valid based on the passed function.
        /// </summary>
        /// <param name="name"></param>
        private void CheckNameValidNull(string name)
        {
            if (name == null || !Regex.IsMatch(name, @"[a-zA-Z]+\d+") || !IsValid(Normalize(name)))
                throw new InvalidNameException();
        }

        /// <summary>
        /// Used in recalculating cell values.  Checks to see if the 
        /// value is a number and will then return that number;
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double PseudoLookup(string name)
        {
            if (cellMap.ContainsKey(name))
            {
                object currentCellValue = cellMap[name].Value;
                if (currentCellValue is double) return (double)currentCellValue;
            }
            throw new ArgumentException("Value was not a double");
        }

        /// <summary>
        /// Uses the fact that when creating a cell it checks to see
        /// what type the content is and evaluate it if it is a formula
        /// Used to update cells when dependents/dependees changes
        /// </summary>
        /// <param name="name"></param>
        private void RecalculateDependencies(string name)
        {
            foreach (string s in GetCellsToRecalculate(name))
                cellMap[s].Contents = cellMap[s].Contents;
        }

        /// <summary>
        /// Reader that gets data from a xml file to build a spreadsheet
        /// </summary>
        /// <param name="filename"></param>
        private void GetSavedSpreadSheet(string filename)
        {
            using (XmlReader reader = XmlReader.Create(filename))
            {
                string name = "";
                string contents = "";

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name.Equals("spreadsheet")) Version = reader["version"];
                        else if (reader.Name.Equals("cell"))
                        {
                            reader.Read();
                            name = reader.ReadElementContentAsString();
                            contents = reader.ReadElementContentAsString();
                            SetContentsOfCell(name, contents);
                        }
                    }

                }
            }
        }


        /******************  END HELPERS ****************************************/

        /// <summary>
        /// This class is used to generate a map between the name of a cell
        /// and its contents and its values.  A cell also has the special property of
        /// when its contents are assigned it will check to see what it's value type is
        /// and if it is a formula it will be re-evaluated.
        /// </summary>
        private class Cell
        {
            public String Name { get; private set; }
            public object Value { get; private set; }
            private object contents;
            public Func<string, double> MyLookup { get; private set; }

            public object Contents
            {
                get { return contents; }
                set
                {
                    Value = value;
                    //If it is a formula use it as a formula
                    if (value is Formula) Value = (Value as Formula).Evaluate(MyLookup);
                    contents = value;
                }
            }

            //Lookup is needed for evaluation
            public Cell(string name, object contents, Func<string, double> lookup)
            {
                Name = name;
                MyLookup = lookup;
                Contents = contents;

            }

        }
    }

}
