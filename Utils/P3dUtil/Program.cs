using System;
using System.IO;
using BIS.P3D.MLOD;
using CommandLine;
using Tomlet;

namespace P3dUtil
{
    class Program
    {
        [Verb("template", HelpText = "Generate P3D files based on a template definition.")]
        class TemlateOptions
        {
            [Value(0, MetaName = "template", HelpText = "Template definition TOML file.", Required = true)]
            public string TemplateDefinition { get; set; }

            [Option('n', "no-backup", Required = false, HelpText = "Do not generate a backup files (.p3d.bak).")]
            public bool NoBackup { get; set; }
        }

        [Verb("replace-path", HelpText = "Replace a path in all texture and material reference of a P3D file.")]
        class ReplaceOptions
        {
            [Value(0, MetaName = "model", HelpText = "P3D file(s) (can be a pattern)", Required = true)]
            public string Source { get; set; }

            [Value(1, MetaName = "old-path", HelpText = "Old path to be replaced.", Required = true)]
            public string OldPath { get; set; }

            [Value(2, MetaName = "new-path", HelpText = "New path to use.", Required = true)]
            public string NewPath { get; set; }

            [Option('n', "no-backup", Required = false, HelpText = "Do not generate a backup file (.p3d.bak).")]
            public bool NoBackup { get; set; }

            [Option('r', "recursive", Required = false, HelpText = "If model is a pattern, do a recursive file search.")]
            public bool IsRecursive { get; set; }
        }


        public static int Main(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<TemlateOptions, ReplaceOptions>(args)
              .MapResult(
                (TemlateOptions opts) => Templating(opts),
                (ReplaceOptions opts) => Replace(opts),
                errs => 1);
        }

        private static int Templating(TemlateOptions opts)
        {
            var document = TomlParser.ParseFile(opts.TemplateDefinition);
            var documentDirectory = Path.GetDirectoryName(Path.GetFullPath(opts.TemplateDefinition));
            var definition = TomletMain.To<TemlateDefinition>(document);

            var backup = opts.NoBackup ? false : definition.Backup ?? true;

            if (string.Equals(definition.Mode, "per-texture", StringComparison.OrdinalIgnoreCase))
            {
                PerTextureTemplating(documentDirectory, definition, backup);
            }
            return 0;
        }

        private static void PerTextureTemplating(string directory, TemlateDefinition definition, bool backup)
        {
            definition.TemplateFile = Path.Combine(directory, definition.TemplateFile);
            definition.TextureBaseDirectory = Path.Combine(directory, definition.TextureBaseDirectory);
            definition.TexturePattern = definition.TexturePattern ?? "*.paa";
            definition.TextureNameFilter = definition.TextureNameFilter ?? definition.TexturePattern.Replace("*", "");
            definition.TextureBaseGamePath = definition.TextureBaseGamePath ?? "";

            Console.WriteLine($"TemplateFile         = '{definition.TemplateFile}'");
            Console.WriteLine($"TextureBaseDirectory = '{definition.TextureBaseDirectory}'");
            Console.WriteLine($"TextureBaseGamePath  = '{definition.TextureBaseGamePath}'");
            Console.WriteLine($"TexturePattern       = '{definition.TexturePattern}'");
            Console.WriteLine($"TextureNameFilter    = '{definition.TextureNameFilter}'");

            var master = new MLOD(definition.TemplateFile);
            foreach (var file in Directory.GetFiles(definition.TextureBaseDirectory, definition.TexturePattern, SearchOption.AllDirectories))
            {
                var p3d = file.Replace(definition.TextureNameFilter, ".p3d", StringComparison.OrdinalIgnoreCase);
                if (!string.Equals(definition.TemplateFile, p3d, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Process '{file}'...");
                    if (File.Exists(p3d) && backup)
                    {
                        BackupFile(p3d);
                    }
                    var newTexture = file.Replace(definition.TextureBaseDirectory, definition.TextureBaseGamePath, StringComparison.OrdinalIgnoreCase);
                    Console.WriteLine($"  '{definition.InitialTexture}' -> '{newTexture}'");
                    foreach (var lod in master.Lods)
                    {
                        foreach (var face in lod.Faces)
                        {
                            if (string.Equals(face.Texture, definition.InitialTexture, StringComparison.OrdinalIgnoreCase))
                            {
                                face.Texture = newTexture;
                            }
                        }
                    }
                    master.WriteToFile(p3d, true);
                }
            }
        }

        private static int Replace(ReplaceOptions opts)
        {
            if (Path.GetFileNameWithoutExtension(opts.Source).Contains("*"))
            {
                var files = Directory.GetFiles(Path.GetDirectoryName(opts.Source), Path.GetFileName(opts.Source), opts.IsRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    Replace(file, opts.OldPath, opts.NewPath, !opts.NoBackup);
                }
            }
            else
            {
                if (!File.Exists(opts.Source))
                {
                    Console.Error.WriteLine($"File '{opts.Source}' does not exists.");
                    return 1;
                }
                Replace(opts.Source, opts.OldPath, opts.NewPath, !opts.NoBackup);
            }
            return 0;
        }
        private static void Replace(string file, string oldPath, string newPath, bool backup)
        {
            Console.WriteLine($"Process '{file}'...");
            if (backup)
            {
                BackupFile(file);
            }
            var p3d = new MLOD(file);
            foreach(var lod in p3d.Lods)
            {
                foreach (var face in lod.Faces)
                {
                    face.Material = face.Material?.Replace(oldPath, newPath, StringComparison.OrdinalIgnoreCase);
                    face.Texture = face.Texture?.Replace(oldPath, newPath, StringComparison.OrdinalIgnoreCase);
                }
            }
            p3d.WriteToFile(file, true);
            Console.WriteLine("  Done");
        }

        private static void BackupFile(string file)
        {
            int backupNum = 0;
            string backupPath = Path.ChangeExtension(file, ".p3d.bak");
            while (File.Exists(backupPath))
            {
                backupNum++;
                backupPath = Path.ChangeExtension(file, ".p3d.bak" + backupNum);
            }
            File.Copy(file, backupPath);
            Console.WriteLine($"  Backup to '{backupPath}'");
        }
    }
}
