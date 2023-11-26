using System;
using System.Collections.Generic;
using System.Text;

namespace BIS.SQFC.SqfAst
{
    public abstract class SqfExpression
    {
        public abstract SqfLocation Location { get; }

        public abstract bool IsConstant { get; }

        internal abstract void Compile(SqfcFile context, List<SqfcInstruction> instructions, SqfArraySafety mutationSafety = SqfArraySafety.MightBeMutated);

        public abstract int Precedence { get; }

        public abstract SqfValueType ResultType { get; }

        internal virtual SqfcConstant CreateConstant(SqfcFile context)
        {
            throw new NotSupportedException();
        }

        internal static void Append(StringBuilder sb, int thisPrecedence, SqfExpression other)
        {
            if (other.Precedence < thisPrecedence)
            {
                sb.Append('(');
                sb.Append(other.ToString());
                sb.Append(')');
            }
            else
            {
                sb.Append(other.ToString());
            }
        }
    }
}
