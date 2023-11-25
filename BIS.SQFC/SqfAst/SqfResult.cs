using System.Collections.Generic;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfResult : SqfStatement
    {
        public SqfResult(SqfExpression expression)
        {
            Expression = expression;
        }

        public SqfExpression Expression { get; }

        public override SqfLocation Location => Expression.Location;

        public override string ToString()
        {
            return Expression.ToString();
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions)
        {
            instructions.Add(new SqfcInstructionEndStatement());
            Expression.Compile(context, instructions);
        }
    }
}
