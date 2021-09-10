
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Scanlab.Sirius
{
    internal class STLHelper
    {
        public List<STLFace> Facets = new List<STLFace>();

        public float MinX { get; set; }

        public float MaxX { get; set; }

        public float MinY { get; set; }

        public float MaxY { get; set; }

        public float MinZ { get; set; }

        public float MaxZ { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Depth { get; set; }

        public Vector3 Center { get; set; }

        public static T ByteToType<T>(BinaryReader reader)
        {
            GCHandle gcHandle = GCHandle.Alloc((object)reader.ReadBytes(Marshal.SizeOf(typeof(T))), GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
            gcHandle.Free();
            return structure;
        }

        public STLHelper(string fileName)
        {
            this.Facets.Clear();
            bool flag = false;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                using (BinaryReader binaryReader = new BinaryReader(streamReader.BaseStream))
                    flag = !Encoding.Default.GetString(binaryReader.ReadBytes(80)).Trim().ToLower().Contains("solid");
            }
            if (flag)
            {
                using (BinaryReader reader = new BinaryReader((Stream)new FileStream(fileName, FileMode.Open)))
                {
                    reader.ReadBytes(80);
                    uint num1 = reader.ReadUInt32();
                    Logger.Log(Logger.Type.Debug, string.Format("importing stl by binary format. facet counts : {0}", (object)num1), Array.Empty<object>());
                    this.Facets = new List<STLFace>((int)num1);
                    for (uint index = 0; index < num1; ++index)
                    {
                        STLFace stlFace = new STLFace();
                        stlFace.Normal = STLHelper.ByteToType<STLVertex>(reader);
                        stlFace.V1 = STLHelper.ByteToType<STLVertex>(reader);
                        stlFace.V2 = STLHelper.ByteToType<STLVertex>(reader);
                        stlFace.V3 = STLHelper.ByteToType<STLVertex>(reader);
                        int num2 = (int)reader.ReadUInt16();
                        this.Facets.Add(stlFace);
                    }
                }
            }
            else
            {
                uint num = 0;
                char[] chArray = new char[3] { ' ', '\r', '\n' };
                string empty = string.Empty;
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    streamReader.ReadLine();
                    do
                    {
                        STLFace stlFace = new STLFace();
                        string[] strArray1 = streamReader.ReadLine().Split(chArray);
                        if (!strArray1[0].Contains("endsolid"))
                        {
                            stlFace.Normal.X = float.Parse(strArray1[1]);
                            stlFace.Normal.Y = float.Parse(strArray1[2]);
                            stlFace.Normal.Z = float.Parse(strArray1[3]);
                            streamReader.ReadLine();
                            string[] strArray2 = streamReader.ReadLine().Split(chArray);
                            stlFace.V1.X = float.Parse(strArray2[1]);
                            stlFace.V1.Y = float.Parse(strArray2[2]);
                            stlFace.V1.Z = float.Parse(strArray2[3]);
                            string[] strArray3 = streamReader.ReadLine().Split(chArray);
                            stlFace.V2.X = float.Parse(strArray3[1]);
                            stlFace.V2.Y = float.Parse(strArray3[2]);
                            stlFace.V2.Z = float.Parse(strArray3[3]);
                            string[] strArray4 = streamReader.ReadLine().Split(chArray);
                            stlFace.V3.X = float.Parse(strArray4[1]);
                            stlFace.V3.Y = float.Parse(strArray4[2]);
                            stlFace.V3.Z = float.Parse(strArray4[3]);
                            streamReader.ReadLine();
                            streamReader.ReadLine();
                            this.Facets.Add(stlFace);
                            ++num;
                        }
                        else
                            break;
                    }
                    while (!streamReader.EndOfStream);
                    Logger.Log(Logger.Type.Debug, string.Format("importing stl by text format. facet counts : {0}", (object)num), Array.Empty<object>());
                }
            }
            this.MinX = float.MaxValue;
            this.MaxX = float.MinValue;
            this.MinY = float.MaxValue;
            this.MaxY = float.MinValue;
            this.MinZ = float.MaxValue;
            this.MaxZ = float.MinValue;
            foreach (STLFace facet in this.Facets)
            {
                if ((double)facet.V1.X < (double)this.MinX)
                    this.MinX = facet.V1.X;
                if ((double)facet.V1.X > (double)this.MaxX)
                    this.MaxX = facet.V1.X;
                if ((double)facet.V1.Y < (double)this.MinY)
                    this.MinY = facet.V1.Y;
                if ((double)facet.V1.Y > (double)this.MaxY)
                    this.MaxY = facet.V1.Y;
                if ((double)facet.V1.Z < (double)this.MinZ)
                    this.MinZ = facet.V1.Z;
                if ((double)facet.V1.Z > (double)this.MaxZ)
                    this.MaxZ = facet.V1.Z;
                if ((double)facet.V2.X < (double)this.MinX)
                    this.MinX = facet.V2.X;
                if ((double)facet.V2.X > (double)this.MaxX)
                    this.MaxX = facet.V2.X;
                if ((double)facet.V2.Y < (double)this.MinY)
                    this.MinY = facet.V2.Y;
                if ((double)facet.V2.Y > (double)this.MaxY)
                    this.MaxY = facet.V2.Y;
                if ((double)facet.V2.Z < (double)this.MinZ)
                    this.MinZ = facet.V2.Z;
                if ((double)facet.V2.Z > (double)this.MaxZ)
                    this.MaxZ = facet.V2.Z;
                if ((double)facet.V3.X < (double)this.MinX)
                    this.MinX = facet.V3.X;
                if ((double)facet.V3.X > (double)this.MaxX)
                    this.MaxX = facet.V3.X;
                if ((double)facet.V3.Y < (double)this.MinY)
                    this.MinY = facet.V3.Y;
                if ((double)facet.V3.Y > (double)this.MaxY)
                    this.MaxY = facet.V3.Y;
                if ((double)facet.V3.Z < (double)this.MinZ)
                    this.MinZ = facet.V3.Z;
                if ((double)facet.V3.Z > (double)this.MaxZ)
                    this.MaxZ = facet.V3.Z;
            }
            this.Width = this.MaxX - this.MinX;
            this.Height = this.MaxY - this.MinY;
            this.Depth = this.MaxZ - this.MinZ;
            this.Center = new Vector3((float)(((double)this.MinX + (double)this.MaxX) / 2.0), (float)(((double)this.MinY + (double)this.MaxY) / 2.0), (float)(((double)this.MinZ + (double)this.MaxZ) / 2.0));
        }
    }
}
