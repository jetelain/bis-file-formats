namespace BIS.SQFC
{
    internal enum SqfcFileBlockType : byte
    {
        Constant,
        ConstantCompressed,
        LocationInfo,
        Code,
        CodeDebug,
        CommandNameDirectory 
    }
}
