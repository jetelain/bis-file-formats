using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcConstantString : SqfcConstant
    {
        public SqfcConstantString(string v)
        {
            Value = v;
        }

        public override ConstantType ConstantType => ConstantType.String;

        public string Value { get; }

        internal override void WriteDataTo(BinaryWriterEx writer, SqfcFile context)
        {
            writer.WriteSqfcString(Value);
        }

        public override string ToString()
        {
            return $"\"{Value.Replace("\"", "\"\"")}\"";
        }
    }
}