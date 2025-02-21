using System.IO;
using BIS.Core.Streams;

namespace BIS.PBO
{
    public sealed class PBOFileToCompress : IPBOFileEntry
    {
        private readonly IPBOFileEntry file;
        private readonly byte[] compressedData;

        public PBOFileToCompress(FileInfo file, string pboFileName)
            : this(new PBOFileToAdd(file, pboFileName))
        {

        }

        public PBOFileToCompress(IPBOFileEntry file)
        {
            this.file = file;
            this.compressedData = CompressData(file);
        }

        private static byte[] CompressData(IPBOFileEntry file)
        {
            var mem = new MemoryStream();
            using (var writer = new BinaryWriterEx(mem))
            {
                writer.WriteLZSS(file.GetFileData());
            }
            return mem.ToArray();
        }

        public string FileName => file.FileName;

        public int Size => file.Size;

        public int TimeStamp => file.TimeStamp;

        public bool IsCompressed => true;

        public int DiskSize => compressedData.Length;

        public byte[] GetCompressedData()
        {
            return compressedData;
        }

        public Stream OpenRead()
        {
            return file.OpenRead();
        }

        public byte[] GetFileData()
        {
            return file.GetFileData();
        }
    }
}