using System.Collections.Generic;
using System.Globalization;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfScalar : SqfExpression
    {
        public SqfScalar(float value)
        { 
            Value = value;
        }

        public override SqfLocation Location => SqfLocation.None;

        public override bool IsConstant => true;

        public float Value { get; }

        public override int Precedence => 11;

        public override SqfValueType ResultType => SqfValueType.Number;

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions, SqfArraySafety mutationSafety = SqfArraySafety.MightBeMutated)
        {
            instructions.Add(new SqfcInstructionPushStatement(context.MakeConstantScalar(Value)));
        }

        internal override SqfcConstant CreateConstant(SqfcFile context)
        {
            return new SqfcConstantScalar(Value);
        }
    }
}
