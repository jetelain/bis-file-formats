using System.Collections.Generic;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfNular : SqfExpression
    {
        public SqfNular(SqfLocation location, string name)
        {
            Location = location;
            Name = name;
        }

        public string Name { get; }

        public override bool IsConstant => SqfcConstantNularCommand.ValidValues.Contains(Name);

        public override SqfLocation Location { get; }

        public override int Precedence => 11;

        public override string ToString()
        {
            return Name;
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions)
        {
            if (IsConstant)
            {
                instructions.Add(new SqfcInstructionPushStatement(context.MakeConstantNular(Name)));
            }
            else
            {
                instructions.Add(new SqfcInstructionGeneric(Location.Compile(context), InstructionType.CallNular, Name));
            }
        }

        internal override SqfcConstant CreateConstant(SqfcFile context)
        {
            if (IsConstant)
            {
                return new SqfcConstantNularCommand(Name);
            }
            return base.CreateConstant(context);
        }
    }
}
