using System.Collections.Generic;
using System.Linq;

namespace BIS.SQFC.SqfAst
{
    public sealed class SqfMakeArray : SqfExpression
    {
        public SqfMakeArray(SqfLocation location, IReadOnlyList<SqfExpression> items)
        {
            Location = location;
            Items = items;
        }

        public IReadOnlyList<SqfExpression> Items { get; }

        public override bool IsConstant => Items.All(i => i.IsConstant);

        public override SqfLocation Location { get; }

        public override int Precedence => 11;

        public override string ToString()
        {
            return $"[ {string.Join(", ", Items.Select(v => v.ToString()))} ]";
        }

        internal override void Compile(SqfcFile context, List<SqfcInstruction> instructions)
        {
            if (IsConstant)
            {
                instructions.Add(new SqfcInstructionPushStatement(context.MakeConstant(CreateConstantImpl(context))));
            }
            else
            {
                foreach(var item in Items)
                {
                    item.Compile(context, instructions);
                }
                instructions.Add(new SqfcInstructionMakeArray(Location.Compile(context), (ushort)Items.Count));
            }
        }

        private SqfcConstantArray CreateConstantImpl(SqfcFile context)
        {
            return new SqfcConstantArray(Items.Select(i => i.CreateConstant(context)).ToList());
        }

        internal override SqfcConstant CreateConstant(SqfcFile context)
        {
            if (IsConstant)
            {
                return CreateConstantImpl(context);
            }
            return base.CreateConstant(context);
        }

    }
}
