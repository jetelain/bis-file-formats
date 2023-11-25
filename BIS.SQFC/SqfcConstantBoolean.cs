using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcConstantBoolean : SqfcConstant
    {
        public SqfcConstantBoolean(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public override ConstantType ConstantType => ConstantType.Boolean;

        internal override void WriteDataTo(BinaryWriterEx writer, SqfcFile context)
        {
            writer.Write((bool)Value);
        }

        public override string ToString()
        {
            return Value ? "true" : "false";
        }
    }
}