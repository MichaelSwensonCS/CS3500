// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

//Assignment completed by Michael Swenson, U0585863


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        //PS3 List #1 of 8 Parsing (Combined with entire for loop  below)
        private string[] tokenArray;
        private HashSet<string> normalizedTokenArray;
        private int tokenizedArrayLength;


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message. 
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>

        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            if(this is null) throw new FormulaFormatException("Passed Formula is null");

            Double parsedNumberToken = 0;
            int CountLeftP = 0;
            int CountRightP = 0;

            //Looking for invalid stuff
            //PS3 List #1 of 8 Parsing tokens
            formula = formula.Trim();
            tokenArray = (GetTokens(formula)).ToArray();
            tokenizedArrayLength = tokenArray.Length;

            normalizedTokenArray = new HashSet<string>();

            //PS3 List #2 of 8 At Least One Token
            if (tokenizedArrayLength < 1) throw new FormulaFormatException("A token must be Passed in, this appears to be blank");


            //PS3 List #5 of 8 Starting Token == Number,Variable,"(" 
            if (IfItIsNotANumVarOrLeftP(parsedNumberToken, tokenArray[0])) throw new FormulaFormatException("The First Character or Token is invalid");

            for (int i = 0; i < tokenizedArrayLength; i++)
            {
                //PS3 List #7 of 8 Token Following operator || "(" == Number, Variable, "("
                //Parantheses Handling                                   //////////////
                if (tokenArray[i].Equals("(") || tokenArray[i].IsAOperator())
                {
                    if (tokenArray[i].Equals("(")) CountLeftP++;
                    if ((i + 1) < (tokenizedArrayLength))
                    {
                        if (IfItIsNotANumVarOrLeftP(parsedNumberToken, tokenArray[i + 1])) throw new FormulaFormatException("There appears to be an invalid token following a left parantheses or an operator(*/+-)");
                    }
                }
                else if (tokenArray[i].Equals(")"))
                {
                    CountRightP++;
                    //PS3 List #3 of 8 Right Paranetheses "(" >= ")"
                    if (CountRightP > CountLeftP) throw new FormulaFormatException("There is a mismatched Number of Parentheses");
                }
                ///////  End Parantheses                                /////////////


                //PS3 List #8 of 8 Extra Following Token after Number||Variable||")" == Operator || ")"

                //If It Is A Number Or Variable Or Right Parantheses
                if (!IfItIsNotANumVarOrRightP(parsedNumberToken, tokenArray[i]))
                {
                    if ((i + 1) < tokenizedArrayLength)
                    {
                        if (!(tokenArray[i + 1].Equals(")") || (tokenArray[i + 1].IsAOperator())))
                            throw new FormulaFormatException("There is an invalid token after an operator or and Right parantheses");
                    }
                }
            }

            //PS3 List #6 of 8 End Token == Number || Variable || ")"
            if (IfItIsNotANumVarOrRightP(parsedNumberToken, tokenArray[tokenizedArrayLength - 1])) throw new FormulaFormatException("Last Token is an invalid Token");

            //PS3 List #4 of 8 Balanced Parentheses "(" == ")" at the end
            if (!CountLeftP.Equals(CountRightP)) throw new FormulaFormatException("There are an uneven number of parenthesis.");

            //8 of 8 rules enforced for verifying valid formula

            int index = 0;
            //Try Normalize and isValid and if that works replace the old value with the normalized value
            foreach (string s in tokenArray)
            {
                if (s.IsAVariable())
                {
                    //If Normalize creates an invalid Variable
                    if (!normalize(s).IsAVariable()) throw new FormulaFormatException("Normalized Variable is not valid");
                    tokenArray[index] = normalize(s);

                    //If it isn't valid based on user input 
                    if (!isValid(tokenArray[index])) throw new FormulaFormatException("Normalized variable does not match passed IsValid test");
                    else
                    {
                        normalizedTokenArray.Add(normalize(s));
                    }
                }
                index++;
            }
        }



        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            bool isFirstToken = true;
            Stack<Double> values = new Stack<Double>();
            Stack<string> operators = new Stack<string>();

            Double currentToken = 0;
            for (int i = 0; i < tokenArray.Count(); i++)
            {
                Double.TryParse(tokenArray[i].Trim(), out currentToken);

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
                        //Every method that might return a Divide by 0 exception must be caught
                        //Note: Double / 0 = Infinity which is typeOf(Double) so it must be manually caught
                        try
                        {
                            IsStackEmptyPeekAndEvaluate(values, operators, currentToken);
                        }
                        catch (DivideByZeroException)
                        {
                            return new FormulaError("Divide by Zero");
                        }
                    }
                }
                //It is a variable so pass it to a Lookup and get an double and treat it as a number
                else if (StringExtensions.IsAVariable(tokenArray[i]))
                {
                    string currentTokenIsAVariable = tokenArray[i];
                    isFirstToken = false;
                    try
                    {
                        //Lookup is the passed method
                        currentToken = lookup(currentTokenIsAVariable);
                    }
                    catch (Exception)
                    {
                        return new FormulaError("Variable value is undefined");
                    }

                    try
                    {
                        IsStackEmptyPeekAndEvaluate(values, operators, currentToken);
                    }
                    catch (DivideByZeroException)
                    {
                        return new FormulaError("Divide by Zero");
                    }
                }

                // * / Handlers
                else if (tokenArray[i] == "/" || tokenArray[i] == "*")
                {
                    operators.Push(tokenArray[i]);
                }

                //+ - Operators handlers
                else if (tokenArray[i] == "+" || tokenArray[i] == "-")
                {
                    if (values.Count() >= 2 && operators.Count() >= 1 && (operators.Peek() == "+" || operators.Peek() == "-"))
                    {
                        try
                        {
                            DoTheMathAndPush(values, operators);
                        }
                        catch (DivideByZeroException)
                        {
                            return new FormulaError("Divide by zero error");
                        }
                    }
                    operators.Push(tokenArray[i]);
                }

                //Paranthesis Handlers
                else if (tokenArray[i] == "(")
                {
                    operators.Push(tokenArray[i]);
                }
                else if (tokenArray[i] == ")")
                {

                    InsideParenthesis(values, operators, tokenArray, i);

                }
            }

            //No More Parsing after here, Just final situations to check
            if (operators.Count == 0 && values.Count == 1)
            {
                return values.Pop();

            }
            //This should only be a + or -, but DoTheMathAndPush doesn't care
            else // (values.Count == 2 && operators.Count == 1)
            {
                DoTheMathAndPush(values, operators);
                return values.Pop();
            }
        }


        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            //Changed to Hashset because I thought maybe they only wanted Unique variables
            return new HashSet<string>(normalizedTokenArray);
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in tokenArray)
            {
                //Strip spaces, probably could've used a regex
                if (s.Equals(" ")) continue;
                else sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            //Make sure you will be comparing two of the same thing
            if (obj.GetType() != this.GetType()) return false;

            Formula that = (Formula)obj;
            for (int i = 0; i < this.tokenizedArrayLength; i++)
            {
                //If both can be parsed as numbers, then check numbers
                if (Double.TryParse(this.tokenArray[i], out Double thisResult) && Double.TryParse(that.tokenArray[i], out Double thatResult))
                {
                    if (thisResult != thatResult) return false;
                }
                //Check anything else with should just be a string
                else if (!this.tokenArray[i].Equals(that.tokenArray[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            //Tried using ReferenceEquals like in class but VS said to do this
            if (f1 is null && f2 is null) return true;
            else if (!(f1 is null) && f2 is null) return false;
            else if (!(f2 is null) && f1 is null) return false;

            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }




        /**************************          HELPER METHODS               *******************************/
        /// <summary>
        /// Used in construction of a formula object to check if a token is a variable,number or ")"
        /// </summary>
        /// <param name="parsedNumberToken"></param>
        /// <param name="currentToken"></param>
        /// <returns></returns>
        private static bool IfItIsNotANumVarOrRightP(double parsedNumberToken, string currentToken)
        {
            return !(currentToken.IsAVariable() || StringExtensions.IsANumber(currentToken, parsedNumberToken) || currentToken.Equals(")"));
        }
        /// <summary>
        /// Used in construction of a formula object to check if a token is a variable,number or "("
        /// </summary>
        /// <param name="parsedNumberToken"></param>
        /// <param name="currentToken"></param>
        /// <returns></returns>
        private static bool IfItIsNotANumVarOrLeftP(double parsedNumberToken, string currentToken)
        {
            return !(currentToken.IsAVariable() || StringExtensions.IsANumber(currentToken, parsedNumberToken) || currentToken.Equals("("));
        }

        /************************        HELPER METHODS      *************************************************************/


        /// <summary>
        /// This is a helper method that does checking of stack values to verify which operations
        /// should be done and will push the appropriate value on the Operand stack
        /// </summary>
        /// <param name="values">Operand Stack</param>
        /// <param name="operators">Operator Stack</param>
        /// <param name="currentToken">The Number we are currently Looking at</param>
        private static void IsStackEmptyPeekAndEvaluate(Stack<Double> values, Stack<string> operators, Double currentToken)
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
        private static void InsideParenthesis(Stack<Double> values, Stack<string> operators, string[] tokenArray, int i)
        {
            if (values.Count > 1 && operators.Count > 0 && (operators.Peek() != "("))
            {
                DoTheMathAndPush(values, operators);
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
        private static void DoTheMathAndPush(Stack<Double> values, Stack<string> operators)
        {
            Double value1 = values.Pop();
            Double value2 = values.Pop();
            string switchValue = operators.Pop();
            switch (switchValue)
            {
                case "*":
                    values.Push(value2 * value1);
                    break;
                case "/":
                    if (value1 == 0) throw new DivideByZeroException("Math Helper Can't divide by 0");
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
        private static void DivideAndMultiply(Stack<Double> values, Stack<string> operators, Double currentToken)
        {
            Double result = 0;
            if (operators.Peek() == "/" && currentToken == 0) throw new DivideByZeroException("Tried to divide by zero check the expression or assignment of currentToken");
            result = operators.Pop() == "*" ? values.Pop() * currentToken : values.Pop() / currentToken;
            values.Push(result);
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }

    }

    /// <summary>
    /// Provides quick,short, methods for determining if a string is a Number,Variable, or Operator
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsANumber(this string token, double assignment) => double.TryParse(token, out assignment);
        public static bool IsAVariable(this string token) => Regex.IsMatch(token, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*");
        public static bool IsAOperator(this string token) => (token.Equals("*") || token.Equals("/") || token.Equals("+") || token.Equals("-"));
    }
}

