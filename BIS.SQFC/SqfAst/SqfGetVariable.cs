using System.Collections.Generic;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfGetVariable : SqfExpression
    {
        public SqfGetVariable(SqfLocation location, string name)
        {
            Location = location;
            Name = name;
        }

        public string Name { get; }

        public override bool IsConstant => false;

        public override SqfLocation Location { get; }

        public override int Precedence => 11;

        public override SqfValueType ResultType => SqfValueType.Unknown;

        public override string ToString()
        {
            return Name;
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions, SqfArraySafety mutationSafety = SqfArraySafety.MightBeMutated)
        {
            context.RegisterCommand(Name);
            instructions.Add(new SqfcInstructionGeneric(Location.Compile(context), InstructionType.GetVariable, Name));
        }
    }
}
