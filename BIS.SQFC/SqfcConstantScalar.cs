using System.Globalization;
using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcConstantScalar : SqfcConstant
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
    }
}