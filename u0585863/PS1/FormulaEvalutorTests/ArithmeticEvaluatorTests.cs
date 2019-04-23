using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaEvalutorTests
{   /// <summary>
    /// 
    /// </summary>
    class ArithmeticEvaluatorTests
    {
        private static readonly Dictionary<string, int> LookupCells = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            InitializeVariableDictionary(LookupCells);

            //Should work collection
            Dictionary<int, int> shouldWorkTests = new Dictionary<int, int>
            {
                {3, FormulaEvaluator.Evaluator.Evaluate("1+2", SimpleLookup)},
                {-1, FormulaEvaluator.Evaluator.Evaluate("1-2", SimpleLookup)},
                {2, FormulaEvaluator.Evaluator.Evaluate("4/2", SimpleLookup)},
                {8, FormulaEvaluator.Evaluator.Evaluate("4*2", SimpleLookup) },
                {10, FormulaEvaluator.Evaluator.Evaluate("a4+3", SimpleLookup) },
                {4, FormulaEvaluator.Evaluator.Evaluate("(1+3)", SimpleLookup) },
                {5, FormulaEvaluator.Evaluator.Evaluate("2+3*3/3", SimpleLookup)},
                {9, FormulaEvaluator.Evaluator.Evaluate("6+A4-4", SimpleLookup)},
                {0, FormulaEvaluator.Evaluator.Evaluate("1+A4-4*2", SimpleLookup)},
                {30, FormulaEvaluator.Evaluator.Evaluate("111+A4-44*2", SimpleLookup)},
                {37, FormulaEvaluator.Evaluator.Evaluate("111+(A4-44)*2", SimpleLookup)},
                {-37, FormulaEvaluator.Evaluator.Evaluate("111+A4-44*(2+1)-23", SimpleLookup)},
                {16, FormulaEvaluator.Evaluator.Evaluate("(16)", SimpleLookup)},
                {88, FormulaEvaluator.Evaluator.Evaluate("((16)+(6*12))", SimpleLookup)},
                {-36, FormulaEvaluator.Evaluator.Evaluate("111\t+A4-44*(2+1)-22", SimpleLookup)},
                {17, FormulaEvaluator.Evaluator.Evaluate("(17 \t )", SimpleLookup)},
            };

            RunAllValidTests(shouldWorkTests);


            //Should throw collection
            //    Dictionary<int, int> shouldThrowTests = new Dictionary<int, int>
            //    {
            //        {1, FormulaEvaluator.Evaluator.Evaluate("1+2 / 0", SimpleLookup)},
            //        {2, FormulaEvaluator.Evaluator.Evaluate("1--2", SimpleLookup)},
            //        {3, FormulaEvaluator.Evaluator.Evaluate("4 2", SimpleLookup)},
            //        {4, FormulaEvaluator.Evaluator.Evaluate("4444444444*2", SimpleLookup) },
            //        {5, FormulaEvaluator.Evaluator.Evaluate("A5+3", SimpleLookup) },
            //        {6, FormulaEvaluator.Evaluator.Evaluate(")(1+3)", SimpleLookup) },
            //        {7, FormulaEvaluator.Evaluator.Evaluate("(((2+3*3/3", SimpleLookup)},
            //        {8, FormulaEvaluator.Evaluator.Evaluate("6+A4-4", SimpleLookup)},
            //        {9, FormulaEvaluator.Evaluator.Evaluate("1+A4-4*2", SimpleLookup)},
            //        {10, FormulaEvaluator.Evaluator.Evaluate("$111+$4-44*2", SimpleLookup)},
            //    };

            //RunAllInvalidTests(shouldThrowTests);

        }

        /// <summary>
        /// This will fill a dictionary with the Variables A-Z with there associated ascii value appended to the letter
        /// EG A65 B66.  This is meant to be used in conjunction with a lookup delegate for testing
        /// </summary>
        /// <param name="variableKvPs"></param>
        private static void InitializeVariableDictionary(Dictionary<string, int> variableKvPs)
        {
            for (int i = 'A'; i <= 'Z'; i++)
            {
                KeyValuePair<string, int> kvp = new KeyValuePair<string, int>(((char)i).ToString() + i, i);
                Console.WriteLine(kvp);
                variableKvPs.Add(kvp.Key, kvp.Value);
            }
        }
        /// <summary>
        /// This will run through every entry in a dictionary and display messages depending on if the entry method
        /// throws or doesn't throw.  Currently doesn't work because entire program just stops on any throw so every
        /// test that needs to be run has to be the only one in the dictionary
        /// </summary>
        /// <param name="shouldThrowTests"></param>
        private static void RunAllInvalidTests(Dictionary<int, int> shouldThrowTests)
        {
            int count = 1;
            try
            {
                foreach (KeyValuePair<int, int> kvp in shouldThrowTests)
                {
                    Console.WriteLine($"{kvp.Key} Didn't throw");
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Key {count} threw as expected");
            }
        }
        /// <summary>
        /// Method will dispay in console how many methods in a dictionary return true based where the evaluated expression
        /// matches the key value, Displays the total number of tests run and total tests passed
        /// </summary>
        /// <param name="shouldWorkTests"></param>
        private static void RunAllValidTests(Dictionary<int, int> shouldWorkTests)
        {
            Console.WriteLine("SHOULD PASS TESTS\n\n");
            int count = 0;
            int passed = 0;
            foreach (KeyValuePair<int, int> kvp in shouldWorkTests)
            {
                Console.WriteLine($"Expected:{kvp.Key,5} ?= {kvp.Value,-5} : {kvp.Key == kvp.Value,-5}");
                count++;
                if (kvp.Key == kvp.Value) passed++;
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write($"\nTests Run: {count,-10}");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Tests Passed: {passed,10}");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// If the passed string is not "a4" or "A4" throw an argument exception
        /// </summary>
        /// <param name="s">VariableToLookup</param>
        /// <returns></returns>
        public static int SimpleLookup(string s)
        {
            string upperVariation = s.ToUpper();
            if (upperVariation == "A4")
            {
                return 7;
            }
            else throw new ArgumentException("Not a Mapped Variable Value");
        }
        /// <summary>
        /// This is what I want lookup to do in the future but it doesn't match the delegate signature 
        /// and I don't know how else to get it a dictionary without making a static dictionary
        /// </summary>
        /// <param name="s"></param>
        /// <param name="allValidVariables"></param>
        /// <returns></returns>
        public static int SimpleLookup(string s, Dictionary<string, int> allValidVariables)
        {
            string upperVariation = s.ToUpper();
            if (allValidVariables.ContainsKey(upperVariation))
            {
                return allValidVariables[upperVariation];
            }
            else throw new ArgumentException("Not a Mapped Variable Value");
        }
    }
}