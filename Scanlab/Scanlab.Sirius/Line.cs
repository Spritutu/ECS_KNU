
using Newtonsoft.Json;
using SharpGL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>선분 엔티티</summary>
    public class Line : IEntity, IMarkerable, IDrawable, ICloneable
    {
        private string name;
        private System.Drawing.Color color;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private bool isReversable;
        private Vector2 start;
        private float startRampFactor;
        private Vector2 end;
        private float endRampFactor;
        private float angle;
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Line;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [System.ComponentModel.Description("엔티티의 이름")]
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Description")]
        [System.ComponentModel.Description("엔티티에 대한 설명")]
        public string Description { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public AciColor Color { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Color")]
        [System.ComponentModel.Description("색상")]
        public System.Drawing.Color Color2
        {
            get => this.color;
            set => this.color = value;
        }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Bound")]
        [System.ComponentModel.Description("외각 영역")]
        public BoundRect BoundRect { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Selected")]
        [System.ComponentModel.Description("선택여부")]
        public bool IsSelected { get; set; }

        [Browsable(false)]
        public bool IsHighlighted { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Visible")]
        [System.ComponentModel.Description("스크린에 출력 여부")]
        public bool IsVisible
        {
            get => this.isVisible;
            set => this.isVisible = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Markerable")]
        [System.ComponentModel.Description("레이저 가공 여부")]
        public bool IsMarkerable
        {
            get => this.isMarkerable;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isMarkerable = value;
                if (this.Node.NodeFont == null)
                    this.Node.NodeFont = new Font(Config.NodeFont, (float)Config.NodeFontSize);
                if (this.isMarkerable)
                    this.Node.NodeFont = new Font(Config.NodeFont, (float)Config.NodeFontSize, this.Node.NodeFont.Style & ~FontStyle.Strikeout);
                else
                    this.Node.NodeFont = new Font(Config.NodeFont, (float)Config.NodeFontSize, this.Node.NodeFont.Style | FontStyle.Strikeout);
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Mark Path")]
        [System.ComponentModel.Description("가공 경로를 표시")]
        public bool IsDrawPath { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Locked")]
        [System.ComponentModel.Description("편집 금지 여부")]
        public bool IsLocked
        {
            get => this.isLocked;
            set => this.isLocked = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Repeat")]
        [System.ComponentModel.Description("가공 반복 횟수")]
        public uint Repeat { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Repeat With Reverse")]
        [System.ComponentModel.Description("반복 가공시 역방향으로 전환 가공 기능 활성화 여부")]
        public bool IsReversable
        {
            get => this.isReversable;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isReversable = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Start")]
        [System.ComponentModel.Description("시작점 좌표값")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Start
        {
            get => this.start;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.start = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Start Ramp")]
        [System.ComponentModel.Description("시작점 Ramp 비율값")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float StartRampFactor
        {
            get => this.startRampFactor;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.startRampFactor = value;
            }
        }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Length")]
        [System.ComponentModel.Description("두점 사이의 거리값")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Length
        {
            get => Vector2.Distance(this.start, this.end);
            set
            {
                if (this.Owner != null && this.isLocked || (double)value == 0.0)
                    return;
                this.end = Vector2.Normalize(this.end - this.start) * value + this.start;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("End")]
        [System.ComponentModel.Description("끝점 좌표값")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 End
        {
            get => this.end;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.end = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("End Ramp")]
        [System.ComponentModel.Description("끝점 Ramp 비율값")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float EndRampFactor
        {
            get => this.endRampFactor;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.endRampFactor = value;
            }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Angle")]
        [System.ComponentModel.Description("회전 각도")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Angle
        {
            get => this.angle;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                float angle = value - this.angle;
                if (this.Owner != null)
                    this.Rotate(angle);
                this.angle = value;
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public TreeNode Node { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public int Index { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public object Tag { get; set; }

        public override string ToString() => this.Name + ": " + this.start.ToString() + ", " + this.end.ToString();

        /// <summary>생성자</summary>
        public Line()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Line);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.color = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.Start = Vector2.Zero;
            this.End = new Vector2(1f, 1f);
            this.isRegen = true;
            this.Repeat = 1U;
            this.isReversable = false;
            this.startRampFactor = 1f;
            this.endRampFactor = 1f;
        }

        /// <summary>생성자</summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public Line(float startX, float startY, float endX, float endY)
          : this()
        {
            this.Start = new Vector2(startX, startY);
            this.End = new Vector2(endX, endY);
        }

        /// <summary>생성자</summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="rampStart"></param>
        /// <param name="rampEnd"></param>
        public Line(
          float startX,
          float startY,
          float endX,
          float endY,
          float rampStart = 1f,
          float rampEnd = 1f)
          : this(startX, startY, endX, endY)
        {
            this.startRampFactor = rampStart;
            this.endRampFactor = rampEnd;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Line()
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            IsHighlighted = this.IsHighlighted,
            isVisible = this.IsVisible,
            isMarkerable = this.IsMarkerable,
            isLocked = this.IsLocked,
            Repeat = this.Repeat,
            isReversable = this.IsReversable,
            color = this.Color2,
            BoundRect = this.BoundRect.Clone(),
            start = this.start,
            startRampFactor = this.StartRampFactor,
            end = this.end,
            endRampFactor = this.EndRampFactor,
            angle = this.angle,
            Tag = this.Tag,
            Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            }
        };

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            bool flag = true;
            IRtc rtc = markerArg.Rtc;
            if (!this.IsReversable)
            {
                for (int index = 0; (long)index < (long)this.Repeat; ++index)
                {
                    flag = flag & rtc.ListJump(this.Start, this.StartRampFactor) & rtc.ListMark(this.End, this.EndRampFactor);
                    if (!flag)
                        break;
                }
            }
            else
            {
                for (int index = 0; (long)index < (long)this.Repeat; ++index)
                {
                    flag = index % 2 != 0 ? flag & rtc.ListJump(this.End, this.StartRampFactor) & rtc.ListMark(this.Start, this.EndRampFactor) : flag & rtc.ListJump(this.Start, this.StartRampFactor) & rtc.ListMark(this.End, this.EndRampFactor);
                    if (!flag)
                        break;
                }
            }
            return flag;
        }

        private void RegenVertextList()
        {
        }

        private void RegenBoundRect()
        {
            float left = (double)this.Start.X < (double)this.End.X ? this.Start.X : this.End.X;
            float right = (double)this.Start.X < (double)this.End.X ? this.End.X : this.Start.X;
            float top = (double)this.Start.Y > (double)this.End.Y ? this.Start.Y : this.End.Y;
            float bottom = (double)this.Start.Y > (double)this.End.Y ? this.End.Y : this.Start.Y;
            this.BoundRect = new BoundRect(left, top, right, bottom);
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            this.isRegen = false;
        }

        /// <summary>선분 벡터의 각도 (degree : 0~360)</summary>
        internal float LineVectorAngle
        {
            get
            {
                Vector2 vector2 = this.End - this.Start;
                return MathHelper.NormalizeAngle((float)Math.Atan2((double)vector2.Y, (double)vector2.X) * 57.29578f);
            }
        }

        public bool Draw(IView view)
        {
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            OpenGL renderer = view.Renderer;
            if (this.IsSelected && this.IsDrawPath)
            {
                float size = view.Dp2Lp(10);
                (view as ViewDefault).DrawLaserMarkTo(this.End.X, this.End.Y, this.LineVectorAngle, size);
            }
            if (this.IsSelected)
            {
                renderer.Begin(1U);
                renderer.Color(Config.EntitySelectedColor[0] * this.StartRampFactor, Config.EntitySelectedColor[1] * this.StartRampFactor, Config.EntitySelectedColor[2] * this.StartRampFactor);
                renderer.Vertex(this.Start.X, this.Start.Y);
                renderer.Color(Config.EntitySelectedColor[0] * this.EndRampFactor, Config.EntitySelectedColor[1] * this.EndRampFactor, Config.EntitySelectedColor[2] * this.EndRampFactor);
                renderer.Vertex(this.End.X, this.End.Y);
                renderer.End();
            }
            else
            {
                renderer.Begin(1U);
                OpenGL openGl1 = renderer;
                System.Drawing.Color color2 = this.Color2;
                int num1 = (int)(byte)((double)color2.R * (double)this.StartRampFactor);
                color2 = this.Color2;
                int num2 = (int)(byte)((double)color2.G * (double)this.StartRampFactor);
                color2 = this.Color2;
                int num3 = (int)(byte)((double)color2.B * (double)this.StartRampFactor);
                openGl1.Color((byte)num1, (byte)num2, (byte)num3);
                renderer.Vertex(this.Start.X, this.Start.Y);
                OpenGL openGl2 = renderer;
                color2 = this.Color2;
                int num4 = (int)(byte)((double)color2.R * (double)this.EndRampFactor);
                color2 = this.Color2;
                int num5 = (int)(byte)((double)color2.G * (double)this.EndRampFactor);
                color2 = this.Color2;
                int num6 = (int)(byte)((double)color2.B * (double)this.EndRampFactor);
                openGl2.Color((byte)num4, (byte)num5, (byte)num6);
                renderer.Vertex(this.End.X, this.End.Y);
                renderer.End();
            }
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            this.start = Vector2.Add(this.start, delta);
            this.end = Vector2.Add(this.end, delta);
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.start = Vector2.Transform(this.start, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), this.BoundRect.Center));
            this.end = Vector2.Transform(this.end, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), this.BoundRect.Center));
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.start = Vector2.Transform(this.start, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.end = Vector2.Transform(this.end, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            Vector2 vector2 = (this.start + this.end) * 0.5f;
            this.start = (this.start - vector2) * scale + vector2;
            this.end = (this.end - vector2) * scale + vector2;
            this.Regen();
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.start = (this.start - scaleCenter) * scale + scaleCenter;
            this.end = (this.end - scaleCenter) * scale + scaleCenter;
            this.Regen();
        }

        public bool HitTest(float x, float y, float threshold) => this.IsVisible && this.BoundRect.HitTest(x, y, threshold) && MathHelper.IntersectPointInLine((double)this.Start.X, (double)this.Start.Y, (double)this.End.X, (double)this.End.Y, (double)x, (double)y, (double)threshold);

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold) => this.IsVisible && this.BoundRect.HitTest(br, threshold) && MathHelper.IntersectLineInRect(br, (double)this.Start.X, (double)this.Start.Y, (double)this.End.X, (double)this.End.Y);
    }
}
