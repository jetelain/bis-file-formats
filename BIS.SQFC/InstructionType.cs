namespace BIS.SQFC
{
    public enum InstructionType : byte
    {
        EndStatement,
        Push,
        CallUnary,
        CallBinary,
        CallNular,
        AssignTo,
        AssignToLocal,
        GetVariable,
        MakeArray
    }
}
