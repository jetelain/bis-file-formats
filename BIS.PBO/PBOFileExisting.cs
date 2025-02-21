using System.IO;

namespace BIS.PBO
{
    public class PBOFileExisting : IPBOFileEntry
    {
        private readonly FileEntry fileEntry;
        private readonly PBO pbo;

        public PBOFileExisting(FileEntry fileEntry, PBO pbo)
        {
            this.fileEntry = fileEntry;
            this.pbo = pbo;
        }

        public string FileName => fileEntry.FileName;

        public int TimeStamp => fileEntry.TimeStamp;

        public int Size => fileEntry.IsCompressed ? fileEntry.UncompressedSize : fileEntry.DataSize;

        public bool IsCompressed => fileEntry.IsCompressed;

        public int DiskSize => fileEntry.DataSize;

        public string PboFile => pbo.PBOFilePath;

        public byte[] GetFileData()
        {
            return pbo.GetFileData(fileEntry);
        }

        public byte[] GetCompressedData()
        {
            return pbo.GetCompressedData(fileEntry);
        }

        public Stream OpenRead()
        {
            return pbo.GetFileEntryStream(fileEntry);
        }
    }
}
