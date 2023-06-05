using BIS.Core.Streams;
using System;
using System.IO;
using System.IO.Compression;
using Xunit;

namespace BIS.Core.Test.Compression
{
    public class LZSSTest
    {
        private byte[] data;

        public LZSSTest()
        {
            // Use random bytes with low entropy as test input
            data = new byte[8192];
            var rng = new Random();
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)rng.Next(0, 10);
            }
        }

        [Fact]
        public void CanEncode()
        {
            using var buffer = new MemoryStream();
            using var writer = new BinaryWriterEx(buffer, true);

            writer.WriteLZSS(data, false);
            var compressed = buffer.ToArray();

            Assert.True(compressed.Length < data.Length);
        }

        [Fact]
        public void CanDecode()
        {
            using var buffer = new MemoryStream();
            using var writer = new BinaryWriterEx(buffer, true);
            using var reader = new BinaryReaderEx(buffer);

            writer.WriteLZSS(data, false);
            reader.BaseStream.Position = 0;
            var result = reader.ReadLZSS((uint)data.Length, false);

            Assert.Equal(data, result);
        }

        [Fact]
        public void LzssStreamConsistent()
        {
            var buffer = new MemoryStream();
            using var compression = new LzssStream(buffer, CompressionMode.Compress, true);
            using var decompression = new LzssStream(buffer, CompressionMode.Decompress, true);

            compression.Write(data, 0, data.Length);
            buffer.Seek(0, SeekOrigin.Begin);
            var result = new byte[data.Length];
            decompression.Read(result, 0, result.Length);

            Assert.Equal(data, result);
        }
    }
}
