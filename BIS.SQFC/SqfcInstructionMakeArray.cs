using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcInstructionMakeArray : SqfcInstruction
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
    }
}