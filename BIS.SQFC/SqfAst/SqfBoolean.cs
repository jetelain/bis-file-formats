using System.Collections.Generic;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfBoolean : SqfExpression
    {
        public SqfBoolean(bool value)
        { 
            Value = value;
        }

        public override bool IsConstant => true;

        public bool Value { get; }

        public override SqfLocation Location => SqfLocation.None;

        public override int Precedence => 11;

        public override SqfValueType ResultType => SqfValueType.Boolean;

        public override string ToString()
        {
            return Value ? "true" : "false";
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions, SqfArraySafety mutationSafety = SqfArraySafety.MightBeMutated)
        {
            instructions.Add(new SqfcInstructionPushStatement(context.MakeConstantBoolean(Value)));
        }

        internal override SqfcConstant CreateConstant(SqfcFile context)
        {
            return new SqfcConstantBoolean(Value);
        }
    }
}
