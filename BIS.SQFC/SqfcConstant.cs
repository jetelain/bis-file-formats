using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal abstract class SqfcConstant : IEquatable<SqfcConstant>
    {
        public abstract ConstantType ConstantType { get; }

        internal static SqfcConstant Read(BinaryReaderEx reader, SqfcFile context)
        {
            var type = (ConstantType)reader.ReadByte();
            switch (type)
            {
                case ConstantType.Boolean:
                    return new SqfcConstantBoolean(reader.ReadBoolean());

                case ConstantType.Scalar:
                    return new SqfcConstantScalar(reader.ReadSingle());

                case ConstantType.String:
                    return new SqfcConstantString(reader.ReadSqfcString());

                case ConstantType.Array:
                    return new SqfcConstantArray(ReadRange(reader, context, reader.ReadInt32()).ToList());

                case ConstantType.Code:
                    var contentString = reader.ReadUInt64();
                    return new SqfcConstantCode(contentString, reader.ReadArrayBase(r => SqfcInstruction.Read(r, context), reader.ReadInt32())); 

                case ConstantType.NularCommand:
                    return new SqfcConstantNularCommand(reader.ReadSqfcString());

            }
            throw new IOException();
        }

        internal static IEnumerable<SqfcConstant> ReadRange(BinaryReaderEx reader, SqfcFile context, int size)
        {
            return reader.ReadRange(r => Read(r, context), size);
        }

        internal void WriteTo(BinaryWriterEx writer, SqfcFile context)
        {
            writer.Write((byte)ConstantType);
            WriteDataTo(writer, context);
        }

        internal abstract void WriteDataTo(BinaryWriterEx writer, SqfcFile context);

        internal virtual string ToString(SqfcFile context)
        {
            return ToString();
        }

        internal abstract SqfExpression ToExpression(SqfcFile context);

        public abstract bool Equals(SqfcConstant other);

        public override bool Equals(object obj)
        {
            return Equals(obj as SqfcConstant);
        }

        public override abstract int GetHashCode();

    }
}