using System.Collections.Generic;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfString : SqfExpression
    {
        public SqfString(string value)
        { 
            Value = value;
        }

        public override SqfLocation Location => SqfLocation.None;

        public override bool IsConstant => true;

        public string Value { get; }

        public override int Precedence => 11;

        public override SqfValueType ResultType => SqfValueType.String;

        public override string ToString()
        {
            return $"\"{Value.Replace("\"", "\"\"")}\"";
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions, SqfArraySafety mutationSafety = SqfArraySafety.MightBeMutated)
        {
            instructions.Add(new SqfcInstructionPushStatement(context.MakeConstantString(Value)));
        }

        internal override SqfcConstant CreateConstant(SqfcFile context)
        {
            return new SqfcConstantString(Value);
        }
    }
}
