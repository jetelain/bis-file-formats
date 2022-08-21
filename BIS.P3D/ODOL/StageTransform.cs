using BIS.Core.Math;
using BIS.Core.Streams;

namespace BIS.P3D.ODOL
{
    internal class StageTransform
    {
        public StageTransform(BinaryReaderEx input)
        {
            UvSource = input.ReadUInt32();
            Transformation = new Matrix4P(input);
        }

        public uint UvSource { get; }
        public Matrix4P Transformation { get; }

        public void Write(BinaryWriterEx output)
        {
            output.Write(UvSource);
            Transformation.Write(output);
        }
    }
}