using System.Collections.Generic;
using System.Text;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfBinary : SqfExpression
    {
        public SqfBinary(SqfLocation location, string name, SqfExpression left, SqfExpression right)
        {
            Location = location;
            Name = name;
            Left = left;
            Right = right;
        }

        public string Name { get; }

        public SqfExpression Left { get; }

        public override bool IsConstant => false;

        public SqfExpression Right { get; }

        public override SqfLocation Location { get; }

        public override int Precedence => GetPrecedence(Name);

        public override SqfValueType ResultType => SqfValueType.Unknown;

        public override string ToString()
        {
            if (Name == "then")
            {
                if (Right is SqfMakeArray array && array.Items.Count == 2)
                {
                    return $"{Left} then {array.Items[0]} else {array.Items[1]}";
                }
            }
            var thisPrecedence = Precedence;
            var sb = new StringBuilder();
            Append(sb, thisPrecedence, Left);
            sb.Append(' ');
            sb.Append(Name);
            sb.Append(' ');
            Append(sb, thisPrecedence, Right);
            return sb.ToString();
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions, SqfArraySafety mutationSafety)
        {
            context.RegisterCommand(Name);
            Left.Compile(context, instructions, SqfArraySafety.ConstSafeNotNested);
            Right.Compile(context, instructions, SqfArraySafety.ConstSafeNotNested);
            instructions.Add(new SqfcInstructionGeneric(Location.Compile(context), InstructionType.CallBinary, Name));
        }

        private static int GetPrecedence(string name)
        {
            switch(name)
            {
                case "||":
                case "or":
                    return 1;

                case "&&":
                case "and":
                    return 2;

                case "==":
                case "!=":
                case ">":
                case "<":
                case ">=":
                case "<=":
                case ">>":
                    return 3;

                case "then":
                    return 5;

                case "+":
                case "-":
                case "min":
                case "max":
                    return 6;

                case "*":
                case "/":
                case "%":
                case "mod":
                case "atan2":
                    return 7;

                case "^":
                    return 8;

                case "#":
                    return 9;

                default:
                    return 4;
            }
        }
    }
}
