using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;

namespace SpreadSheetUnitTests
{
    [TestClass]
    public class SpreadSheetTests
    {

        [TestMethod]
        public void TestDefaultCtor()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            Assert.AreEqual("", spreadSheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void BasicValidTextTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "test");
            Assert.AreEqual("test", spreadSheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void BasicModifyText()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "test");
            spreadSheet.SetContentsOfCell("A1", "nope");
            Assert.AreEqual("nope", spreadSheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void BasicModifyDouble()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "1.1");
            spreadSheet.SetContentsOfCell("A1", "1337");
            Assert.AreEqual(1337.0, spreadSheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void BasicModifyFormula()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "=A2 + A3");
            spreadSheet.SetContentsOfCell("A1", "=B1 +B2");
            Assert.AreEqual("B1+B2", spreadSheet.GetCellContents("A1").ToString());
        }

        [TestMethod]
        public void BasicValidEmptyTextTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "");
            Assert.AreEqual("", spreadSheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void BasicValidDoubleTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("B2", "4.2");
            Assert.AreEqual(4.2, spreadSheet.GetCellContents("B2"));
        }

        [TestMethod]
        public void BasicValidFormulaTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "=X1 + B3");
            Assert.AreEqual("X1+B3", spreadSheet.GetCellContents("A1").ToString());
        }

        [TestMethod]
        public void BasicGetValue()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "3.1");
            Assert.AreEqual(3.1, spreadSheet.GetCellValue("A1"));
        }


        [TestMethod]
        public void MixedData()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "1");
            spreadsheet.SetContentsOfCell("A2", "Potato");
            spreadsheet.SetContentsOfCell("A3", "=A1+A2");
            Assert.IsInstanceOfType(spreadsheet.GetCellValue("A3"), typeof(FormulaError));
        }

        [TestMethod]
        public void GetValueAndUpdate()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "3.1");
            spreadSheet.SetContentsOfCell("B1", "1.1");
            spreadSheet.SetContentsOfCell("C1", "=A1 + B1");
            Assert.AreEqual(4.2, spreadSheet.GetCellValue("C1"));
        }

        [TestMethod]
        public void UndefinedVariablesFormulaError()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "=X1+X2");
            Assert.IsInstanceOfType(spreadSheet.GetCellValue("A1"), typeof(FormulaError));
        }

        [TestMethod]
        public void FormulaErrorDivideby0()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "=X1/X2");
            spreadSheet.SetContentsOfCell("X1", "1.1");
            spreadSheet.SetContentsOfCell("X2", "0");
            Assert.IsInstanceOfType(spreadSheet.GetCellValue("A1"), typeof(FormulaError));
        }

        [TestMethod]
        public void CheckVersion()
        {
            Spreadsheet spreadSheet = new Spreadsheet(x => true, x=> x, "test");
            spreadSheet.Save("test1.txt");
            Assert.AreEqual("test", spreadSheet.GetSavedVersion("test1.txt"));
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void CheckMismatchVersion()
        {
            Spreadsheet spreadSheet = new Spreadsheet(x => true, x => x, "test");
            spreadSheet.Save("test1.txt");
            Spreadsheet tester = new Spreadsheet("Fail", x => true, x => x, "test1.txt");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TryToStartMismatchVersions()
        {
            Spreadsheet spreadSheet = new Spreadsheet("v1",x => true, x => x, "test");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void BadInput()
        {
            Spreadsheet spreadSheet = new Spreadsheet(x => x.Equals("A"), x => x, "test");
            spreadSheet.SetContentsOfCell("A1", "B");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void BadFilePath()
        {
            Spreadsheet spreadSheet = new Spreadsheet("",x => x.Equals("A"), x => x, "test");
        }

        [TestMethod]
        public void SaveWithAllKindsOfData()
        {
            Spreadsheet spreadSheet = new Spreadsheet(x=> true, x => x, "test");
            spreadSheet.SetContentsOfCell("A1", "=B2+B3");
            spreadSheet.SetContentsOfCell("A2", "4.1");
            spreadSheet.SetContentsOfCell("A3", "test");
            spreadSheet.Save("Whatever.txt");

            //throwing error, checked file manually, looked good
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
 
        public void ReadWriteNull()
        {
            Spreadsheet spreadSheet = new Spreadsheet(x => true, x => x, "test");
            spreadSheet.Save(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]

        public void XMLError()
        {
            Spreadsheet spreadSheet = new Spreadsheet(x => true, x => x, "test");
            spreadSheet.Save("C:\\Potato");
            Spreadsheet test = new Spreadsheet("C:\\Potato", x => true, x => x, "");
        }

    }
}