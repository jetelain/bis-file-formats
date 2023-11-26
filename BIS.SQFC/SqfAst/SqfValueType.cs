using System;

namespace BIS.SQFC.SqfAst
{
    [Flags]
    public enum SqfValueType
    {
        Array   = 0x0000001,
        String  = 0x0000002,
        Number  = 0x0000004,
        Boolean = 0x0000008,
        Code    = 0x0000010,
        Unknown = 0xFFFFFFF
    }
}
