using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>2차원 보정 데이타 구조체</summary>
    public struct CorrectionData2D
    {
        /// <summary>이론상의 좌표값</summary>
        public Vector2 Reference { get; set; }

        /// <summary>실제 측정된 좌표값</summary>
        public Vector2 Measured { get; set; }

        /// <summary>생성자</summary>
        /// <param name="reference">이론상의 좌표값</param>
        /// <param name="measured">실제 측정된 좌표값</param>
        public CorrectionData2D(Vector2 reference, Vector2 measured)
        {
            this.Reference = reference;
            this.Measured = measured;
        }

        /// <summary>이론값 문자열 출력</summary>
        /// <returns></returns>
        public string ReferenceToString() => string.Format("{0:F3}, {1:F3}", (object)this.Reference.X, (object)this.Reference.Y);

        /// <summary>실측값 문자열 출력</summary>
        /// <returns></returns>
        public string MeasuredToString() => string.Format("{0:F3}, {1:F3}", (object)this.Measured.X, (object)this.Measured.Y);
    }
}
