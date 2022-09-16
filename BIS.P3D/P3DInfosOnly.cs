﻿using System;
using System.IO;
using BIS.Core.Streams;

namespace BIS.P3D
{
    public class P3DInfosOnly : IReadObject
    {
        private MLOD.MLOD editable;
        private ODOL.ODOL binarized;

        public IModelInfo ModelInfo => 
            binarized?.ModelInfo 
            ?? editable.ModelInfo;

        public bool IsEditable =>  editable != null;

        public int Version => binarized?.Version ?? editable.Version;

        public static bool IsODOL(string filePath)
        {
            return IsODOL(File.OpenRead(filePath));
        }

        public static bool IsODOL(Stream stream)
        {
            bool result = false;
            if (stream.ReadByte() == 'O'
            && stream.ReadByte() == 'D'
            && stream.ReadByte() == 'O'
            && stream.ReadByte() == 'L')
                result = true; ;

            stream.Position = 0;

            return result;
        }
        public static bool IsMLOD(string filePath)
        {
            return IsMLOD(File.OpenRead(filePath));
        }

        public static bool IsMLOD(Stream stream)
        {
            bool result = false;
            if (stream.ReadByte() == 'M'
            && stream.ReadByte() == 'L'
            && stream.ReadByte() == 'O'
            && stream.ReadByte() == 'D')
                result = true; ;

            stream.Position = 0;

            return result;
        }

        public void Read(BinaryReaderEx input)
        {
            var signature = input.ReadAscii(4);
            switch (signature)
            {
                case "ODOL":
                    binarized = new ODOL.ODOL();
                    binarized.ReadHeaderOnly(input);
                    editable = null;
                    break;
                case "MLOD":
                    editable = new MLOD.MLOD();
                    editable.ReadContent(input);
                    binarized = null;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown P3D format '{signature}'");
            }
        }
    }
}
