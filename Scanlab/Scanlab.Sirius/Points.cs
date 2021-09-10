// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Points
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>점 집합 엔티티</summary>
    [JsonObject]
    public class Points : IEntity, IMarkerable, IDrawable, ICloneable, IExplodable
    {
        private string name;
        private List<Vertex> items = new List<Vertex>();
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private Alignment align;
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
        public EType EntityType => EType.Points;

        [JsonIgnore]
        [Browsable(false)]
        internal IView View { get; set; }

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
        [JsonIgnore]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Count")]
        [System.ComponentModel.Description("개수")]
        public int Count => this.items.Count;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Items")]
        [System.ComponentModel.Description("위치 배열 구성하는 항목(들)")]     
        public Vertex[] Items
        {
            get => this.items.ToArray();
            set
            {
                if (this.Owner != null && this.isLocked || value == null)
                    return;
                this.items.Clear();
                this.items.AddRange((IEnumerable<Vertex>)value);
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

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
                Vector2 delta = value - this.location;
                if (this.Owner != null)
                    this.Transit(delta);
                this.location = value;
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

        public override string ToString() => string.Format("{0}: ({1})", (object)this.name, (object)this.Count);

        public Points()
        {
            this.Node = new TreeNode();
            this.items = new List<Vertex>();
            this.Name = nameof(Points);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.align = Alignment.Center;
            this.location = Vector2.Zero;
            this.Color2 = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.dwellTime = 0.05f;
            this.isRegen = true;
            this.Repeat = 1U;
        }

        public Points(List<Vertex> vertices)
          : this()
        {
            this.items.AddRange((IEnumerable<Vertex>)vertices);
            this.Node.Text = this.ToString() ?? "";
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Points(this.items.ToList<Vertex>())
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            IsHighlighted = this.IsHighlighted,
            isVisible = this.isVisible,
            isMarkerable = this.isMarkerable,
            isLocked = this.IsLocked,
            Repeat = this.Repeat,
            align = this.align,
            location = this.location,
            dwellTime = this.dwellTime,
            size = this.size,
            angle = this.angle,
            Color2 = this.Color2,
            BoundRect = this.BoundRect.Clone(),
            Tag = this.Tag,
            Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            }
        };

        public List<IEntity> Explode()
        {
            List<IEntity> entityList = new List<IEntity>(this.Count);
            foreach (Vertex vertex in this.items)
            {
                Point point = new Point(vertex.Location);
                point.Color2 = this.Color2;
                point.DwellTime = this.dwellTime;
                point.Size = this.size;
                point.Tag = this.Tag;
                point.Regen();
                entityList.Add((IEntity)point);
            }
            return entityList;
        }

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
                foreach (Vertex vertex in this.items)
                {
                    flag &= rtc.ListJump(vertex.Location);
                    rtc.ListLaserOn(this.DwellTime);
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
        }

        private void RegenBoundRect()
        {
            this.BoundRect.Clear();
            foreach (Vertex vertex in this.items)
                this.BoundRect.Union(vertex.Location);
            this.location = this.BoundRect.LocationByAlign(this.align);
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
            this.View = view;
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
            foreach (Vertex vertex in this.items)
                renderer.Vertex(vertex.Location.X, vertex.Location.Y);
            renderer.End();
            renderer.PointSize(0.0f);
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            for (int index = 0; index < this.items.Count; ++index)
                this.items[index] = Vertex.Translate(this.items[index], delta);
            this.location = Vector2.Add(this.location, delta);
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            for (int index = 0; index < this.items.Count; ++index)
                this.items[index] = Vertex.Rotate(this.items[index], angle, this.location);
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            for (int index = 0; index < this.items.Count; ++index)
                this.items[index] = Vertex.Rotate(this.items[index], angle, rotateCenter);
            this.location = Vector2.Transform(this.location, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            for (int index = 0; index < this.items.Count; ++index)
                this.items[index] = Vertex.Scale(this.items[index], scale, this.location);
            this.Regen();
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            for (int index = 0; index < this.items.Count; ++index)
                this.items[index] = Vertex.Scale(this.items[index], scale, scaleCenter);
            this.location = (this.location - scaleCenter) * scale + scaleCenter;
            this.Regen();
        }

        public bool HitTest(float x, float y, float threshold)
        {
            if (!this.IsVisible || !this.BoundRect.HitTest(x, y, threshold))
                return false;
            bool flag = false;
            foreach (Vertex vertex in this.items)
            {
                flag |= MathHelper.IntersectPointInCircle((double)vertex.Location.X, (double)vertex.Location.Y, (double)x, (double)y, (double)threshold);
                if (flag)
                    break;
            }
            return flag;
        }

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold)
        {
            if (!this.IsVisible || !this.BoundRect.HitTest(br, threshold))
                return false;
            bool flag = false;
            foreach (Vertex vertex in this.items)
            {
                flag |= MathHelper.IntersectPointInRect(br, (double)vertex.Location.X, (double)vertex.Location.Y, (double)threshold);
                if (flag)
                    break;
            }
            return flag;
        }
    }
}
