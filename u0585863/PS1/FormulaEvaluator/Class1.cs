using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    public enum OperatorPrecedence
    {
        LeftP = 0,
        Plus = 1,
        Minus = 1,
        Multiply = 2,
        Divide = 2,
        RightP = 3

    }
}
