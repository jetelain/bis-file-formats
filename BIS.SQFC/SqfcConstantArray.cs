using System.Collections.Generic;
using System.Linq;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal sealed class SqfcConstantArray : SqfcConstant
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

        internal override SqfExpression ToExpression(SqfcFile context)
        {
            return new SqfMakeArray(SqfLocation.None, Value.Select(i => i.ToExpression(context)).ToList());
        }

        public override bool Equals(SqfcConstant other)
        {
            if (other is SqfcConstantArray array)
            {
                return other == this || (Value.Count == array.Value.Count && Value.SequenceEqual(array.Value));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.Count;
        }
    }
}