namespace BIS.SQFC.SqfAst
{
    public sealed class SqfLocation
    {
        private SqfLocation()
        {
            FileName = string.Empty;
        }

        public SqfLocation(string fileName, ushort line, uint offset)
        {
            FileName = fileName;
            Line = line;
            Offset = offset;
        }

        public static SqfLocation None { get; } = new SqfLocation();

        public string FileName { get; }

        public ushort Line { get; }

        public uint Offset { get; }

        internal SqfcLocation Compile(SqfcFile context)
        {
            if (this == None)
            {
                return SqfcLocation.None;
            }
            var fileIndex = context.FileNames.IndexOf(FileName);
            if (fileIndex == -1)
            {
                fileIndex = context.FileNames.Count;
                context.FileNames.Add(FileName);
            }
            return new SqfcLocation(Offset, (byte)fileIndex, Line);
        }
    }
}