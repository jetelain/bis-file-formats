using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfCodeBlock : SqfExpression
    {
        public SqfCodeBlock(IReadOnlyCollection<SqfStatement> statements, string sourceCode = "")
        {
            Statements = statements;
            SourceCode = sourceCode;
        }

        public string SourceCode { get; }

        public override bool IsConstant => true;

        public IReadOnlyCollection<SqfStatement> Statements { get; }

        public override SqfLocation Location => Statements.Select(s => s.Location).FirstOrDefault(l => l != SqfLocation.None) ?? SqfLocation.None;

        public override int Precedence => 11;

        public override SqfValueType ResultType => SqfValueType.Code;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (var instruction in Statements)
            {
                sb.Append("  ");
                sb.AppendLine(instruction.ToString().Replace(Environment.NewLine, Environment.NewLine + "  "));
            }
            sb.Append("}");
            return sb.ToString();
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions, SqfArraySafety mutationSafety = SqfArraySafety.MightBeMutated)
        {
            instructions.Add(new SqfcInstructionPushStatement(context.MakeConstant(CreateConstant(context))));
        }

        internal override SqfcConstant CreateConstant(SqfcFile context)
        {
            return new SqfcConstantCode(SqfcConstantCode.NoSourceCode, CreateInstructions(context));
        }

        private List<SqfcInstruction> CreateInstructions(SqfcFile context)
        {
            var instructions = new List<SqfcInstruction>();
            foreach (var statement in Statements)
            {
                statement.Compile(context, instructions);
            }
            return instructions;
        }

        internal SqfcConstantCode CreateRootConstant(SqfcFile context)
        {
            return new SqfcConstantCode(context.MakeConstantString(SourceCode), CreateInstructions(context));
        }
    }
}
