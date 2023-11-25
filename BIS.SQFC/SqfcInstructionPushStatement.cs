using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcInstructionPushStatement : SqfcInstruction
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
            return $"push {context.Constants[ConstantIndex].ToString(context)};";
        }
    }
}