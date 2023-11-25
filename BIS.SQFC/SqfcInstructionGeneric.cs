using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcInstructionGeneric : SqfcInstruction
    {
        public SqfcInstructionGeneric(SqfcLocation location, InstructionType type, string value)
        {
            Location = location;
            InstructionType = type;
            Value = value;
        }

        public override InstructionType InstructionType { get;}

        public string Value { get; }

        public override SqfcLocation Location { get; }

        protected override void WriteData(BinaryWriterEx writer, SqfcFile context)
        {
            if (context.CommandNameDirectory.Count > 0)
            {
                writer.Write((ushort)context.CommandNameDirectory.IndexOf(Value));
            }
            else
            {
                writer.WriteSqfcString(Value);
            }
        }

        public override string ToString()
        {
            return Value + ";";
        }
    }
}