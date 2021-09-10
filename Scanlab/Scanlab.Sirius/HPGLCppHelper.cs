
using System;
using System.Runtime.InteropServices;

namespace Scanlab.Sirius
{
    internal class HPGLCppHelper
    {
        private const string DLLNamex64 = "spirallab.hpglx64.dll";
        private const string DLLNamex32 = "spirallab.hpgl.dll";

        [DllImport("spirallab.hpgl.dll", EntryPoint = "HPGLImportFile", CharSet = CharSet.Ansi)]
        private static extern bool ImportHPGLFilex32(string filename);

        [DllImport("spirallab.hpglx64.dll", EntryPoint = "HPGLImportFile", CharSet = CharSet.Ansi)]
        private static extern bool ImportHPGLFilex64(string filename);

        internal static bool ImportHPGLFile(string filename) => IntPtr.Size != 8 ? HPGLCppHelper.ImportHPGLFilex32(filename) : HPGLCppHelper.ImportHPGLFilex64(filename);

        [DllImport("spirallab.hpgl.dll", EntryPoint = "HPGLPolyLineCount", CharSet = CharSet.Ansi)]
        private static extern int HPGLPolyLineCountx32();

        [DllImport("spirallab.hpglx64.dll", EntryPoint = "HPGLPolyLineCount", CharSet = CharSet.Ansi)]
        private static extern int HPGLPolyLineCountx64();

        internal static int HPGLPolyLineCount() => IntPtr.Size != 8 ? HPGLCppHelper.HPGLPolyLineCountx32() : HPGLCppHelper.HPGLPolyLineCountx64();

        [DllImport("spirallab.hpgl.dll", EntryPoint = "HPGLPolyLineVertexCount", CharSet = CharSet.Ansi)]
        private static extern int HPGLPolyLineVertexCountx32(int polyLineIndex);

        [DllImport("spirallab.hpglx64.dll", EntryPoint = "HPGLPolyLineVertexCount", CharSet = CharSet.Ansi)]
        private static extern int HPGLPolyLineVertexCountx64(int polyLineIndex);

        internal static int HPGLPolyLineVertexCount(int polyLineIndex) => IntPtr.Size != 8 ? HPGLCppHelper.HPGLPolyLineVertexCountx32(polyLineIndex) : HPGLCppHelper.HPGLPolyLineVertexCountx64(polyLineIndex);

        [DllImport("spirallab.hpgl.dll", EntryPoint = "HPGLPolyLineVertexData", CharSet = CharSet.Ansi)]
        private static extern HPGLCppHelper.HPGLVertex HPGLPolyLineVertexDatax32(
          int polyLineIndex,
          int index);

        [DllImport("spirallab.hpglx64.dll", EntryPoint = "HPGLPolyLineVertexData", CharSet = CharSet.Ansi)]
        private static extern HPGLCppHelper.HPGLVertex HPGLPolyLineVertexDatax64(
          int polyLineIndex,
          int index);

        internal static HPGLCppHelper.HPGLVertex HPGLPolyLineVertexData(
          int polyLineIndex,
          int index)
        {
            return IntPtr.Size != 8 ? HPGLCppHelper.HPGLPolyLineVertexDatax32(polyLineIndex, index) : HPGLCppHelper.HPGLPolyLineVertexDatax64(polyLineIndex, index);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal struct HPGLVertex
        {
            public float x;
            public float y;
        }
    }
}
