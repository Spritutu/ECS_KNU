
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
    /// <summary>원 엔티티</summary>
    public class Circle : IEntity, IMarkerable, IDrawable, ICloneable, IExplodable, IHatchable
    {
        private string name;
        private System.Drawing.Color color;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private float radius;
        private Vector2 center;
        private float startAngle;
        private bool isHatchable;
        private HatchMode hatchMode;
        private float hatchAngle;
        private float hatchAngle2;
        private float hatchInterval;
        private float hatchExclude;
        private Group hatch = new Group()
        {
            Name = "Hatch"
        };
        private LwPolyline lwPolyline = new LwPolyline();
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Circle;

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
                if (this.Owner != null && this.isLocked)
                    return;
                this.color = value;
                this.lwPolyline.Color2 = this.color;
                this.hatch.Color2 = this.color;
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
        [DisplayName("Radius")]
        [System.ComponentModel.Description("반지름 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Radius
        {
            get => this.radius;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.radius = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Center")]
        [System.ComponentModel.Description("Center location")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Center
        {
            get => this.center;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.center = value;
                this.isRegen = true;
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

        [JsonIgnore]
        [Browsable(false)]
        public float Angle { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch")]
        [System.ComponentModel.Description("Hatch 여부")]
        public bool IsHatchable
        {
            get => this.isHatchable;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isHatchable = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Mode")]
        [System.ComponentModel.Description("Hatch Mode")]
        public HatchMode HatchMode
        {
            get => this.hatchMode;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.hatchMode = value;
                if (!this.isHatchable)
                    return;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Angle")]
        [System.ComponentModel.Description("Hatch Angle")]
        public float HatchAngle
        {
            get => this.hatchAngle;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.hatchAngle = value;
                if (!this.isHatchable)
                    return;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Angle2")]
        [System.ComponentModel.Description("2nd Hatch Angle for Cross Hatch")]
        public float HatchAngle2
        {
            get => this.hatchAngle2;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.hatchAngle2 = value;
                if (!this.IsHatchable)
                    return;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Interval")]
        [System.ComponentModel.Description("Hatch Interval = Line Interval (mm)")]
        public float HatchInterval
        {
            get => this.hatchInterval;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.hatchInterval = value;
                if (!this.isHatchable)
                    return;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Exclude")]
        [System.ComponentModel.Description("Hatch Exclude = Gap Interval (mm)")]
        public float HatchExclude
        {
            get => this.hatchExclude;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.hatchExclude = value;
                if (!this.isHatchable)
                    return;
                this.isRegen = true;
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

        public override string ToString() => string.Format("{0} : {1:F3}", (object)this.Name, (object)this.Radius);

        public Circle()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Circle);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.Color2 = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.center = Vector2.Zero;
            this.hatchInterval = 0.5f;
            this.hatchAngle = 90f;
            this.hatchAngle2 = 0.0f;
            this.Repeat = 1U;
            this.isRegen = true;
        }

        public Circle(float radius)
          : this()
        {
            this.radius = radius;
        }

        public Circle(float x, float y, float radius)
          : this(radius)
        {
            this.center = new Vector2(x, y);
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Circle()
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            IsHighlighted = this.IsHighlighted,
            isVisible = this.isVisible,
            isMarkerable = this.isMarkerable,
            isLocked = this.IsLocked,
            color = this.Color2,
            isHatchable = this.isHatchable,
            hatchMode = this.hatchMode,
            hatchAngle = this.hatchAngle,
            hatchAngle2 = this.hatchAngle2,
            hatchInterval = this.hatchInterval,
            hatchExclude = this.hatchExclude,
            BoundRect = this.BoundRect.Clone(),
            Repeat = this.Repeat,
            center = this.center,
            radius = this.radius,
            Angle = this.Angle,
            Tag = this.Tag,
            Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            },
            lwPolyline = (this.lwPolyline.Clone() as LwPolyline),
            hatch = (this.hatch.Clone() as Group)
        };

        public List<IEntity> Explode()
        {
            List<IEntity> entityList = this.lwPolyline.Explode();
            if (this.isHatchable && this.hatch != null)
            {
                Group group = (Group)this.hatch.Clone();
                entityList.Add((IEntity)group);
            }
            return entityList;
        }

        public Group Hatch(
          HatchMode mode,
          float angle,
          float angle2,
          float interval,
          float exclude)
        {
            return this.lwPolyline.Hatch(mode, angle, angle2, interval, exclude);
        }

        public LwPolyline ToLwPolyline() => (LwPolyline)this.lwPolyline.Clone();

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            if ((double)this.Radius <= 0.0)
                return false;
            bool flag1 = true;
            IRtc rtc = markerArg.Rtc;
            double num1 = Math.Cos((double)this.startAngle * (Math.PI / 180.0)) * (double)this.Radius + (double)this.center.X;
            double num2 = Math.Sin((double)this.startAngle * (Math.PI / 180.0)) * (double)this.Radius + (double)this.center.Y;
            bool flag2 = flag1 & rtc.ListJump(new Vector2((float)num1, (float)num2));
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                flag2 &= rtc.ListArc(this.center, 360f);
                if (!flag2)
                    break;
            }
            if (this.isHatchable & flag2)
            {
                for (int index = 0; (long)index < (long)this.Repeat; ++index)
                {
                    flag2 &= this.hatch.Mark(markerArg);
                    if (!flag2)
                        break;
                }
            }
            return flag2;
        }

        private void RegenVertextList()
        {
            this.lwPolyline.Clear();
            this.lwPolyline.Color2 = this.color;
            for (double startAngle = (double)this.StartAngle; startAngle < (double)this.StartAngle + 360.0; startAngle += (double)Config.AngleFactor)
                this.lwPolyline.Add(new LwPolyLineVertex((float)Math.Cos(startAngle * (Math.PI / 180.0)) * this.Radius + this.center.X, (float)Math.Sin(startAngle * (Math.PI / 180.0)) * this.Radius + this.center.Y));
            this.lwPolyline.IsClosed = true;
            this.lwPolyline.Owner = (IEntity)this;
            this.lwPolyline.Regen();
        }

        private void RegenBoundRect()
        {
            float left = this.center.X - this.Radius;
            float right = this.center.X + this.Radius;
            float top = this.center.Y + this.Radius;
            float bottom = this.center.Y - this.Radius;
            this.BoundRect = new BoundRect(left, top, right, bottom);
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            if (this.IsHatchable)
                this.hatch = this.Hatch(this.hatchMode, this.hatchAngle, this.hatchAngle2, this.hatchInterval, this.hatchExclude);
            else
                this.hatch.Clear();
            this.isRegen = false;
        }

        public bool Draw(IView view)
        {
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            OpenGL renderer = view.Renderer;
            this.lwPolyline.IsSelected = this.IsSelected;
            this.lwPolyline.IsDrawPath = this.IsDrawPath;
            this.lwPolyline.Color2 = this.color;
            this.lwPolyline.Draw(view);
            if (this.isHatchable && this.hatch != null)
            {
                this.hatch.IsSelected = this.IsSelected;
                this.hatch.IsDrawPath = this.IsDrawPath;
                this.hatch.Color2 = this.color;
                this.hatch.Draw(view);
            }
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            this.center = Vector2.Add(this.center, delta);
            this.lwPolyline?.Transit(delta);
            this.hatch.Transit(delta);
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.hatch.Rotate(angle, this.center);
            this.startAngle += angle;
            this.startAngle = MathHelper.NormalizeAngle(this.startAngle);
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.center = Vector2.Transform(this.center, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.lwPolyline?.Rotate(angle, rotateCenter);
            this.hatch.Rotate(angle, rotateCenter);
            this.startAngle += angle;
            this.startAngle = MathHelper.NormalizeAngle(this.startAngle);
            this.Regen();
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.radius *= scale.X;
            this.Regen();
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.center = (this.center - scaleCenter) * scale + scaleCenter;
            this.radius *= scale.X;
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
