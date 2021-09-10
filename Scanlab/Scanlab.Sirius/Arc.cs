// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Arc
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
    /// <summary>호 (arc) 엔티티 객체</summary>
    public class Arc : IEntity, IMarkerable, IDrawable, ICloneable, IExplodable
    {
        private string name;
        private System.Drawing.Color color;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private Alignment align;
        private float radius;
        private Vector2 center;
        private float startAngle;
        private float sweepAngle;
        [JsonIgnore]
        private LwPolyline lwPolyline = new LwPolyline();
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Arc;

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
                this.Node.Text = this.ToString() ?? "";
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
                this.center = value;
                this.Node.Text = this.ToString() ?? "";
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

        [JsonIgnore]
        [Browsable(false)]
        public float Angle { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public TreeNode Node { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public int Index { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public object Tag { get; set; }

        public override string ToString() => string.Format("{0}: {1:F3}, {2:F3} R={3:F3}", (object)this.Name, (object)this.center.X, (object)this.center.Y, (object)this.radius);

        public Arc()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Arc);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.color = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.center = Vector2.Zero;
            this.radius = 1f;
            this.isRegen = true;
            this.Repeat = 1U;
        }

        public Arc(float x, float y, float radius, float startAngle, float sweepAngle)
          : this()
        {
            this.center = new Vector2(x, y);
            this.radius = radius;
            this.startAngle = startAngle;
            this.sweepAngle = sweepAngle;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Arc()
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
            radius = this.radius,
            center = this.center,
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

        public LwPolyline ToLwPolyline() => (LwPolyline)this.lwPolyline.Clone();

        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            if ((double)this.Radius <= 0.0)
                return false;
            bool flag = true;
            IRtc rtc = markerArg.Rtc;
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                double num1 = Math.Cos((double)this.StartAngle * (Math.PI / 180.0)) * (double)this.Radius + (double)this.Center.X;
                double num2 = Math.Sin((double)this.StartAngle * (Math.PI / 180.0)) * (double)this.Radius + (double)this.Center.Y;
                flag = flag & rtc.ListJump(new Vector2((float)num1, (float)num2)) & rtc.ListArc(new Vector2(this.center.X, this.center.Y), this.sweepAngle);
                if (!flag)
                    break;
            }
            return flag;
        }

        private void RegenVertextList()
        {
            this.lwPolyline.Clear();
            this.lwPolyline.Color2 = this.color;
            double num1 = Math.Cos((double)this.StartAngle * (Math.PI / 180.0)) * (double)this.Radius + (double)this.Center.X;
            double num2 = Math.Sin((double)this.StartAngle * (Math.PI / 180.0)) * (double)this.Radius + (double)this.Center.Y;
            if ((double)this.SweepAngle > 0.0)
            {
                for (double startAngle = (double)this.StartAngle; startAngle < (double)this.StartAngle + (double)this.SweepAngle; startAngle += (double)Config.AngleFactor)
                    this.lwPolyline.Add(new LwPolyLineVertex((float)Math.Cos(startAngle * (Math.PI / 180.0)) * this.Radius + this.Center.X, (float)Math.Sin(startAngle * (Math.PI / 180.0)) * this.Radius + this.Center.Y));
            }
            else
            {
                for (double startAngle = (double)this.StartAngle; startAngle > (double)this.StartAngle + (double)this.SweepAngle; startAngle -= (double)Config.AngleFactor)
                    this.lwPolyline.Add(new LwPolyLineVertex((float)Math.Cos(startAngle * (Math.PI / 180.0)) * this.Radius + this.Center.X, (float)Math.Sin(startAngle * (Math.PI / 180.0)) * this.Radius + this.Center.Y));
            }
            this.lwPolyline.Add(new LwPolyLineVertex((float)Math.Cos(((double)this.StartAngle + (double)this.SweepAngle) * (Math.PI / 180.0)) * this.Radius + this.Center.X, (float)Math.Sin(((double)this.StartAngle + (double)this.SweepAngle) * (Math.PI / 180.0)) * this.Radius + this.Center.Y));
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
            this.startAngle += angle;
            this.startAngle = MathHelper.NormalizeAngle(this.startAngle);
            this.Regen();
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.center = Vector2.Transform(this.center, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
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
