using System;
using System.Collections.Generic;
using System.Text;
using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcConstantCode : SqfcConstant
    {
        public SqfcConstantCode(ulong contentString, IReadOnlyList<SqfcInstruction> sqfcInstructions)
        {
            ContentString = contentString;
            Instructions = sqfcInstructions;
        }

        public override ConstantType ConstantType => ConstantType.Code;

        public ulong ContentString { get; }

        public IReadOnlyList<SqfcInstruction> Instructions { get; }

        internal override void WriteDataTo(BinaryWriterEx writer, SqfcFile context)
        {
            writer.Write(ContentString);
            writer.Write(Instructions.Count);
            foreach(var instruction in Instructions)
            {
                instruction.WriteTo(writer, context);
            }
        }

        public override string ToString()
        {
            return ToString(null);
        }

        internal override string ToString(SqfcFile context)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (var instruction in Instructions)
            {
                if (instruction.Location != SqfcLocation.None)
                {
                    sb.Append("  // ");
                    sb.AppendLine(instruction.Location.ToString(context));
                }
                sb.Append("  ");
                sb.AppendLine(instruction.ToString(context));
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}