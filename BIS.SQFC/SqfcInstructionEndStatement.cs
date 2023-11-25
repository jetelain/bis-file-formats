using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcInstructionEndStatement : SqfcInstruction
    {
        public override InstructionType InstructionType => InstructionType.EndStatement;

        public override SqfcLocation Location => SqfcLocation.None;

        protected override void WriteData(BinaryWriterEx writer, SqfcFile context)
        {
        }


        public override string ToString()
        {
            return "end;";
        }
    }
}