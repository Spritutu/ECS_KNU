
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
    /// <summary>circle entity</summary>
    public class Trepan : IEntity, IMarkerable, IDrawable, ICloneable, IExplodable
    {
        private string name;
        private System.Drawing.Color color;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private float innerDiameter;
        private float outterDiameter;
        private uint revolutions;
        private Vector2 center;
        private float angle;
        private List<LwPolyline> list = new List<LwPolyline>();
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Trepan;

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

        [Browsable(false)]
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
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Revolutions")]
        [System.ComponentModel.Description("회전 수")]
        public uint Revolutions
        {
            get => this.revolutions;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.revolutions = value;
                if (this.revolutions <= 0U)
                    this.revolutions = 1U;
                this.Node.Text = this.ToString() ?? "";
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

        public override string ToString() => string.Format("{0} : {1}", (object)this.Name, (object)this.revolutions);

        public Trepan()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Trepan);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.color = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.center = Vector2.Zero;
            this.revolutions = 1U;
            this.isRegen = true;
            this.Repeat = 1U;
        }

        public Trepan(float outterDiameter, float innerDiameter)
          : this()
        {
            this.outterDiameter = outterDiameter;
            this.innerDiameter = innerDiameter;
        }

        public Trepan(float x, float y, float outterDiameter, float innerDiameter)
          : this(outterDiameter, innerDiameter)
        {
            this.center = new Vector2(x, y);
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone()
        {
            Trepan trepan = new Trepan()
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
                Repeat = this.Repeat,
                BoundRect = this.BoundRect.Clone(),
                center = this.center,
                OutterDiameter = this.OutterDiameter,
                InnerDiameter = this.InnerDiameter,
                revolutions = this.Revolutions,
                angle = this.angle,
                Tag = this.Tag,
                Node = new TreeNode()
                {
                    Text = this.Node.Text,
                    Tag = this.Node.Tag
                }
            };
            trepan.list = new List<LwPolyline>(this.list.Count);
            foreach (LwPolyline lwPolyline in this.list)
                trepan.list.Add((LwPolyline)lwPolyline.Clone());
            return (object)trepan;
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
            if ((double)this.innerDiameter <= 0.0 || (double)this.outterDiameter <= 0.0)
                return false;
            bool flag1 = true;
            IRtc rtc = markerArg.Rtc;
            rtc.MatrixStack.Push(this.center);
            rtc.MatrixStack.Push((double)this.angle);
            bool flag2 = flag1 & rtc.ListJump(new Vector2(this.outterDiameter / 2f - this.innerDiameter, 0.0f)) & rtc.ListArc(new Vector2((float)(((double)this.outterDiameter - (double)this.innerDiameter) / 2.0), 0.0f), 180f);
            for (uint index = 0; index < this.Revolutions; ++index)
            {
                flag2 &= rtc.ListArc(new Vector2(0.0f, 0.0f), 360f);
                if (!flag2)
                    break;
            }
            bool flag3 = flag2 & rtc.ListArc(new Vector2((float)(((double)this.outterDiameter - (double)this.innerDiameter) / 2.0), 0.0f), 180f);
            rtc.MatrixStack.Pop();
            rtc.MatrixStack.Pop();
            return flag3;
        }

        private void RegenVertextList()
        {
            this.list.Clear();
            LwPolyline lwPolyline1 = new LwPolyline();
            LwPolyline lwPolyline2 = new LwPolyline();
            LwPolyline lwPolyline3 = new LwPolyline();
            lwPolyline1.Color2 = this.color;
            lwPolyline2.Color2 = this.color;
            lwPolyline3.Color2 = this.color;
            for (int index = 180; index < 360; index += Config.AngleFactor)
            {
                double num1 = Math.Cos((double)index * (Math.PI / 180.0)) * (double)this.innerDiameter / 2.0 + ((double)this.outterDiameter / 2.0 - (double)this.innerDiameter / 2.0);
                double num2 = Math.Sin((double)index * (Math.PI / 180.0)) * (double)this.innerDiameter / 2.0;
                lwPolyline1.Add(new LwPolyLineVertex((float)num1, (float)num2));
            }
            double num3 = Math.Cos(6.28318548202515) * (double)this.innerDiameter / 2.0 + ((double)this.outterDiameter / 2.0 - (double)this.innerDiameter / 2.0);
            double num4 = Math.Sin(6.28318548202515) * (double)this.innerDiameter / 2.0;
            lwPolyline1.Add(new LwPolyLineVertex((float)num3, (float)num4));
            lwPolyline1.Regen();
            this.list.Add(lwPolyline1);
            for (int index = 0; index < 360; index += Config.AngleFactor)
            {
                double num1 = Math.Cos((double)index * (Math.PI / 180.0)) * (double)this.outterDiameter / 2.0;
                double num2 = Math.Sin((double)index * (Math.PI / 180.0)) * (double)this.outterDiameter / 2.0;
                lwPolyline2.Add(new LwPolyLineVertex((float)num1, (float)num2));
            }
            lwPolyline2.IsClosed = true;
            lwPolyline2.Regen();
            this.list.Add(lwPolyline2);
            for (int index = 0; index < 180; index += Config.AngleFactor)
            {
                double num1 = Math.Cos((double)index * (Math.PI / 180.0)) * (double)this.innerDiameter / 2.0 + ((double)this.outterDiameter / 2.0 - (double)this.innerDiameter / 2.0);
                double num2 = Math.Sin((double)index * (Math.PI / 180.0)) * (double)this.innerDiameter / 2.0;
                lwPolyline3.Add(new LwPolyLineVertex((float)num1, (float)num2));
            }
            double num5 = Math.Cos(3.14159274101257) * (double)this.innerDiameter / 2.0 + ((double)this.outterDiameter / 2.0 - (double)this.innerDiameter / 2.0);
            double num6 = Math.Sin(3.14159274101257) * (double)this.innerDiameter / 2.0;
            lwPolyline3.Add(new LwPolyLineVertex((float)num5, (float)num6));
            lwPolyline3.Regen();
            this.list.Add(lwPolyline3);
            foreach (LwPolyline lwPolyline4 in this.list)
            {
                lwPolyline4.Owner = (IEntity)this;
                lwPolyline4.Transit(this.center);
                lwPolyline4.Rotate(this.angle, this.center);
                lwPolyline4.Regen();
            }
        }

        private void RegenBoundRect()
        {
            float left = this.center.X - this.outterDiameter / 2f;
            float right = this.center.X + this.outterDiameter / 2f;
            float top = this.center.Y + this.outterDiameter / 2f;
            float bottom = this.center.Y - this.outterDiameter / 2f;
            this.BoundRect = new BoundRect(left, top, right, bottom);
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
            foreach (LwPolyline lwPolyline in this.list)
            {
                lwPolyline.IsSelected = this.IsSelected;
                lwPolyline.IsDrawPath = this.IsDrawPath;
                lwPolyline.Color2 = this.color;
                lwPolyline.Draw(view);
            }
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked)
                return;
            this.center = Vector2.Add(this.Center, delta);
            foreach (LwPolyline lwPolyline in this.list)
                lwPolyline.Transit(delta);
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked)
                return;
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked)
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
            foreach (LwPolyline lwPolyline in this.list)
            {
                if (lwPolyline.HitTest(x, y, threshold))
                {
                    num1 = num2;
                    break;
                }
                ++num2;
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
            foreach (LwPolyline lwPolyline in this.list)
            {
                if (lwPolyline.HitTest(br, threshold))
                {
                    num1 = num2;
                    break;
                }
                ++num2;
            }
            return num1 >= 0;
        }
    }
}
