// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Vertex
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>
    /// vertex 구조체
    /// (.NET의 Vector2를 propertygrid 에 출력하기 용이하지 않아, Vector2 와 유사한 기능의 구조체를 만듦
    /// Points 엔티티 등 에서 사용
    /// </summary>
    [JsonObject]
    public struct Vertex
    {
        /// <summary>위치</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Position")]
        [Description("X,Y 위치 (mm)")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Location { get; set; }

        /// <summary>생성자</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vertex(float x, float y) => this.Location = new Vector2(x, y);

        /// <summary>생성자</summary>
        /// <param name="v"></param>
        public Vertex(Vector2 v) => this.Location = v;

        /// <summary>이동량</summary>
        /// <param name="v"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public static Vertex Translate(Vertex v, float dx, float dy) => new Vertex(v.Location.X + dx, v.Location.Y + dy);

        /// <summary>이동량</summary>
        /// <param name="v"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Vertex Translate(Vertex v, Vector2 delta) => new Vertex(v.Location.X + delta.X, v.Location.Y + delta.Y);

        /// <summary>회전량</summary>
        /// <param name="vertex"></param>
        /// <param name="angle"></param>
        /// <param name="rotateCenter"></param>
        /// <returns></returns>
        public static Vertex Rotate(Vertex vertex, float angle, Vector2 rotateCenter) => new Vertex(Vector2.Transform(vertex.Location, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter)));

        /// <summary>크기변환</summary>
        /// <param name="vertex"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Vertex Scale(Vertex vertex, Vector2 scale) => new Vertex(vertex.Location * scale);

        /// <summary>크기변환</summary>
        /// <param name="vertex"></param>
        /// <param name="scale"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static Vertex Scale(Vertex vertex, Vector2 scale, Vector2 center) => new Vertex((vertex.Location - center) * scale + center);

        /// <summary>두 정점의 거리</summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Distance(Vertex v1, Vertex v2)
        {
            Vector2 vector2 = v1.Location - v2.Location;
            return Math.Sqrt((double)vector2.X * (double)vector2.X + (double)vector2.Y * (double)vector2.Y);
        }

        /// <summary>두 정점 벡터가 이루는 각도</summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Angle(Vertex v1, Vertex v2) => Vertex.Angle(v2 - v1);

        /// <summary>지정된 폴리라인 벡터의 회전 각도</summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Angle(Vertex v)
        {
            double num = Math.Atan2((double)v.Location.Y, (double)v.Location.X);
            return num < 0.0 ? 2.0 * Math.PI + num : num;
        }

        /// <summary>3x3 행렬로 계산된 결과 리턴</summary>
        [JsonIgnore]
        [Browsable(false)]
        public Matrix3x2 ToMatrix => Matrix3x2.CreateTranslation(this.Location);

        /// <summary>문자열</summary>
        /// <returns></returns>
        public override string ToString() => string.Format("{0:F3}, {1:F3}", (object)this.Location.X, (object)this.Location.Y);

        public LwPolyLineVertex ToPolylineVertex() => new LwPolyLineVertex(this.Location);

        /// <summary>0 값으로 초기화된 벡터 리턴</summary>
        [JsonIgnore]
        [Browsable(false)]
        public static Vertex Zero => new Vertex(0.0f, 0.0f);

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vertex vertex1))
                return false;
            double x1 = (double)this.Location.X;
            Vertex vertex2 = vertex1;
            double x2 = (double)vertex2.Location.X;
            if (!MathHelper.IsEqual((float)x1, (float)x2))
                return false;
            double y1 = (double)this.Location.Y;
            vertex2 = (Vertex)obj;
            double y2 = (double)vertex2.Location.Y;
            return MathHelper.IsEqual((float)y1, (float)y2);
        }

        public static Vertex operator +(Vertex left, Vertex right) => new Vertex(left.Location + right.Location);

        public static Vertex operator -(Vertex left, Vertex right) => new Vertex(left.Location - right.Location);

        public static Vertex operator *(Vertex left, Vertex right) => new Vertex(left.Location * right.Location);

        public static Vertex operator /(Vertex left, Vertex right) => new Vertex(left.Location / right.Location);

        public static bool operator ==(Vertex left, Vertex right) => MathHelper.IsEqual(left.Location.X, right.Location.X) && MathHelper.IsEqual(left.Location.Y, right.Location.Y);

        public static bool operator !=(Vertex left, Vertex right) => !MathHelper.IsEqual(left.Location.X, right.Location.X) || !MathHelper.IsEqual(left.Location.Y, right.Location.Y);
    }
}
