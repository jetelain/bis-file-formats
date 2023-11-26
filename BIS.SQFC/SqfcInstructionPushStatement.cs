using System;
using System.Collections.Generic;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal sealed class SqfcInstructionPushStatement : SqfcInstruction
    {
        public SqfcInstructionPushStatement(ushort constantIndex)
        {
            this.ConstantIndex = constantIndex;
        }

        public override InstructionType InstructionType => InstructionType.Push;

        public override SqfcLocation Location => SqfcLocation.None;

        public ushort ConstantIndex { get; }

        protected override void WriteData(BinaryWriterEx writer, SqfcFile context)
        {
            if (ConstantIndex >= context.Constants.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            writer.Write(ConstantIndex);
        }

        public override string ToString()
        {
            return $"push #{ConstantIndex};";
        }

        public override string ToString(SqfcFile context)
        {
            if (context == null)
            {
                return ToString();
            }
            return $"push {context.Constants[ConstantIndex].ToString(context).Replace(Environment.NewLine, Environment.NewLine + "  ")};";
        }

        internal override void Execute(List<SqfStatement> result, Stack<SqfExpression> stack, SqfcFile context)
        {
            stack.Push(context.Constants[ConstantIndex].ToExpression(context));
        }

        public override bool Equals(SqfcInstruction other)
        {
            return other is SqfcInstructionPushStatement push && push.ConstantIndex == ConstantIndex && push.Location.Equals(Location);
        }

        public override int GetHashCode()
        {
            return ConstantIndex.GetHashCode();
        }
    }
}