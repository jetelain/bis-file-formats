using System.Collections.Generic;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal sealed class SqfcInstructionMakeArray : SqfcInstruction
    {
        public SqfcInstructionMakeArray(SqfcLocation location, ushort arraySize)
        {
            Location = location;
            ArraySize = arraySize;
        }

        public override SqfcLocation Location { get; }

        public ushort ArraySize { get; }

        public override InstructionType InstructionType => InstructionType.MakeArray;

        protected override void WriteData(BinaryWriterEx writer, SqfcFile context)
        {
            writer.Write(ArraySize);
        }

        public override string ToString()
        {
            return $"makeArray {ArraySize};";
        }

        internal override void Execute(List<SqfStatement> result, Stack<SqfExpression> stack, SqfcFile context)
        {
            var items = new SqfExpression[ArraySize];
            for (int i = ArraySize-1; i >= 0; i--)
            {
                items[i] = stack.Pop();
            }
            stack.Push(new SqfMakeArray(Location.ToSqf(context), items));
        }

        public override bool Equals(SqfcInstruction other)
        {
            return other is SqfcInstructionMakeArray generic && generic.ArraySize == ArraySize && generic.Location.Equals(Location);
        }

        public override int GetHashCode()
        {
            return ArraySize.GetHashCode();
        }
    }
}