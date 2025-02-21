using System.IO;

namespace BIS.PBO
{
    public interface IPBOFileEntry
    {
        string FileName { get; }
        int Size { get; }
        int TimeStamp { get; }
        Stream OpenRead();
        bool IsCompressed { get; }
        byte[] GetCompressedData();
        byte[] GetFileData();
        int DiskSize { get; }
    }
}
