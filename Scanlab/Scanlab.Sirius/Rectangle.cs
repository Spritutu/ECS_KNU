// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Rectangle
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

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
    /// <summary>사각형 엔티티</summary>
    public class Rectangle : IEntity, IMarkerable, IDrawable, ICloneable, IExplodable, IHatchable
    {
        private string name;
        private System.Drawing.Color color;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private float width;
        private float height;
        private Alignment align;
        private Vector2 location;
        private float angle;
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
        private bool isRegen;
        [JsonIgnore]
        private LwPolyline lwPolyline = new LwPolyline();

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Rectangle;

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
        [DisplayName("Repeat")]
        [System.ComponentModel.Description("가공 반복 횟수")]
        public uint Repeat { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Width")]
        [System.ComponentModel.Description("폭 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Width
        {
            get => this.width;
            set
            {
                if (this.Owner != null && this.isLocked || (double)value <= 0.0)
                    return;
                this.isRegen = true;
                if (0.0 == (double)this.width)
                {
                    this.width = value;
                    this.isRegen = true;
                }
                else
                {
                    this.Scale(new Vector2(value / this.width, 1f));
                    this.width = value;
                }
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Height")]
        [System.ComponentModel.Description("높이 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Height
        {
            get => this.height;
            set
            {
                if (this.Owner != null && this.isLocked || (double)value <= 0.0)
                    return;
                if (0.0 == (double)this.height)
                {
                    this.height = value;
                    this.isRegen = true;
                }
                else
                {
                    this.Scale(new Vector2(1f, value / this.height));
                    this.height = value;
                    this.isRegen = true;
                }
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Center")]
        [System.ComponentModel.Description("중심위치 (mm)")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Center => this.BoundRect.Center;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Align")]
        [System.ComponentModel.Description("정렬 기준위치")]
        public Alignment Align
        {
            get => this.align;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                if (this.Owner != null && this.align != value)
                    this.location = this.BoundRect.LocationByAlign(value);
                this.align = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Location")]
        [System.ComponentModel.Description("기준 위치")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Location
        {
            get => this.location;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.Transit(value - this.location);
                this.location = value;
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
                if (!this.isHatchable)
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

        public override string ToString() => this.Name ?? "";

        public Rectangle()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Rectangle);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.color = Config.DefaultColor;
            this.align = Alignment.Center;
            this.location = Vector2.Zero;
            this.BoundRect = BoundRect.Empty;
            this.Repeat = 1U;
            this.hatchInterval = 0.5f;
            this.hatchAngle = 90f;
            this.HatchAngle2 = 0.0f;
            this.isRegen = true;
        }

        public Rectangle(float cx, float cy, float width, float height)
          : this()
        {
            this.width = width;
            this.height = height;
            this.location = new Vector2(cx, cy);
            this.lwPolyline.Color2 = this.color;
            this.lwPolyline.Add(new LwPolyLineVertex((float)(-(double)this.width / 2.0), this.height / 2f));
            this.lwPolyline.Add(new LwPolyLineVertex((float)(-(double)this.width / 2.0), (float)(-(double)this.height / 2.0)));
            this.lwPolyline.Add(new LwPolyLineVertex(this.width / 2f, (float)(-(double)this.height / 2.0)));
            this.lwPolyline.Add(new LwPolyLineVertex(this.width / 2f, this.height / 2f));
            this.lwPolyline.IsClosed = true;
            this.lwPolyline.Regen();
            this.lwPolyline.Transit(new Vector2(cx, cy));
            this.isRegen = true;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Rectangle()
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            IsHighlighted = this.IsHighlighted,
            isVisible = this.IsVisible,
            isMarkerable = this.IsMarkerable,
            isLocked = this.IsLocked,
            color = this.Color2,
            isHatchable = this.isHatchable,
            hatchMode = this.hatchMode,
            hatchAngle = this.hatchAngle,
            hatchAngle2 = this.hatchAngle2,
            hatchInterval = this.hatchInterval,
            hatchExclude = this.hatchExclude,
            Repeat = this.Repeat,
            BoundRect = this.BoundRect.Clone(),
            width = this.width,
            height = this.height,
            align = this.align,
            location = this.location,
            angle = this.angle,
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

        /// <summary>markerable</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            if ((double)this.Width <= 0.0 || (double)this.Height <= 0.0)
                return false;
            bool flag = true;
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                flag &= this.lwPolyline.Mark(markerArg);
                if (!flag)
                    break;
            }
            if (this.isHatchable & flag)
            {
                for (int index = 0; (long)index < (long)this.Repeat; ++index)
                {
                    flag &= this.hatch.Mark(markerArg);
                    if (!flag)
                        break;
                }
            }
            return flag;
        }

        private void RegenVertextList()
        {
            this.lwPolyline = new LwPolyline();
            this.lwPolyline.Owner = (IEntity)this;
            this.lwPolyline.Color2 = this.color;
            this.lwPolyline.Add(new LwPolyLineVertex((float)(-(double)this.width / 2.0), this.height / 2f));
            this.lwPolyline.Add(new LwPolyLineVertex((float)(-(double)this.width / 2.0), (float)(-(double)this.height / 2.0)));
            this.lwPolyline.Add(new LwPolyLineVertex(this.width / 2f, (float)(-(double)this.height / 2.0)));
            this.lwPolyline.Add(new LwPolyLineVertex(this.width / 2f, this.height / 2f));
            this.lwPolyline.IsClosed = true;
            this.lwPolyline.Regen();
            this.lwPolyline.Align = this.Align;
            this.lwPolyline.Location = this.Location;
            this.lwPolyline.Rotate(this.Angle);
        }

        private void RegenBoundRect()
        {
            this.BoundRect.Clear();
            if (this.lwPolyline == null)
                return;
            this.BoundRect.Union(this.lwPolyline.BoundRect);
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            if (this.IsHatchable)
                this.hatch = this.Hatch(this.hatchMode, this.hatchAngle, this.hatchAngle2, this.hatchInterval, this.hatchExclude);
            else
                this.hatch.Clear();
            this.hatch.Color2 = this.color;
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
            this.lwPolyline.Draw(view);
            if (this.isHatchable && this.hatch != null)
            {
                this.hatch.IsSelected = this.IsSelected;
                this.hatch.IsDrawPath = this.IsDrawPath;
                this.hatch.Draw(view);
            }
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            this.location = Vector2.Add(this.location, delta);
            this.lwPolyline?.Transit(delta);
            this.hatch.Transit(delta);
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
            this.location = Vector2.Transform(this.location, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.lwPolyline?.Scale(scale, this.location);
            this.hatch.Scale(scale, this.location);
            this.RegenBoundRect();
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.lwPolyline?.Scale(scale, scaleCenter);
            this.hatch.Scale(scale, scaleCenter);
            this.location = (this.location - scaleCenter) * scale + scaleCenter;
            this.RegenBoundRect();
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
