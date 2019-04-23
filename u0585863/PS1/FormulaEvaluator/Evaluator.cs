/*
 * 
 * Author: Michael Swenson
 * Date: 9/7/2018
 * Class: CS3500
 * Assignment: PS1
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    /// <summary>
    /// This class will evaluate a string arithmetic expression using integer math and return a value.
    /// EG "2 * 3 + (5+1)" will return 11. The other function is that you can define a lookup delegate with
    /// the signature int Foo(string) so that your evaluated string can recognize variable names
    /// EG "A4 + 8" where A4 is a value defined else where.
    /// 
    /// </summary>
    public static class Evaluator
    {
        public delegate int Lookup(String v); 
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            int leftParanthesis = 0;
            int rightParanthesis = 0;
            bool isFirstToken = true;
            Stack<int> values = new Stack<int>();
            Stack<string> operators = new Stack<string>();
            exp = exp.Trim();

            string[] tokenArray = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            int currentToken = 0;
            for (int i = 0; i < tokenArray.Count(); i++)
            {
                //This honestly isn't exact but I couldn't think of a way besides checked to see if it was an overflow before parsing it.
                if (tokenArray[i].Length > 8) throw new ArgumentException("Close to overflow these are integers, the Max is 2.1 billion");
                int.TryParse(tokenArray[i].Trim(), out currentToken);

                //Ignore spaces
                if(tokenArray[i] == "")
                {
                    continue;
                }

                //Check if the first Token is a Valid Token
                if (!StringExtensions.IsANumber(tokenArray[0], currentToken)
                    && !StringExtensions.IsAVariable(tokenArray[0]) 
                    && tokenArray[0] != "(" && tokenArray[0] != "")
                    throw new ArgumentException();

                //If it is Valid, Check if it is a Number, if it is the first Token we will always push
                if (StringExtensions.IsANumber(tokenArray[i], currentToken))
                {
                    if (isFirstToken)
                    {
                        values.Push(currentToken);
                        isFirstToken = false;
                    }
                    else if (!isFirstToken)
                    {
                        IsStackEmptyPeekAndEvaluate(values, operators, currentToken);
                    }
                }
                //It is a variable so pass it to a Lookup and get an int and treat it as a number
                else if (StringExtensions.IsAVariable(tokenArray[i]))
                {
                    string currentTokenIsAVariable = tokenArray[i];
                    isFirstToken = false;
                    currentToken = variableEvaluator(currentTokenIsAVariable);
                    IsStackEmptyPeekAndEvaluate(values, operators, currentToken);
                }

                // * / Handlers
                else if (tokenArray[i] == "/" || tokenArray[i] == "*")
                {
                    operators.Push(tokenArray[i]);
                }
                
                //+ - Operators handlers
                else if (tokenArray[i] == "+" || tokenArray[i] == "-")
                {
                    if (values.Count() >= 2 && operators.Count() >= 1 && (operators.Peek() == "+"|| operators.Peek() == "-"))
                    {
                        DoTheMathAndPush(values, operators);
                    }
                    operators.Push(tokenArray[i]);
                }
                
                //Paranthesis Handlers
                else if (tokenArray[i] == "(")
                {
                    //This might be redundant checking but Left ( should never be greater than right, especially because we never push right )
                    leftParanthesis++;
                    if (leftParanthesis < rightParanthesis) throw new ArgumentException("More Right Paranthesis than left");
                    operators.Push(tokenArray[i]);
                }
                else if (tokenArray[i] == ")")
                {
                    rightParanthesis++;
                    InsideParenthesis(values, operators, tokenArray, i);
                }
            }
             
            //No More Parsing after here, Just final situations to check, and if a bad token messses up the expression many times it will get caught below
            if (operators.Count == 0 && values.Count == 1)
            {
                return values.Pop();

            }
            //This should only be a + or -, but DoTheMathAndPush doesn't care
            else if (values.Count == 2 && operators.Count == 1)
            {
                DoTheMathAndPush(values, operators);
                return values.Pop();
            }
            else
            {
                throw new ArgumentException("End of parser, a bad token");
            }
        }


        /************************        HELPER METHODS      *************************************************************/


        /// <summary>
        /// This is a helper method that does checking of stack values to verify which operations
        /// should be done and will push the appropriate value on the Operand stack
        /// </summary>
        /// <param name="values">Operand Stack</param>
        /// <param name="operators">Operator Stack</param>
        /// <param name="currentToken">The Number we are currently Looking at</param>
        private static void IsStackEmptyPeekAndEvaluate(Stack<int> values, Stack<string> operators, int currentToken)
        {
            if (operators.Count != 0 && values.Count != 0)
            {
                if (operators.Peek() == "*" || operators.Peek() == "/")
                    DivideAndMultiply(values, operators, currentToken);
                else values.Push(currentToken);
            }
            else
            {
                values.Push(currentToken);
            }
        }
        /// <summary>
        /// Helper Method that does a lot of stack size and peek() checking to verify which operation needs to
        /// occur. It utitilizes <code>DoTheMathAndPush</code>  Method to do small arithmetic evalutations and push the 
        /// result onto the stack
        /// </summary>
        /// <param name="values">Operand Stack</param>
        /// <param name="operators">Operator Stack "*/+-("</param>
        /// <param name="tokenArray"></param>
        /// <param name="i"></param>
        private static void InsideParenthesis(Stack<int> values, Stack<string> operators, string[] tokenArray, int i)
        {
            if (values.Count > 1 && operators.Count > 0 && (operators.Peek() != "("))
            {
                DoTheMathAndPush(values, operators);
            }

            if (operators.Count == 0 || operators.Peek() != "(")
            {
                throw new ArgumentException("Parenthesis pop off left bracket is broken");
            }
            operators.Pop();
            if (values.Count > 1 && operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
            {
                DoTheMathAndPush(values, operators);
            }
        }
        /// <summary>
        /// Switch statement for taking any two values from the Operand stack and one operator from the Operator Stack
        /// and computing the appropriate result and pushing that result back onto the Operand Stack will also catch
        /// divide by zero errors, throw as ArgumentExceptions
        /// </summary>
        /// <param name="values">Operand Stack</param>
        /// <param name="operators">Operator Stack</param>
        private static void DoTheMathAndPush(Stack<int> values, Stack<string> operators)
        {
            int value1 = values.Pop();
            int value2 = values.Pop();
            string switchValue = operators.Pop();
            switch (switchValue)
            {
                case "*":
                    values.Push(value2 * value1);
                    break;
                case "/":
                    if (value1 == 0) throw new ArgumentException("Math Helper Can't divide by 0");
                    values.Push(value2 / value1);
                    break;
                case "+":
                    values.Push(value2 + value1);
                    break;
                case "-":
                    values.Push(value2 - value1);
                    break;
                default:
                    Console.WriteLine("Paranthesis switch broke");
                    break;
            }
        }
        /// <summary>
        /// This is a small arithmetic helper that only does divide and multiply functions in one specific situation
        /// Will catch divide by 0 errors
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operators"></param>
        /// <param name="currentToken"></param>
        private static void DivideAndMultiply(Stack<int> values, Stack<string> operators, int currentToken)
        {
            int result = 0;
            if (operators.Peek() == "/" && currentToken == 0) throw new ArgumentException("Tried to divide by zero check the expression or assignment of currentToken");
            result = operators.Pop() == "*" ? values.Pop() * currentToken : values.Pop() / currentToken;
            values.Push(result);
        }
    }
    /// <summary>
    /// Contains two methods for checking for valid numbers or valid variables, both will return a bool
    /// and the IsANumber will also assigned the passed variable if the number is valid
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsANumber(this string token, int assignment) => int.TryParse(token, out assignment);
        public static bool IsAVariable(this string token) => Regex.IsMatch(token, @"[a-zA-Z]\d");
    }

}

//private static int CaptureEntireNumber(char[] tokenArray, ref int i, ref bool scanningNumber)
//{
//    int lastIndex = tokenArray.Length - 1;
//    StringBuilder captureMultiDigitNumber = new StringBuilder();
//    captureMultiDigitNumber.Append(tokenArray[i]);
//    if (i != lastIndex && (IsA0to9Digit(tokenArray[i]) || IsALetter(tokenArray[i])) && tokenArray[i + 1] == ' ')
//    {
//        throw new ArgumentException("Invalid token, token plus space");
//    }
//    while (i != tokenArray.Length - 1 && IsA0to9Digit(tokenArray[i + 1]))
//    {
//        captureMultiDigitNumber.Append(tokenArray[i + 1]);
//        i++;
//    }
//    scanningNumber = false;

//    int.TryParse(captureMultiDigitNumber.ToString(), out int currentToken);
//    return currentToken;
//}