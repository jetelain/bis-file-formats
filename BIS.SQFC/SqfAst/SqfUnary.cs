using System.Collections.Generic;
using System.Text;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfUnary : SqfExpression
    {
        public SqfUnary(SqfLocation location, string name, SqfExpression argument)
        {
            Location = location;
            Name = name;
            Argument = argument;
        }

        public string Name { get; }

        public SqfExpression Argument { get; }

        public override bool IsConstant => false;

        public override SqfLocation Location { get; }

        public override int Precedence => 10;

        public override SqfValueType ResultType => SqfValueType.Unknown;

        public override string ToString()
        {
            if (Name == "if")
            {
                return $"if ({Argument})";
            }
            var sb = new StringBuilder();
            sb.Append(Name);
            sb.Append(' ');
            Append(sb, Precedence, Argument);
            return sb.ToString();
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions, SqfArraySafety mutationSafety = SqfArraySafety.MightBeMutated)
        {
            context.RegisterCommand(Name);

            var safety = SqfArraySafety.ConstSafeNotNested;
            if (Name == "+")
            {
                safety = SqfArraySafety.ConstSafe;
            }
            Argument.Compile(context, instructions, safety);
            instructions.Add(new SqfcInstructionGeneric(Location.Compile(context), InstructionType.CallUnary, Name));
        }
    }
}
