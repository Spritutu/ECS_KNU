
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 그룹(Group) 엔티티의 offset 처리용
    /// 그룹에서 여러개의 X,Y 위치 배열 정보 처리용
    /// </summary>
    [JsonObject]
    public struct InsertVertex
    {
        private Vector2 transit;
        private Vector2? scale;
        private float angle;

        /// <summary>이동량</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Transit")]
        [Description("X,Y 이동량 (mm)")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Transit
        {
            get => this.transit;
            set => this.transit = value;
        }

        /// <summary>크기 변화량</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Scale")]
        [Description("Sx, Sy 크기 비율")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Scale
        {
            get => this.scale ?? new Vector2(1f, 1f);
            set => this.scale = new Vector2?(value);
        }

        /// <summary>회전량 (각도)</summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Angle")]
        [Description("회전 각도")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Angle
        {
            get => this.angle;
            set
            {
                this.angle = value;
                this.angle = MathHelper.NormalizeAngle(this.angle);
            }
        }

        /// <summary>3x3 행렬 계산값</summary>
        [JsonIgnore]
        [Browsable(false)]
        public Matrix3x2 ToMatrix
        {
            get
            {
                Matrix3x2 matrix3x2 = Matrix3x2.CreateTranslation(this.Transit.X, this.Transit.Y) * Matrix3x2.CreateRotation(this.Angle * ((float)Math.PI / 180f));
                Matrix3x2.CreateScale(this.Scale.X, this.Scale.Y);
                return matrix3x2;
            }
        }

        /// <summary>생성자</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="angle"></param>
        public InsertVertex(float x, float y, float sx = 1f, float sy = 1f, float angle = 0.0f)
        {
            this.scale = new Vector2?(new Vector2(sx, sy));
            this.transit = new Vector2(x, y);
            this.angle = MathHelper.NormalizeAngle(angle);
        }

        /// <summary>객체의 문자열</summary>
        /// <returns></returns>
        public override string ToString()
        {
            Vector2 vector2 = this.Transit;
            string str1 = vector2.ToString();
            vector2 = this.Scale;
            string str2 = vector2.ToString();
            // ISSUE: variable of a boxed type
            float angle = this.Angle;
            return string.Format("D:{0}, S:{1}, A:{2:F3}º", str1, str2, angle);
        }

        /// <summary>0,0 값으로 생성</summary>
        public static InsertVertex Zero => new InsertVertex(0.0f, 0.0f);

        /// <summary>이동</summary>
        /// <param name="vertex"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static InsertVertex DoTransit(InsertVertex vertex, Vector2 delta)
        {
            vertex.transit.X += delta.X;
            vertex.transit.Y += delta.Y;
            return vertex;
        }

        /// <summary>지정된 회전 중심에서 회전</summary>
        /// <param name="vertex"></param>
        /// <param name="angle"></param>
        /// <param name="rotateCenter"></param>
        /// <returns></returns>
        public static InsertVertex Rotate(
          InsertVertex vertex,
          float angle,
          Vector2 rotateCenter)
        {
            Vector2 vector2 = Vector2.Transform(new Vector2(vertex.transit.X, vertex.transit.Y), Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            vertex.Transit = vector2;
            vertex.Angle += angle;
            return vertex;
        }
    }
}
