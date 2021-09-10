// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Spiral
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
    /// <summary>나선 엔티티</summary>
    public class Spiral : IEntity, IMarkerable, IDrawable, ICloneable, IExplodable
    {
        private string name;
        private System.Drawing.Color color;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private float innerDiameter;
        private float outterDiameter;
        private int revolutions;
        private bool closed;
        private Vector2 center;
        private float angle;
        private List<LwPolyline> list = new List<LwPolyline>();
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Spiral;

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
                if (this.list != null)
                {
                    foreach (IDrawable drawable in this.list)
                    {
                        if (drawable != null)
                            drawable.Color2 = this.color;
                    }
                }
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
        [DisplayName("Inner Diameter")]
        [System.ComponentModel.Description("안쪽 원 지름 크기 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float InnerDiameter
        {
            get => this.innerDiameter;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.innerDiameter = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Outter Diameter")]
        [System.ComponentModel.Description("바깥쪽 원 지름 크기 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float OutterDiameter
        {
            get => this.outterDiameter;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.outterDiameter = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Revolutions")]
        [System.ComponentModel.Description("회전 수")]
        public int Revolutions
        {
            get => this.revolutions;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.revolutions = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Closed")]
        [System.ComponentModel.Description("폐곡선 여부")]
        public bool Closed
        {
            get => this.closed;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.closed = value;
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
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [DisplayName("Radial Pitch")]
        [System.ComponentModel.Description("가공 간격 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float RadialPitch => (float)(((double)this.outterDiameter - (double)this.innerDiameter) / 2.0) / (float)this.revolutions;

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

        public override string ToString() => string.Format("{0} : {1:F3}", (object)this.Name, (object)this.RadialPitch);

        public Spiral()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Spiral);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.color = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.center = Vector2.Zero;
            this.isRegen = true;
            this.Repeat = 1U;
        }

        public Spiral(float innerDiameter, float outterDiameter, int revolutions, bool closed)
          : this()
        {
            this.InnerDiameter = innerDiameter;
            this.OutterDiameter = outterDiameter;
            this.Revolutions = revolutions;
            this.Closed = closed;
        }

        public Spiral(
          float x,
          float y,
          float innerDiameter,
          float outterDiameter,
          int revolutions,
          bool closed)
          : this(innerDiameter, outterDiameter, revolutions, closed)
        {
            this.center = new Vector2(x, y);
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone()
        {
            Spiral spiral = new Spiral()
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
                BoundRect = this.BoundRect.Clone(),
                Repeat = this.Repeat,
                InnerDiameter = this.InnerDiameter,
                OutterDiameter = this.OutterDiameter,
                Revolutions = this.Revolutions,
                Closed = this.Closed,
                center = this.center,
                angle = this.angle,
                Tag = this.Tag,
                Node = new TreeNode()
                {
                    Text = this.Node.Text,
                    Tag = this.Node.Tag
                }
            };
            spiral.list = new List<LwPolyline>(this.list.Count);
            foreach (LwPolyline lwPolyline in this.list)
                spiral.list.Add((LwPolyline)lwPolyline.Clone());
            return (object)spiral;
        }

        public List<IEntity> Explode()
        {
            List<IEntity> entityList = new List<IEntity>();
            foreach (LwPolyline lwPolyline in this.list)
                entityList.Add((IEntity)lwPolyline.Clone());
            return entityList;
        }

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            if ((double)this.OutterDiameter <= 0.0 || (double)this.InnerDiameter > (double)this.OutterDiameter || this.Revolutions <= 0)
                return false;
            bool flag = true;
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                foreach (LwPolyline lwPolyline in this.list)
                {
                    flag &= lwPolyline.Mark(markerArg);
                    if (!flag)
                        break;
                }
                if (!flag)
                    break;
            }
            return flag;
        }

        private void RegenVertextList()
        {
            this.list.Clear();
            LwPolyline lwPolyline = new LwPolyline();
            lwPolyline.Color2 = this.color;
            double num1 = ((double)this.OutterDiameter - (double)this.InnerDiameter) / 2.0 / (double)this.Revolutions;
            double num2 = (double)this.InnerDiameter / 2.0;
            for (int index = 0; index < this.Revolutions; ++index)
            {
                for (double num3 = 0.0; num3 < 360.0; num3 += (double)Config.AngleFactor)
                {
                    double num4 = (num3 + 360.0 * (double)index) * (Math.PI / 180.0);
                    double num5 = (double)this.InnerDiameter / 2.0 + num1 * (double)index + num1 * num3 / 360.0;
                    double num6 = num5 * Math.Cos(num4);
                    double num7 = num5 * Math.Sin(num4);
                    lwPolyline.Add(new LwPolyLineVertex((float)num6, (float)num7));
                }
            }
            if (this.Closed)
            {
                for (double num3 = 0.0; num3 < 360.0; num3 += (double)Config.AngleFactor)
                {
                    double num4 = num3 * (Math.PI / 180.0);
                    double num5 = (double)this.OutterDiameter / 2.0;
                    double num6 = num5 * Math.Cos(num4);
                    double num7 = num5 * Math.Sin(num4);
                    lwPolyline.Add(new LwPolyLineVertex((float)num6, (float)num7));
                }
            }
            lwPolyline.Add(new LwPolyLineVertex(this.OutterDiameter / 2f, 0.0f));
            lwPolyline.Owner = (IEntity)this;
            lwPolyline.Regen();
            lwPolyline.Rotate(this.angle);
            lwPolyline.Transit(this.center);
            this.list.Add(lwPolyline);
        }

        private void RegenBoundRect() => this.BoundRect = new BoundRect(this.Center.X - this.OutterDiameter / 2f, this.Center.Y + this.OutterDiameter / 2f, this.Center.X + this.OutterDiameter / 2f, this.Center.Y - this.OutterDiameter / 2f);

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
            if (this.list.Count <= 0 || (double)this.OutterDiameter <= 0.0 || ((double)this.InnerDiameter > (double)this.OutterDiameter || this.Revolutions <= 0))
                return false;
            OpenGL renderer = view.Renderer;
            if (this.IsSelected)
            {
                if (this.IsDrawPath)
                {
                    renderer.Color(Config.EntityCenterColor);
                    renderer.Begin(0U);
                    renderer.Vertex(this.Center.X, this.Center.Y);
                    renderer.End();
                }
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
            foreach (LwPolyline lwPolyline in this.list)
            {
                lwPolyline.IsSelected = this.IsSelected;
                lwPolyline.IsDrawPath = this.IsDrawPath;
                lwPolyline.Draw(view);
            }
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            foreach (LwPolyline lwPolyline in this.list)
                lwPolyline?.Transit(delta);
            this.center = Vector2.Add(this.center, delta);
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
            this.innerDiameter *= scale.X;
            this.outterDiameter *= scale.X;
            this.Regen();
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.center = (this.center - scaleCenter) * scale + scaleCenter;
            this.innerDiameter *= scale.X;
            this.outterDiameter *= scale.X;
            this.Regen();
        }

        public bool HitTest(float x, float y, float threshold)
        {
            if (!this.IsVisible || !this.BoundRect.HitTest(x, y, threshold))
                return false;
            int num1 = -1;
            int num2 = 0;
            foreach (IDrawable drawable in this.list)
            {
                if (drawable != null)
                {
                    if (drawable.HitTest(x, y, threshold))
                    {
                        num1 = num2;
                        break;
                    }
                    ++num2;
                }
            }
            return num1 >= 0;
        }

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold)
        {
            if (!this.IsVisible || !this.BoundRect.HitTest(br, threshold))
                return false;
            int num1 = -1;
            int num2 = 0;
            foreach (IDrawable drawable in this.list)
            {
                if (drawable != null)
                {
                    if (drawable.HitTest(br, threshold))
                    {
                        num1 = num2;
                        break;
                    }
                    ++num2;
                }
            }
            return num1 >= 0;
        }
    }
}
