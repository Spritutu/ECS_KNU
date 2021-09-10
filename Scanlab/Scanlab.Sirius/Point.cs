
using Newtonsoft.Json;
using SharpGL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>점 엔티티</summary>
    public class Point : IEntity, IMarkerable, IDrawable, ICloneable
    {
        private string name;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private Vector2 location;
        private float dwellTime;
        private float size;
        private float angle;
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Point;

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
        public System.Drawing.Color Color2 { get; set; } = Config.DefaultColor;

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

        [JsonIgnore]
        [Browsable(false)]
        public TreeNode Node { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public int Index { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public object Tag { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Location")]
        [System.ComponentModel.Description("위치 (mm)")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Location
        {
            get => this.location;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.location = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Dwell")]
        [DisplayName("Time")]
        [System.ComponentModel.Description("가공시간 (msec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float DwellTime
        {
            get => this.dwellTime;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.dwellTime = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Size")]
        [System.ComponentModel.Description("화면에 표시되는 점의 크기 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Size
        {
            get => this.size;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.size = value;
                this.isRegen = true;
            }
        }

        [Browsable(false)]
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

        public override string ToString() => string.Format("{0}: {1} {2}", (object)this.Name, (object)this.location.X, (object)this.location.Y);

        public Point()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Point);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.Color2 = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.location = Vector2.Zero;
            this.dwellTime = 0.05f;
            this.isRegen = true;
            this.Repeat = 1U;
        }

        public Point(float x, float y)
          : this()
        {
            this.location.X = x;
            this.location.Y = y;
        }

        public Point(Vector2 v)
          : this()
        {
            this.isRegen = false;
            this.location = v;
        }

        public Point(Offset v)
          : this()
        {
            this.isRegen = false;
            this.location = v.ToVector2;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Point()
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
            Color2 = this.Color2,
            location = this.location,
            dwellTime = this.dwellTime,
            size = this.size,
            angle = this.angle,
            BoundRect = this.BoundRect.Clone(),
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
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                flag &= rtc.ListJump(this.Location);
                rtc.ListLaserOn(this.DwellTime);
                if (!flag)
                    break;
            }
            return flag;
        }

        private void RegenVertextList()
        {
        }

        private void RegenBoundRect() => this.BoundRect = new BoundRect(this.Location.X, this.Location.Y, this.Location.X, this.Location.Y);

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            this.isRegen = false;
        }

        public bool Draw(IView view)
        {
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            OpenGL renderer = view.Renderer;
            renderer.PointSize(this.Size);
            if (this.IsSelected)
            {
                renderer.Color(Config.EntitySelectedColor);
            }
            else
            {
                OpenGL openGl = renderer;
                System.Drawing.Color color2 = this.Color2;
                int r = (int)color2.R;
                color2 = this.Color2;
                int g = (int)color2.G;
                color2 = this.Color2;
                int b = (int)color2.B;
                openGl.Color((byte)r, (byte)g, (byte)b);
            }
            renderer.Begin(0U);
            renderer.Vertex(this.Location.X, this.Location.Y);
            renderer.End();
            renderer.PointSize(0.0f);
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            this.Location += delta;
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.location = Vector2.Transform(this.location, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.BoundRect = new BoundRect(this.Location.X, this.Location.Y, this.Location.X, this.Location.Y);
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero)
                return;
            int num = scale == Vector2.One ? 1 : 0;
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.location = (this.location - scaleCenter) * scale + scaleCenter;
            this.BoundRect = new BoundRect(this.Location.X, this.Location.Y, this.Location.X, this.Location.Y);
        }

        public bool HitTest(float x, float y, float threshold) => this.IsVisible && this.BoundRect.HitTest(x, y, threshold) && MathHelper.IntersectPointInCircle((double)this.Location.X, (double)this.Location.Y, (double)x, (double)y, (double)threshold);

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold) => this.IsVisible && this.BoundRect.HitTest(br, threshold) && MathHelper.IntersectPointInRect(br, (double)this.Location.X, (double)this.Location.Y, (double)threshold);
    }
}
