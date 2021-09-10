
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>오프셋(X,Y,회전) 정보를 가지는 구조체</summary>
    [JsonObject]
    public struct Offset
    {
        /// <summary>X 위치</summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("X")]
        [Description("X 위치 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float X { get; set; }

        /// <summary>Y 위치</summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Y")]
        [Description("Y 위치 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Y { get; set; }

        /// <summary>회전량 (각도)</summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Angle")]
        [Description("각도 (+ : CCW)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Angle { get; set; }

        /// <summary>사용자 정의 데이타</summary>
        [Browsable(false)]
        [ReadOnly(false)]
        public object Tag { get; set; }

        /// <summary>생성자</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="angle"></param>
        public Offset(float x, float y, float angle = 0.0f)
        {
            this.X = x;
            this.Y = y;
            this.Angle = angle;
            this.Tag = (object)null;
        }

        /// <summary>생성자</summary>
        /// <param name="v"></param>
        /// <param name="angle"></param>
        public Offset(Vector2 v, float angle = 0.0f)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Angle = angle;
            this.Tag = (object)null;
        }

        public Offset Clone() => new Offset()
        {
            X = this.X,
            Y = this.Y,
            Angle = this.Angle,
            Tag = this.Tag
        };

        /// <summary>이동량</summary>
        /// <param name="o"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public static Offset Translate(Offset o, float dx, float dy) => new Offset(o.X + dx, o.Y + dy, o.Angle);

        /// <summary>이동량</summary>
        /// <param name="o"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Offset Translate(Offset o, Vector2 delta) => new Offset(o.X + delta.X, o.Y + delta.Y, o.Angle);

        /// <summary>회전량</summary>
        /// <param name="o"></param>
        /// <param name="angle"></param>
        /// <param name="rotateCenter"></param>
        /// <returns></returns>
        public static Offset Rotate(Offset o, float angle, Vector2 rotateCenter) => new Offset(Vector2.Transform(o.ToVector2, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter)), o.Angle + angle);

        /// <summary>회전량</summary>
        /// <param name="o"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Offset Scale(Offset o, Vector2 scale) => new Offset(o.ToVector2 * scale, o.Angle);

        /// <summary>크기 변화량</summary>
        /// <param name="o"></param>
        /// <param name="scale"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static Offset Scale(Offset o, Vector2 scale, Vector2 center) => new Offset((o.ToVector2 - center) * scale + center, o.Angle);

        /// <summary>0 값으로 초기화된 객체 반환</summary>
        [JsonIgnore]
        [Browsable(false)]
        public static Offset Zero => new Offset(0.0f, 0.0f);

        /// <summary>Vector2 구조체로 변환된 값 반환 (X, Y 성분만)</summary>
        [JsonIgnore]
        [Browsable(false)]
        public Vector2 ToVector2 => new Vector2(this.X, this.Y);

        /// <summary>3x3 행렬값으로 계산된 값 반환 (X, Y, 회전성분 포함)</summary>
        [JsonIgnore]
        [Browsable(false)]
        public Matrix3x2 ToMatrix => Matrix3x2.CreateTranslation(this.X, this.Y) * Matrix3x2.CreateRotation((float)Math.PI / 180f * this.Angle);

        /// <summary>문자열</summary>
        /// <returns></returns>
        public override string ToString() => string.Format("{0:F3}, {1:F3}, {2:F3}", (object)this.X, (object)this.Y, (object)this.Angle);
    }
}
