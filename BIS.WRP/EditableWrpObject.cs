﻿using System.Collections.Generic;
using System.Numerics;
using BIS.Core.Math;
using BIS.Core.Streams;

namespace BIS.WRP
{
    public sealed class EditableWrpObject
    {
        public static EditableWrpObject Dummy = new EditableWrpObject()
        {
            Model = "",
            ObjectID = int.MaxValue, 
            Transform = new Matrix4P(new Matrix4x4(
                        float.NaN, float.NaN, float.NaN, 0f,
                        float.NaN, float.NaN, float.NaN, 0f,
                        float.NaN, float.NaN, float.NaN, 0f,
                        float.NaN, float.NaN, float.NaN, 1f))
        };

        public EditableWrpObject()
        {

        }

        internal EditableWrpObject(BinaryReaderEx input, Dictionary<string, string> models)
        {
            Transform = new Matrix4P(input);
            ObjectID = input.ReadInt32();
            Model = input.ReadAscii32();
            if (models.TryGetValue(Model, out var model))
            {
                Model = model;
            }
            else
            {
                models.Add(Model, Model);
            }
        }

        public Matrix4P Transform { get; set; }
        public int ObjectID { get; set; }
        public string Model { get; set; }

        internal void Write(BinaryWriterEx output)
        {
            Transform.Write(output);
            output.Write(ObjectID);
            output.WriteAscii32(Model);
        }
    }
}
