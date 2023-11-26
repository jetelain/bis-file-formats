using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal sealed class SqfcConstantString : SqfcConstant
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
        internal override SqfExpression ToExpression(SqfcFile context)
        {
            return new SqfString(Value);
        }

        public override bool Equals(SqfcConstant other)
        {
            return other is SqfcConstantString str && str.Value == Value;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}