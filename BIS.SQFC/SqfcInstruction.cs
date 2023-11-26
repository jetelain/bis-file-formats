using System;
using System.Collections.Generic;
using System.IO;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal abstract class SqfcInstruction : IEquatable<SqfcInstruction>
    {
        public abstract InstructionType InstructionType { get; }

        public abstract SqfcLocation Location { get; }

        internal static SqfcInstruction Read(BinaryReaderEx reader, SqfcFile context)
        {
            var type = (InstructionType)reader.ReadByte();

            var location = SqfcLocation.None;
            if (type != InstructionType.EndStatement && type != InstructionType.Push)
            {
                location = SqfcLocation.Read(reader);
            }

            switch(type)
            {
                case InstructionType.EndStatement:
                    return new SqfcInstructionEndStatement();

                case InstructionType.Push:
                    return new SqfcInstructionPushStatement(reader.ReadUInt16());

                case InstructionType.CallUnary:
                case InstructionType.CallBinary:
                case InstructionType.CallNular:
                case InstructionType.AssignTo:
                case InstructionType.AssignToLocal:
                case InstructionType.GetVariable:
                    if (context.CommandNameDirectory.Count > 0)
                    {
                        return new SqfcInstructionGeneric(location, type, context.CommandNameDirectory[reader.ReadUInt16()]);
                    }
                    return new SqfcInstructionGeneric(location, type, reader.ReadSqfcString());

                case InstructionType.MakeArray:
                    return new SqfcInstructionMakeArray(location, reader.ReadUInt16());
            }

            throw new IOException();
        }

        internal void WriteTo(BinaryWriterEx writer, SqfcFile context)
        {
            writer.Write((byte)InstructionType);
            if (InstructionType != InstructionType.EndStatement && InstructionType != InstructionType.Push)
            {
                Location.WriteTo(writer, context);
            }
            WriteData(writer, context);
        }

        protected abstract void WriteData(BinaryWriterEx writer, SqfcFile context);

        public virtual string ToString(SqfcFile context)
        {
            return ToString();
        }

        internal abstract void Execute(List<SqfStatement> result, Stack<SqfExpression> stack, SqfcFile context);

        public abstract bool Equals(SqfcInstruction other);

        public override bool Equals(object obj)
        {
            return Equals(obj as SqfcInstruction);
        }

        public override abstract int GetHashCode();
    }
}
