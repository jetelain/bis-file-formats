using System;
using System.Collections.Generic;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal sealed class SqfcInstructionGeneric : SqfcInstruction
    {
        public SqfcInstructionGeneric(SqfcLocation location, InstructionType type, string value)
        {
            Location = location;
            InstructionType = type;
            Value = value;
        }

        public override InstructionType InstructionType { get;}

        public string Value { get; }

        public override SqfcLocation Location { get; }

        protected override void WriteData(BinaryWriterEx writer, SqfcFile context)
        {
            if (context.CommandNameDirectory.Count > 0)
            {
                var index = context.CommandNameDirectory.IndexOf(Value);
                if (index == -1)
                {
                    throw new InvalidOperationException();
                }
                writer.Write((ushort)index);
            }
            else
            {
                writer.WriteSqfcString(Value);
            }
        }

        public override string ToString()
        {
            switch(InstructionType)
            {
                case InstructionType.GetVariable:
                    return $"get {Value};";

                case InstructionType.AssignTo:
                    return $"setGlobal {Value};";

                case InstructionType.AssignToLocal:
                    return $"setPrivate {Value};";

                case InstructionType.CallUnary:
                    return $"unary {Value};";

                case InstructionType.CallBinary:
                    return $"binary {Value};";

                case InstructionType.CallNular:
                    return $"nular {Value};";
            }
            return $"??? {Value};";
        }

        internal override void Execute(List<SqfStatement> result, Stack<SqfExpression> stack, SqfcFile context)
        {
            switch (InstructionType)
            {
                case InstructionType.GetVariable:
                    stack.Push(new SqfGetVariable(Location.ToSqf(context), Value));
                    break;

                case InstructionType.AssignTo:
                    result.Add(new SqfAssignGlobal(Location.ToSqf(context), Value, stack.Pop()));
                    break;

                case InstructionType.AssignToLocal:
                    result.Add(new SqfAssignLocal(Location.ToSqf(context), Value, stack.Pop()));
                    break;

                case InstructionType.CallUnary:
                    stack.Push(new SqfUnary(Location.ToSqf(context), Value, stack.Pop()));
                    break;

                case InstructionType.CallBinary:
                    var right = stack.Pop();
                    var left = stack.Pop();
                    stack.Push(new SqfBinary(Location.ToSqf(context), Value, left, right));
                    break;

                case InstructionType.CallNular:
                    stack.Push(new SqfNular(Location.ToSqf(context), Value));
                    break;
            }
        }

        public override bool Equals(SqfcInstruction other)
        {
            return other is SqfcInstructionGeneric generic && generic.Value == Value && generic.InstructionType == InstructionType && generic.Location.Equals(Location);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ InstructionType.GetHashCode();
        }
    }
}