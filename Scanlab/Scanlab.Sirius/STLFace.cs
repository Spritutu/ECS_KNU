using System.Runtime.InteropServices;

namespace Scanlab.Sirius
{
    [StructLayout(LayoutKind.Explicit)]
    public struct STLFace
    {
        [FieldOffset(0)]
        public STLVertex Normal;
        [FieldOffset(12)]
        public STLVertex V1;
        [FieldOffset(24)]
        public STLVertex V2;
        [FieldOffset(32)]
        public STLVertex V3;
    }
}
