using System.Collections.Concurrent;
using System.CommandLine;
using BIS.PBO;

namespace PboUtil
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            var source = new Argument<string>("source", "Source file");
            var target = new Argument<string>("target", () => "", "Target file");
            var extensions = new Option<string>("--extensions", () => ".paa;.p3d", "Extensions to compress");
            var compress = new Command("compress", "Compress PBO file") { source, target, extensions };
            compress.SetHandler(Compress, source, target, extensions);
            var root = new RootCommand();
            root.AddCommand(compress);
            return root.Invoke(args);
        }

        private static void Compress(string source, string target, string extensions)
        {
            var extensionsArray = extensions.Split(',',' ',';');
            var pbo = new PBO(source, false);
            var locker = new object();

            var originalSize = pbo.Files.Sum(f => f.Size);
            var overallGain = 0;
            var compressed = 0;

            var newFiles = new ConcurrentStack<IPBOFileEntry>();
            var toRemove = new ConcurrentStack<IPBOFileEntry>();

            Parallel.ForEach(pbo.Files.ToList(), file =>
            {
                if (extensionsArray.Contains(Path.GetExtension(file.FileName), StringComparer.OrdinalIgnoreCase))
                {
                    var result = new PBOFileToCompress(file);
                    if (result.DiskSize < result.Size)
                    {
                        Interlocked.Add(ref overallGain, result.Size - result.DiskSize);
                        Interlocked.Increment(ref compressed);
                        toRemove.Push(file);
                        newFiles.Push(result);
                    }
                }
            });
            foreach(var remove in toRemove)
            {
                pbo.Files.Remove(remove);
            }
            pbo.Files.AddRange(newFiles);
            pbo.Files.Sort((a, b) => a.FileName.CompareTo(b.FileName));
            pbo.SaveTo(string.IsNullOrEmpty(target) ? source : target);
            Console.WriteLine($"Compressed {compressed} entries, Compression {(originalSize-overallGain) * 100.0/ originalSize:0.0} %");
        }
    }
}
