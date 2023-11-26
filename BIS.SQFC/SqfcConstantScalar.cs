using System.Globalization;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal sealed class SqfcConstantScalar : SqfcConstant
    {
        public SqfcConstantScalar(float v)
        {
            Value = v;
        }

        public float Value { get; }

        public override ConstantType ConstantType => ConstantType.Scalar;

        internal override void WriteDataTo(BinaryWriterEx writer, SqfcFile context)
        {
            writer.Write((float)Value);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        internal override SqfExpression ToExpression(SqfcFile context)
        {
            return new SqfScalar(Value);
        }

        public override bool Equals(SqfcConstant other)
        {
            return other is SqfcConstantScalar number && number.Value == Value;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}