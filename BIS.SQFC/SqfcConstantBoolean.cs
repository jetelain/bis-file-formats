using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal sealed class SqfcConstantBoolean : SqfcConstant
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

        internal override SqfExpression ToExpression(SqfcFile context)
        {
            return new SqfBoolean(Value);
        }

        public override bool Equals(SqfcConstant other)
        {
            return other is SqfcConstantBoolean boolean && boolean.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}