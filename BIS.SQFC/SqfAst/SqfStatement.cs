using System.Collections.Generic;

namespace BIS.SQFC.SqfAst
{
    public abstract class SqfStatement
    {
        public abstract SqfLocation Location { get; }

        internal abstract void Compile(SqfcFile context, List<SqfcInstruction> instructions);
    }
}
