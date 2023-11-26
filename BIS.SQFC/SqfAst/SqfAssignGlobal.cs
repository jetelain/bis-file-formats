using System.Collections.Generic;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfAssignGlobal : SqfStatement
    {
        public SqfAssignGlobal(SqfLocation location, string name, SqfExpression value)
        {
            Location = location;
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public SqfExpression Value { get; }

        public override SqfLocation Location { get; }

        public override string ToString()
        {
            return $"{Name} = {Value};";
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions)
        {
            context.RegisterCommand(Name);
            Value.Compile(context, instructions);
            instructions.Add(new SqfcInstructionGeneric(Location.Compile(context), InstructionType.AssignTo, Name));
        }
    }
}
