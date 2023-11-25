using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BIS.Core.Compression;
using BIS.Core.Streams;

namespace BIS.SQFC
{
    internal static class SqfcStreamHelper
    {
        internal static string ReadSqfcString(this BinaryReaderEx reader)
        {
            var bytes = new byte[4];
            reader.Read(bytes, 0, 3);
            var length = BinaryPrimitives.ReadInt32LittleEndian(bytes);
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }

        internal static void WriteSqfcString(this BinaryWriterEx writer, string str)
        {
            var utf8 = Encoding.UTF8.GetBytes(str);
            var bytes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(bytes, utf8.Length);
            writer.Write(bytes, 0, 3);
            writer.Write(utf8);
        }

        internal static void WriteSqfcCompressed(this BinaryWriterEx output, Action<BinaryWriterEx> writeUncompressed)
        {
            var uncompressed = new MemoryStream();
            writeUncompressed(new BinaryWriterEx(uncompressed));
            WriteSqfcCompressed(output, uncompressed.ToArray());
        }

        internal static void WriteSqfcCompressed(this BinaryWriterEx output, byte[] bytes)
        {
            output.Write((uint)bytes.Length);
            output.Write((byte)2);
            output.Write(MiniLZO.MiniLZO.Compress(bytes));
        }

        internal static void ReadSqfcCompressed(this BinaryReaderEx input, Action<BinaryReaderEx> readUncompressed)
        {
            var uncompressedSize = input.ReadUInt32();
            var compressionMode = input.ReadByte();
            if (compressionMode == 2)
            {
                readUncompressed(new BinaryReaderEx(new MemoryStream(LZO.ReadLZO(input.BaseStream, uncompressedSize))));
            }
            else if (compressionMode == 0)
            {
                readUncompressed(input);
            }
            else
            {
                throw new IOException($"Unexpected compression mode: {compressionMode}");
            }
        }
        public static IEnumerable<T> ReadRange<T>(this BinaryReaderEx input, Func<BinaryReaderEx, T> readElement, int size)
        {
            for (int i = 0; i < size; i++)
            {
                yield return readElement(input);
            }
        }
    }
}
