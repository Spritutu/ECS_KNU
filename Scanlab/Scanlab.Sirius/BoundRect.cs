
using SharpGL;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>사각형 객체 (엔티티의 외곽 영역 정보 처리)</summary>
    public class BoundRect : IEquatable<BoundRect>, IMarkerable
    {
        protected float left;
        protected float right;
        protected float top;
        protected float bottom;
        protected float width;
        protected float height;

        /// <summary>크기 데이타를 모두 삭제</summary>
        public static BoundRect Empty => new BoundRect(0.0f, 0.0f, 0.0f, 0.0f)
        {
            IsEmpty = true
        };

        /// <summary>지정된 데이타가 있는지 여부</summary>
        [Browsable(false)]
        public virtual bool IsEmpty { get; private set; }

        /// <summary>왼쪽 X 좌표값</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Left")]
        [Description("왼쪽 위치")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Left
        {
            get => this.left;
            set
            {
                this.left = value;
                this.IsEmpty = false;
            }
        }

        /// <summary>오른쪽 X 좌표값</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Right")]
        [Description("오른쪽 위치")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Right
        {
            get => this.right;
            set
            {
                this.right = value;
                this.IsEmpty = false;
            }
        }

        /// <summary>상단 Y 좌표값</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Top")]
        [Description("위쪽 위치")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Top
        {
            get => this.top;
            set
            {
                this.top = value;
                this.IsEmpty = false;
            }
        }

        /// <summary>하단 Y 좌표값</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Bottom")]
        [Description("아래쪽 위치")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Bottom
        {
            get => this.bottom;
            set
            {
                this.bottom = value;
                this.IsEmpty = false;
            }
        }

        /// <summary>폭</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Width")]
        [Description("폭")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Width
        {
            get
            {
                this.width = Math.Abs(this.Right - this.Left);
                return this.width;
            }
            set
            {
                if (0.0 >= (double)value)
                    return;
                float num = value - this.width;
                this.left -= num / 2f;
                this.right += num / 2f;
                this.width = value;
                this.IsEmpty = false;
            }
        }

        /// <summary>높이</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Height")]
        [Description("높이")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Height
        {
            get
            {
                this.height = Math.Abs(this.Top - this.Bottom);
                return this.height;
            }
            set
            {
                if (0.0 >= (double)value)
                    return;
                float num = value - this.height;
                this.top += num / 2f;
                this.bottom -= num / 2f;
                this.height = value;
                this.IsEmpty = false;
            }
        }

        /// <summary>중심 X,Y 위치</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Center")]
        [Description("중심 위치")]
        [TypeConverter(typeof(Vector2Converter))]
        public virtual Vector2 Center
        {
            get => new Vector2((float)(((double)this.Left + (double)this.Right) / 2.0), (float)(((double)this.Top + (double)this.Bottom) / 2.0));
            set
            {
                Vector2 vector2 = value - this.Center;
                this.Left += vector2.X;
                this.Right += vector2.X;
                this.Top += vector2.Y;
                this.Bottom += vector2.Y;
            }
        }

        /// <summary>레이저 가공 유무</summary>
        [Browsable(false)]
        public virtual bool IsMarkerable { get; set; }

        /// <summary>반복 가공 회수 (기본값 1)</summary>
        [Browsable(false)]
        public virtual uint Repeat { get; set; }

        /// <summary>문자열</summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.IsEmpty)
                return "(Empty)";
            return string.Format("{0:F3},{1:F3} {2:F3},{3:F3}", (object)this.Left, (object)this.Top, (object)this.Right, (object)this.Bottom);
        }

        /// <summary>생성자</summary>
        public BoundRect()
        {
            this.IsEmpty = true;
            this.IsMarkerable = true;
            this.Repeat = 1U;
        }

        /// <summary>생성자</summary>
        /// <param name="left">좌</param>
        /// <param name="top">상단</param>
        /// <param name="right">우</param>
        /// <param name="bottom">하단</param>
        public BoundRect(float left, float top, float right, float bottom)
          : this()
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.IsEmpty = false;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public virtual BoundRect Clone() => new BoundRect()
        {
            Left = this.Left,
            Top = this.Top,
            Right = this.Right,
            Bottom = this.Bottom,
            IsEmpty = this.IsEmpty
        };

        /// <summary>동일성 검사</summary>
        /// <param name="other">Another color to compare to.</param>
        /// <returns>True if the three components are equal or false in any other case.</returns>
        public virtual bool Equals(BoundRect other) => other != null && MathHelper.IsEqual(other.Left, this.Left) && (MathHelper.IsEqual(other.Top, this.Top) && MathHelper.IsEqual(other.Right, this.Right)) && MathHelper.IsEqual(other.Bottom, this.Bottom) && other.IsEmpty == this.IsEmpty;

        /// <summary>내부 데이타 리셋 (모두 제거)</summary>
        public virtual void Clear()
        {
            this.Left = this.Top = this.Right = this.Bottom = 0.0f;
            this.IsEmpty = true;
        }

        /// <summary>정렬에 따른 위치 얻기</summary>
        /// <param name="align">정렬 기준</param>
        /// <returns></returns>
        public virtual Vector2 LocationByAlign(Alignment align)
        {
            if (this.IsEmpty)
                return Vector2.Zero;
            switch (align)
            {
                case Alignment.LeftTop:
                    return new Vector2(this.Left, this.Top);
                case Alignment.MiddleTop:
                    return new Vector2(this.Center.X, this.Top);
                case Alignment.RightTop:
                    return new Vector2(this.Right, this.Top);
                case Alignment.LeftMiddle:
                    return new Vector2(this.Left, this.Center.Y);
                case Alignment.Center:
                    return this.Center;
                case Alignment.RightMiddle:
                    return new Vector2(this.Right, this.Center.Y);
                case Alignment.LeftBottom:
                    return new Vector2(this.Left, this.Bottom);
                case Alignment.MiddleBottom:
                    return new Vector2(this.Center.X, this.Bottom);
                case Alignment.RightBottom:
                    return new Vector2(this.Right, this.Bottom);
                default:
                    throw new InvalidOperationException("invalid alignment value !");
            }
        }

        /// <summary>전달된 boundrect 와의 영역 합치기</summary>
        /// <param name="br"></param>
        public virtual void Union(BoundRect br)
        {
            if (br.IsEmpty)
                return;
            if (this.IsEmpty)
            {
                this.Left = br.Left;
                this.Top = br.Top;
                this.Right = br.Right;
                this.Bottom = br.Bottom;
            }
            else
            {
                if ((double)this.Left > (double)br.Left)
                    this.Left = br.Left;
                if ((double)this.Top < (double)br.Top)
                    this.Top = br.Top;
                if ((double)this.Right < (double)br.Right)
                    this.Right = br.Right;
                if ((double)this.Bottom > (double)br.Bottom)
                    this.Bottom = br.Bottom;
            }
            this.IsEmpty = false;
        }

        /// <summary>전달된 점 위치와 영역 합치기</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void Union(float x, float y)
        {
            if (this.IsEmpty)
            {
                this.Left = x;
                this.Top = y;
                this.Right = x;
                this.Bottom = y;
            }
            else
            {
                if ((double)this.Left > (double)x)
                    this.Left = x;
                if ((double)this.Top < (double)y)
                    this.Top = y;
                if ((double)this.Right < (double)x)
                    this.Right = x;
                if ((double)this.Bottom > (double)y)
                    this.Bottom = y;
            }
            this.IsEmpty = false;
        }

        /// <summary>전달된 벡터 위치와 영역 합치기</summary>
        /// <param name="v"></param>
        public virtual void Union(Vector2 v) => this.Union(v.X, v.Y);

        /// <summary>화면에 그리기</summary>
        /// <param name="gl">Openg 렌더링 객체</param>
        public virtual void Draw(OpenGL gl)
        {
            if (this.IsEmpty)
                return;
            gl.PushMatrix();
            gl.Begin(2U);
            gl.Vertex(this.Left, this.Top, 0.0f);
            gl.Vertex(this.Right, this.Top, 0.0f);
            gl.Vertex(this.Right, this.Bottom, 0.0f);
            gl.Vertex(this.Left, this.Bottom, 0.0f);
            gl.End();
            gl.PopMatrix();
        }

        /// <summary>지정된 Rtc 인터페이스와 Laser 인터페이스를 이용한 엔티티 가공</summary>
        /// <param name="markerArg">IMarkerArg 인터페이스</param>
        /// <returns></returns>
        public virtual bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            if ((double)this.Width <= 0.0 || (double)this.Height <= 0.0)
                return false;
            bool flag = true;
            IRtc rtc = markerArg.Rtc;
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                flag = flag & rtc.ListJump(this.left, this.top) & rtc.ListMark(this.left, this.bottom) & rtc.ListMark(this.right, this.bottom) & rtc.ListMark(this.right, this.top) & rtc.ListMark(this.left, this.top);
                if (!flag)
                    break;
            }
            return flag;
        }

        /// <summary>지정된 양 만큼 이동</summary>
        /// <param name="delta"></param>
        public virtual void Transit(Vector2 delta)
        {
            if (this.IsEmpty)
                return;
            this.Left += delta.X;
            this.Right += delta.X;
            this.Top += delta.Y;
            this.Bottom += delta.Y;
        }

        /// <summary>지정된 좌표가 포함되는지 여부 (충돌)</summary>
        /// <param name="x">X 위치 (mm)</param>
        /// <param name="y">Y 위치 (mm)</param>
        /// <param name="threshold">문턱값 (mm)</param>
        /// <returns></returns>
        public virtual bool HitTest(float x, float y, float threshold) => !this.IsEmpty && MathHelper.IntersectPointInRect(this, (double)x, (double)y, (double)threshold);

        /// <summary>지정된 사각 영역이 포함되는지 여부 (충돌)</summary>
        /// <param name="left">좌</param>
        /// <param name="top">상단</param>
        /// <param name="right">우</param>
        /// <param name="bottom">하단</param>
        /// <param name="threshold">문턱값(mm)</param>
        /// <returns></returns>
        public virtual bool HitTest(
          float left,
          float top,
          float right,
          float bottom,
          float threshold)
        {
            return !this.IsEmpty && this.HitTest(new BoundRect(left, top, right, bottom), threshold);
        }

        /// <summary>지정된 boundrect 영역이 포함되는지 여부(충돌)</summary>
        /// <param name="br">영역</param>
        /// <param name="threshold">문턱값(mm)</param>
        /// <returns></returns>
        public virtual bool HitTest(BoundRect br, float threshold) => !this.IsEmpty && (MathHelper.IntersectRectInRect(this, br, (double)threshold) || MathHelper.CollisionRectWithRect(this, br));
    }
}
