using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    public sealed class SqfcFile : IReadWriteObject
    {
        public int Version { get; set; }

        internal List<SqfcConstant> Constants { get; } = new List<SqfcConstant>();

        internal List<string> CommandNameDirectory { get; } = new List<string>();

        internal List<string> FileNames { get; } = new List<string>();

        internal ulong CodeIndex { get; set; }

        public void Read(BinaryReaderEx input)
        {
            CommandNameDirectory.Clear();
            Constants.Clear();
            FileNames.Clear();

            Version = input.ReadInt32();

            while (!input.HasReachedEnd)
            {
                var blockType = (SqfcFileBlockType)input.ReadByte();

                switch (blockType)
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
            Constants.AddRange(SqfcConstant.ReadRange(input, this, input.ReadUInt16()));
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

        public void CompileSqf(SqfCodeBlock sqf)
        {
            Version = 1;
            CommandNameDirectory.Clear();
            Constants.Clear();
            FileNames.Clear();
            CodeIndex = MakeConstant(sqf.CreateRootConstant(this));
        }

        public override string ToString()
        {
            return Constants[(int)CodeIndex].ToString(this);
        }

        public SqfCodeBlock ToSqf()
        {
            return ((SqfcConstantCode)Constants[(int)CodeIndex]).ToRootExpression(this);
        }

        internal ushort MakeConstantString(string value)
        {
            return MakeConstant(new SqfcConstantString(value));
        }

        internal ushort MakeConstantScalar(float value)
        {
            return MakeConstant(new SqfcConstantScalar(value));
        }

        internal ushort MakeConstantBoolean(bool value)
        {
            return MakeConstant(new SqfcConstantBoolean(value));
        }
        internal ushort MakeConstantNular(string name)
        {
            return MakeConstant(new SqfcConstantNularCommand(name));
        }

        internal ushort MakeConstant(SqfcConstant entry)
        {
            var index = Constants.IndexOf(entry);
            if (index == -1)
            {
                index = Constants.Count;
                Constants.Add(entry);
            }
            return (ushort)index;
        }

        internal void RegisterCommand(string name)
        {
            if (!CommandNameDirectory.Contains(name))
            {
                CommandNameDirectory.Add(name);
            }
        }
    }
}
