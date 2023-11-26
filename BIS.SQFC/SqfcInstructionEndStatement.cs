using System.Collections.Generic;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal sealed class SqfcInstructionEndStatement : SqfcInstruction
    {
        public override InstructionType InstructionType => InstructionType.EndStatement;

        public override SqfcLocation Location => SqfcLocation.None;

        protected override void WriteData(BinaryWriterEx writer, SqfcFile context)
        {
        }


        public override string ToString()
        {
            return "endStatement;";
        }

        internal override void Execute(List<SqfStatement> result, Stack<SqfExpression> stack, SqfcFile context)
        {
            if (stack.Count > 0 )
            {
                foreach(var item in stack)
                {
                    result.Add(new SqfEvaluateStatement(item));
                }
                stack.Clear();
            }
        }

        public override bool Equals(SqfcInstruction other)
        {
            return other is SqfcInstructionEndStatement;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}