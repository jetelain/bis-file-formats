using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal sealed class SqfcConstantCode : SqfcConstant
    {
        internal const ulong NoSourceCode = ulong.MaxValue;

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
                //if (instruction.Location != SqfcLocation.None)
                //{
                //    sb.Append("  // ");
                //    sb.AppendLine(instruction.Location.ToString(context));
                //}
                sb.Append("  ");
                sb.AppendLine(instruction.ToString(context));
            }
            sb.Append("}");
            return sb.ToString();
        }

        internal override SqfExpression ToExpression(SqfcFile context)
        {
            return new SqfCodeBlock(Decompile(context));
        }

        private List<SqfStatement> Decompile(SqfcFile context)
        {
            var result = new List<SqfStatement>();
            var stack = new Stack<SqfExpression>();
            foreach (var instruction in Instructions)
            {
                instruction.Execute(result, stack, context);
            }
            if (stack.Count > 0)
            {
                foreach (var item in stack)
                {
                    result.Add(new SqfResult(item));
                }
            }

            return result;
        }

        internal SqfCodeBlock ToRootExpression(SqfcFile context)
        {
            return new SqfCodeBlock(Decompile(context), (context.Constants[(int)ContentString] as SqfcConstantString)?.Value ?? string.Empty);
        }

        public override bool Equals(SqfcConstant other)
        {
            if ( other is SqfcConstantCode code)
            {
                return other == this || (Instructions.Count == code.Instructions.Count && Instructions.SequenceEqual(code.Instructions));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Instructions.Count;
        }
    }
}