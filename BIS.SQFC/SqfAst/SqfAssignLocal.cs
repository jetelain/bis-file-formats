using System.Collections.Generic;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfAssignLocal : SqfStatement
    {
        public SqfAssignLocal(SqfLocation location, string name, SqfExpression value)
        {
            Name = name;
            Value = value;
            Location = location;
        }

        public string Name { get; }

        public SqfExpression Value { get; }

        public override SqfLocation Location { get; }

        public override string ToString()
        {
            return $"private {Name} = {Value};";
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions)
        {
            context.RegisterCommand(Name);
            Value.Compile(context, instructions);
            instructions.Add(new SqfcInstructionGeneric(Location.Compile(context), InstructionType.AssignToLocal, Name));
        }
    }
}
