using System.Collections.Generic;
using System.Linq;
using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcConstantArray : SqfcConstant
    {
        public SqfcConstantArray(IReadOnlyList<SqfcConstant> value)
        {
            Value = value;
        }

        public override ConstantType ConstantType => ConstantType.Array;

        public IReadOnlyList<SqfcConstant> Value { get; }

        internal override void WriteDataTo(BinaryWriterEx writer, SqfcFile context)
        {
            writer.Write(Value.Count);
            foreach(var c in Value)
            {
                c.WriteTo(writer, context);
            }
        }

        public override string ToString()
        {
            return ToString(null);
        }

        internal override string ToString(SqfcFile context)
        {
            return $"[ {string.Join(", ", Value.Select(v => v.ToString(context)))} ]";
        }
    }
}