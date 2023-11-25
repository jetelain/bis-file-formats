using System.Collections.Generic;
using System.IO;
using BIS.Core.Streams;

namespace BIS.SQFC
{
    public class SqfcFile : IReadWriteObject
    {
        public int Version { get; set; }

        public List<SqfcConstant> Constants { get; } = new List<SqfcConstant>();

        public List<string> CommandNameDirectory { get; } = new List<string>();

        public List<string> FileNames { get; } = new List<string>();

        public ulong CodeIndex { get; set; }

        public void Read(BinaryReaderEx input)
        {
            CommandNameDirectory.Clear();
            Constants.Clear();
            FileNames.Clear();

            Version = input.ReadInt32();

            while(!input.HasReachedEnd)
            {
                var blockType = (SqfcFileBlockType)input.ReadByte();
                
                switch(blockType)
                {
                    case SqfcFileBlockType.Constant:
                        ReadConstantBlock(input);
                        break;
                    case SqfcFileBlockType.ConstantCompressed:
                        SqfcStreamHelper.ReadSqfcCompressed(input, ReadConstantBlock);
                        break;
                    case SqfcFileBlockType.LocationInfo:
                        FileNames.AddRange(input.ReadArrayBase(r => r.ReadSqfcString(), input.ReadUInt16()));
                        break;
                    case SqfcFileBlockType.Code:
                        CodeIndex = input.ReadUInt64();
                        break;
                    case SqfcFileBlockType.CommandNameDirectory:
                        ReadCommandNameDirectoryBlock(input);
                        break;
                    default:
                        throw new IOException();
                }
            }
        }

        private void ReadCommandNameDirectoryBlock(BinaryReaderEx input)
        {
            SqfcStreamHelper.ReadSqfcCompressed(input, uncompressed =>
            {
                CommandNameDirectory.AddRange(uncompressed.ReadArrayBase(r => r.ReadSqfcString(), uncompressed.ReadUInt16()));
            });
        }

        private void ReadConstantBlock(BinaryReaderEx input)
        {
            Constants.AddRange(SqfcConstant.ReadArray(input, this, input.ReadUInt16()));
        }

        public void Write(BinaryWriterEx output)
        {
            output.Write(Version);
            if (CommandNameDirectory.Count > 0)
            {
                output.Write((byte)SqfcFileBlockType.CommandNameDirectory);
                output.WriteSqfcCompressed(uncompressedWriter =>
                {
                    uncompressedWriter.Write((ushort)CommandNameDirectory.Count);
                    uncompressedWriter.WriteArrayBase(CommandNameDirectory, SqfcStreamHelper.WriteSqfcString);
                });
            }

            output.Write((byte)SqfcFileBlockType.ConstantCompressed);
            output.WriteSqfcCompressed(uncompressedWriter =>
            {
                uncompressedWriter.Write((ushort)Constants.Count);
                uncompressedWriter.WriteArrayBase(Constants, (w, c) => c.WriteTo(w, this));
            });

            output.Write((byte)SqfcFileBlockType.LocationInfo);
            output.Write((ushort)FileNames.Count);
            output.WriteArrayBase(FileNames, SqfcStreamHelper.WriteSqfcString);

            output.Write((byte)SqfcFileBlockType.Code);
            output.Write((ulong)CodeIndex);
        }

        public override string ToString()
        {
            return Constants[(int)CodeIndex].ToString(this);
        }
    }
}
