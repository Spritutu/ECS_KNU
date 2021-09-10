
using System.Runtime.InteropServices;

namespace Scanlab.Sirius
{
    [StructLayout(LayoutKind.Explicit)]
    public struct STLVertex
    {
        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;
        [FieldOffset(8)]
        public float Z;

        public static STLVertex Empty => new STLVertex()
        {
            X = 0.0f,
            Y = 0.0f,
            Z = 0.0f
        };
    }
}
