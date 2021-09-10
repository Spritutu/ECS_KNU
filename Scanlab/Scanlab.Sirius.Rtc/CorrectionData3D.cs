// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.CorrectionData3D
// Assembly: spirallab.sirius.rtc, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 330B13B0-CD9B-4679-A17E-EBB26CA3FE4F
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.rtc.dll

using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>3차원 보정 데이타</summary>
    public struct CorrectionData3D
    {
        /// <summary>논리적인 좌표값 x,y (mm)</summary>
        public Vector3 Reference { get; set; }

        /// <summary>실제 측정된 좌표값 x,y (mm)</summary>
        public Vector3 Measured { get; set; }

        /// <summary>생성자</summary>
        /// <param name="reference">논리적인 좌표값</param>
        /// <param name="measured">실제 측정된 좌표값</param>
        public CorrectionData3D(Vector3 reference, Vector3 measured)
        {
            this.Reference = reference;
            this.Measured = measured;
        }

        /// <summary>논리 좌표 문자열 출력</summary>
        /// <returns></returns>
        public string ReferenceToString() => string.Format("{0:F3}, {1:F3}, {2:F3}", (object)this.Reference.X, (object)this.Reference.Y, (object)this.Reference.Z);

        /// <summary>실측값 문자열 출력</summary>
        /// <returns></returns>
        public string MeasuredToString() => string.Format("{0:F3}, {1:F3}, {2:F3}", (object)this.Measured.X, (object)this.Measured.Y, (object)this.Measured.Z);
    }
}
