using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcConstantNularCommand : SqfcConstant
    {
        public SqfcConstantNularCommand(string value)
        {
            Value = value;
        }

        public override ConstantType ConstantType => ConstantType.NularCommand;

        public string Value { get; }

        internal override void WriteDataTo(BinaryWriterEx writer, SqfcFile context)
        {
            writer.WriteSqfcString(Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}