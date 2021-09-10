
using Newtonsoft.Json;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>타원 (ellipse) 엔티티 객체</summary>
    public class Ellipse : IEntity, IMarkerable, IDrawable, ICloneable, IExplodable
    {
        private string name;
        private System.Drawing.Color color;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private Alignment align;
        private float major;
        private float minor;
        private Vector2 center;
        private float startAngle;
        private float sweepAngle;
        private float angle;
        [JsonIgnore]
        private LwPolyline lwPolyline = new LwPolyline();
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Ellipse;

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
            set
            {
                this.color = value;
                if (this.lwPolyline != null)
                    this.lwPolyline.Color2 = this.color;
                this.isRegen = true;
            }
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
        [DisplayName("Align")]
        [System.ComponentModel.Description("정렬 기준위치")]
        public Alignment Align
        {
            get => this.align;
            private set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.align = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Major")]
        [System.ComponentModel.Description("장축 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Major
        {
            get => this.major;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.major = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Minor")]
        [System.ComponentModel.Description("단축 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Minor
        {
            get => this.minor;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.minor = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Center")]
        [System.ComponentModel.Description("중심 위치 (mm)")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Center
        {
            get => this.center;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                if (this.Owner != null)
                    this.Transit(value - this.center);
                this.center = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Start Angle")]
        [System.ComponentModel.Description("시작 각(도)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float StartAngle
        {
            get => this.startAngle;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.startAngle = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Sweep Angle")]
        [System.ComponentModel.Description("각도 변화량(도) : + 값은 반시계 방향")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float SweepAngle
        {
            get => this.sweepAngle;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.sweepAngle = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
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

        public override string ToString() => string.Format("{0}: {1:F3}, {2:F3}", (object)this.Name, (object)this.center.X, (object)this.center.Y);

        public Ellipse()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Ellipse);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.color = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.center = Vector2.Zero;
            this.align = Alignment.Center;
            this.major = 2f;
            this.minor = 1.5f;
            this.startAngle = 0.0f;
            this.sweepAngle = 360f;
            this.Repeat = 1U;
            this.isRegen = true;
        }

        public Ellipse(
          float cx,
          float cy,
          float major,
          float minor,
          float startAngle,
          float sweepAngle,
          float rotateAngle)
          : this()
        {
            this.center = new Vector2(cx, cy);
            this.major = major;
            this.minor = minor;
            this.startAngle = startAngle;
            this.sweepAngle = sweepAngle;
            this.Angle = rotateAngle;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Ellipse()
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
            color = this.Color2,
            BoundRect = this.BoundRect.Clone(),
            align = this.align,
            major = this.Major,
            minor = this.Minor,
            center = this.Center,
            startAngle = this.startAngle,
            sweepAngle = this.sweepAngle,
            Angle = this.Angle,
            Tag = this.Tag,
            Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            },
            lwPolyline = (this.lwPolyline.Clone() as LwPolyline)
        };

        public List<IEntity> Explode() => this.lwPolyline.Explode();

        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            if ((double)this.Major <= 0.0 || (double)this.Minor <= 0.0)
                return false;
            bool flag = true;
            IRtc rtc = markerArg.Rtc;
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                flag &= rtc.ListEllipse(new Vector2(this.center.X, this.center.Y), this.Major / 2f, this.Minor / 2f, this.StartAngle, this.SweepAngle, this.Angle);
                if (!flag)
                    break;
            }
            return flag;
        }

        private void RegenVertextList()
        {
            this.lwPolyline.Clear();
            this.lwPolyline.Angle = 0.0f;
            this.lwPolyline.Align = Alignment.Center;
            this.lwPolyline.Location = Vector2.Zero;
            double num1 = Math.Cos((double)this.StartAngle * (Math.PI / 180.0)) * (double)this.Major / 2.0;
            double num2 = Math.Sin((double)this.StartAngle * (Math.PI / 180.0)) * (double)this.Minor / 2.0;
            if ((double)this.SweepAngle > 0.0)
            {
                for (double startAngle = (double)this.StartAngle; startAngle < (double)this.StartAngle + (double)this.SweepAngle; startAngle += (double)Config.AngleFactor)
                    this.lwPolyline.Add(new LwPolyLineVertex((float)(Math.Cos(startAngle * (Math.PI / 180.0)) * (double)this.Major / 2.0), (float)(Math.Sin(startAngle * (Math.PI / 180.0)) * (double)this.Minor / 2.0)));
            }
            else
            {
                for (double startAngle = (double)this.StartAngle; startAngle > (double)this.StartAngle + (double)this.SweepAngle; startAngle -= (double)Config.AngleFactor)
                    this.lwPolyline.Add(new LwPolyLineVertex((float)(Math.Cos(startAngle * (Math.PI / 180.0)) * (double)this.Major / 2.0), (float)(Math.Sin(startAngle * (Math.PI / 180.0)) * (double)this.Minor / 2.0)));
            }
            this.lwPolyline.Add(new LwPolyLineVertex((float)(Math.Cos(((double)this.StartAngle + (double)this.SweepAngle) * (Math.PI / 180.0)) * (double)this.Major / 2.0), (float)(Math.Sin(((double)this.StartAngle + (double)this.SweepAngle) * (Math.PI / 180.0)) * (double)this.Minor / 2.0)));
            this.lwPolyline.Rotate(this.Angle);
            this.lwPolyline.Transit(this.Center);
            this.lwPolyline.Owner = (IEntity)this;
            this.lwPolyline.Regen();
        }

        private void RegenBoundRect()
        {
            this.BoundRect = this.lwPolyline.BoundRect.Clone();
            this.BoundRect.Union(this.center);
        }

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
            if (this.IsSelected && this.IsDrawPath)
            {
                renderer.Color(Config.EntityCenterColor);
                renderer.Begin(0U);
                renderer.Vertex(this.Center.X, this.Center.Y);
                renderer.End();
            }
            this.lwPolyline.IsSelected = this.IsSelected;
            this.lwPolyline.IsDrawPath = this.IsDrawPath;
            this.lwPolyline.Color2 = this.color;
            this.lwPolyline.Draw(view);
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            this.center = Vector2.Add(this.Center, delta);
            this.lwPolyline.Transit(delta);
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.center = Vector2.Transform(this.center, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.Major *= scale.X;
            this.Minor *= scale.Y;
            this.Regen();
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.center = (this.center - scaleCenter) * scale + scaleCenter;
            this.Major *= scale.X;
            this.Minor *= scale.Y;
            this.Regen();
        }

        public bool HitTest(float x, float y, float threshold)
        {
            if (!this.IsVisible || !this.BoundRect.HitTest(x, y, threshold))
                return false;
            int num = -1;
            if (this.lwPolyline.HitTest(x, y, threshold))
                num = 0;
            return num >= 0;
        }

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold)
        {
            if (!this.IsVisible || !this.BoundRect.HitTest(br, threshold))
                return false;
            int num = -1;
            if (this.lwPolyline.HitTest(br, threshold))
                num = 0;
            return num >= 0;
        }
    }
}
