using System;
using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal class SqfcLocation
    {
        public SqfcLocation(uint offset, byte fileIndex, ushort line)
        {
            Offset = offset;
            FileIndex = fileIndex;
            Line = line;
        }

        public static SqfcLocation None { get; } = new SqfcLocation(0, 0, 0);

        public uint Offset { get; }

        public byte FileIndex { get; }

        public ushort Line { get; }

        internal static SqfcLocation Read(BinaryReaderEx reader)
        {
            return new SqfcLocation(
                reader.ReadUInt32(),
                reader.ReadByte(),
                reader.ReadUInt16());
        }

        internal void WriteTo(BinaryWriterEx writer)
        {
            writer.Write(Offset);
            writer.Write(FileIndex);
            writer.Write(Line);
        }

        public override string ToString()
        {
            return $"File#{FileIndex}@{Line}/{Offset}";
        }

        public string ToString(SqfcFile file)
        {
            if (file == null)
            {
                return ToString();
            }
            return $"{file.FileNames[FileIndex]}@{Line}/{Offset}";
        }
    }
}