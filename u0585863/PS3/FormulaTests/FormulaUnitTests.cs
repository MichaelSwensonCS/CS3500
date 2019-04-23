using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTests
{
    [TestClass]
    public class FormulaUnitTests
    {
        [TestMethod]
        public void TestEqualsNull()
        {
            Formula f = null;
            Assert.IsTrue(f == null);
        }
        [TestMethod]
        public void TestEqualOneNull()
        {
            Formula f = null;
            Formula f2 = new Formula("1+1");
            Assert.IsFalse(f == f2);
        }
        [TestMethod]
        public void TestEqualOneNull2()
        {
            Formula f = null;
            Formula f2 = new Formula("1+1");
            Assert.IsFalse(f2 == f);
        }


        [TestMethod]
        public void TestNotEqualsNull()
        {
            Formula f = null;
            Assert.IsFalse(f != null);
        }

        [TestMethod]
        public void TestEquals()
        {
            Formula f = new Formula("1", s => s, x => true);
            Formula one = new Formula("1", s => s, x => true);
            Assert.IsTrue(f.Equals(one));
        }

        [TestMethod]
        public void TestEqualsNullToNull()
        {
            Formula f = null;
            Formula f1 = null;
            Assert.IsTrue(f == f1);
        }

        [TestMethod]
        public void TestBasicValidFormula()
        {
            Formula f = new Formula("5 + 3");
            Assert.IsTrue("5+3".Equals(f.ToString()));
        }

        [TestMethod]
        public void TestFullFormula()
        {
            Formula f = new Formula("X5 + (_X3 *  34) / 2.0 + (4) - A1");
            Assert.IsTrue("X5+(_X3*34)/2.0+(4)-A1".Equals(f.ToString()));
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaFormatExceptionBadTokenConstruction()
        {
            Formula f = new Formula("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestIsValidFormatException()
        {
            Formula f = new Formula("1+X5", s => s, s => false);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNonLanguageToken()
        {
            Formula f = new Formula("1+$");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestAfterOperator()
        {
            Formula f = new Formula("(1+3)4");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestUnEvenParanth()
        {
            Formula f = new Formula("((1+4)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestRightPGreaterThanLeftP()
        {
            Formula f = new Formula("(1+4-(9+2)))");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNormalizeMakesBadtoken()
        {
            Formula f = new Formula("X1", s => "$", x => x.StartsWith("_"));
        }

        // END CTOR TESTS  ///////////////////////

        [TestMethod]
        public void TestBasicEvaluate()
        {
            Formula f = new Formula("5 + 3");
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void TestFullEvaluate()
        {
            Formula f = new Formula("X5 + (_X3 *  34) / 2.0 + (4) - A1");
            Assert.AreEqual(4.0, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void BasicDivideByZero()
        {
            Formula f = new Formula("2/0");
            Assert.IsInstanceOfType(f.Evaluate(x => 0), typeof(FormulaError));
        }


        [TestMethod]
        public void BasicDivideInsidePara()
        {
            Formula f = new Formula("(3/3)");
            Assert.AreEqual(1.0, f.Evaluate(x=>0));
        }

        [TestMethod]
        public void DivideByZeroInsidePara()
        {
            Formula f = new Formula("(7+6 - (4 / X5) +(2/0))");
            Assert.IsInstanceOfType(f.Evaluate(x => 0), typeof(FormulaError));
        }


        [TestMethod]
        public void BasicDivideByZeroVar()
        {
            Formula f = new Formula("2/X6");
            Assert.IsInstanceOfType(f.Evaluate(x => 0), typeof(FormulaError));
        }
    }
}
