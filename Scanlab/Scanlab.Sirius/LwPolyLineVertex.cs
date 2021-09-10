// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.LwPolyLineVertex
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using System;
using System.ComponentModel;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>
    /// LW(Light-Weighted) 폴리라인 엔티티용 정점(vertex) 객체
    /// Autocad 의 LWPolyline 을 모사함
    /// </summary>
    public struct LwPolyLineVertex : IEquatable<LwPolyLineVertex>, ICloneable
    {
        /// <summary>X 값</summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("X")]
        [Description("X 위치값 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float X { get; set; }

        /// <summary>Y 값</summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Y")]
        [Description("Y 위치값 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Y { get; set; }

        /// <summary>Bulge 값 (1/4 atan)</summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Bulge")]
        [Description("정점의 Bulge 값 (0: 직선)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Bulge { get; set; }

        /// <summary>Bulge 값 (1/4 atan)</summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Ramp")]
        [Description("자동 레이저 출력 (Vector Defined) 사용시의 비율값 (기본값: 1.0)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Ramp { get; set; }

        /// <summary>constructor</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="bulge"></param>
        public LwPolyLineVertex(float x, float y, float bulge = 0.0f)
        {
            this.X = x;
            this.Y = y;
            this.Bulge = bulge;
            this.Ramp = 1f;
        }

        /// <summary>생성자</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="bulge"></param>
        /// <param name="rampFactor"></param>
        public LwPolyLineVertex(float x, float y, float bulge, float rampFactor = 1f)
          : this(x, y, bulge)
        {
            this.Ramp = rampFactor;
        }

        /// <summary>constructor</summary>
        /// <param name="v">Vector2</param>
        /// <param name="bulge">bulge</param>
        public LwPolyLineVertex(Vector2 v, float bulge = 0.0f)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Bulge = bulge;
            this.Ramp = 1f;
        }

        /// <summary>동일성 검사</summary>
        /// <param name="other">Another color to compare to.</param>
        /// <returns>True if the three components are equal or false in any other case.</returns>
        public bool Equals(LwPolyLineVertex other) => MathHelper.IsEqual(other.X, this.X) && MathHelper.IsEqual(other.Y, this.Y) && MathHelper.IsEqual(other.Bulge, this.Bulge) && MathHelper.IsEqual(other.Ramp, this.Ramp);

        /// <summary>문자열</summary>
        /// <returns></returns>
        public override string ToString() => string.Format("{0:F3}, {1:F3}: {2:F3}", (object)this.X, (object)this.Y, (object)this.Bulge);

        /// <summary>복제</summary>
        /// <returns></returns>
        public object Clone() => (object)new LwPolyLineVertex()
        {
            X = this.X,
            Y = this.Y,
            Bulge = this.Bulge,
            Ramp = this.Ramp
        };

        /// <summary>이동</summary>
        /// <param name="vertex"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static LwPolyLineVertex Translate(LwPolyLineVertex vertex, Vector2 delta)
        {
            vertex.X += delta.X;
            vertex.Y += delta.Y;
            return vertex;
        }

        /// <summary>회전</summary>
        /// <param name="vertex"></param>
        /// <param name="angle"></param>
        /// <param name="rotateCenter"></param>
        /// <returns></returns>
        public static LwPolyLineVertex Rotate(
          LwPolyLineVertex vertex,
          float angle,
          Vector2 rotateCenter)
        {
            Vector2 vector2 = Vector2.Transform(new Vector2(vertex.X, vertex.Y), Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            vertex.X = vector2.X;
            vertex.Y = vector2.Y;
            return vertex;
        }

        /// <summary>크기변환</summary>
        /// <param name="vertex"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static LwPolyLineVertex Scale(LwPolyLineVertex vertex, Vector2 scale)
        {
            Vector2 vector2 = new Vector2(vertex.X, vertex.Y) * scale;
            vertex.X = vector2.X;
            vertex.Y = vector2.Y;
            return vertex;
        }

        /// <summary>크기변환</summary>
        /// <param name="vertex"></param>
        /// <param name="scale"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static LwPolyLineVertex Scale(
          LwPolyLineVertex vertex,
          Vector2 scale,
          Vector2 center)
        {
            Vector2 vector2 = new Vector2(vertex.X - center.X, vertex.Y - center.Y) * scale;
            vertex.X = vector2.X + center.X;
            vertex.Y = vector2.Y + center.Y;
            return vertex;
        }

        /// <summary>두 폴리라인 벡터의 차</summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static LwPolyLineVertex operator -(
          LwPolyLineVertex left,
          LwPolyLineVertex right)
        {
            return new LwPolyLineVertex(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>두 폴리라인 벡터의 거리</summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Distance(LwPolyLineVertex v1, LwPolyLineVertex v2) => Math.Sqrt(((double)v1.X - (double)v2.X) * ((double)v1.X - (double)v2.X) + ((double)v1.Y - (double)v2.Y) * ((double)v1.Y - (double)v2.Y));

        /// <summary>두 폴리라인 벡터가 이루는 각도</summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Angle(LwPolyLineVertex v1, LwPolyLineVertex v2) => LwPolyLineVertex.Angle(v2 - v1);

        /// <summary>지정된 폴리라인 벡터의 회전 각도</summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Angle(LwPolyLineVertex v)
        {
            double num = Math.Atan2((double)v.Y, (double)v.X);
            return num < 0.0 ? 2.0 * Math.PI + num : num;
        }
    }
}
