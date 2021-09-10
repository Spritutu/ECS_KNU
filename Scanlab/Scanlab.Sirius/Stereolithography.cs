// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Stereolithography
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>Stereolithography 엔티티</summary>
    public class Stereolithography : IEntity, IMarkerable, IDrawable, ICloneable
    {
        [JsonIgnore]
        [Browsable(false)]
        internal Dictionary<IView, uint> Views = new Dictionary<IView, uint>();
        private string name;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private Stereolithography.FillMode mode;
        private Vector3 location;
        private float angle;
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Stereolithography;

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

        public Stereolithography.FillMode Mode
        {
            get => this.mode;
            set
            {
                this.mode = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Items")]
        [System.ComponentModel.Description("면을 구성하는 정점 항목(들)")]
        public STLFace[] Items
        {
            get => this.Facets.ToArray();
            set
            {
                if (value == null)
                    return;
                this.Facets.Clear();
                this.Facets.AddRange((IEnumerable<STLFace>)value);
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        private List<STLFace> Facets { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Width")]
        [System.ComponentModel.Description("폭 (mm)")]
        public float Width { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Height")]
        [System.ComponentModel.Description("높이 (mm)")]
        public float Height { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Depth")]
        [System.ComponentModel.Description("깊이 (mm)")]
        public float Depth { get; set; }

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
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Location
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

        public override string ToString() => string.Format("{0}: {1}", (object)this.Name, (object)this.Facets.Count);

        public Stereolithography()
        {
            this.Node = new TreeNode();
            this.Facets = new List<STLFace>();
            this.Name = "STL";
            this.mode = Stereolithography.FillMode.Points;
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.Color2 = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.location = Vector3.Zero;
            this.isRegen = true;
            this.Repeat = 1U;
        }

        public Stereolithography(float x, float y)
          : this()
        {
            this.location.X = x;
            this.location.Y = y;
        }

        public Stereolithography(Vector3 v)
          : this()
        {
            this.isRegen = false;
            this.location = v;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Stereolithography()
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
            angle = this.angle,
            BoundRect = this.BoundRect.Clone(),
            Facets = new List<STLFace>((IEnumerable<STLFace>)this.Facets),
            mode = this.mode,
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
            int num = 0;
            while ((long)num < (long)this.Repeat && flag)
                ++num;
            return flag;
        }

        private void RegenVertextList()
        {
        }

        private void RegenBoundRect() => this.BoundRect = new BoundRect(this.Location.X, this.Location.Y, this.Location.X, this.Location.Y);

        private void RegenListID()
        {
            foreach (IView key in this.Views.Keys.ToList<IView>())
            {
                OpenGL renderer = key.Renderer;
                ViewDefault viewDefault = key as ViewDefault;
                if (!viewDefault.IsRegeningListId)
                {
                    uint listID = this.Views[key];
                    viewDefault.DeleteList(ref listID);
                    this.Views[key] = listID;
                    listID = viewDefault.PrepareStartList();
                    switch (this.Mode)
                    {
                        case Stereolithography.FillMode.Points:
                            renderer.PolygonMode(1032U, 6912U);
                            break;
                        case Stereolithography.FillMode.WireFrames:
                            renderer.PolygonMode(1032U, 6913U);
                            break;
                        case Stereolithography.FillMode.Fill:
                            renderer.PolygonMode(1032U, 6914U);
                            break;
                    }
                    OpenGL openGl = renderer;
                    System.Drawing.Color color2 = this.Color2;
                    int r = (int)color2.R;
                    color2 = this.Color2;
                    int g = (int)color2.G;
                    color2 = this.Color2;
                    int b = (int)color2.B;
                    openGl.Color((byte)r, (byte)g, (byte)b);
                    renderer.PushMatrix();
                    renderer.Translate(this.Location.X, this.Location.Y, this.Location.Z);
                    renderer.Begin(4U);
                    for (int index = 0; index < this.Facets.Count; ++index)
                    {
                        renderer.Normal(this.Facets[index].Normal.X, this.Facets[index].Normal.Y, this.Facets[index].Normal.Z);
                        renderer.Normal(this.Facets[index].V1.X, this.Facets[index].V1.Y, this.Facets[index].V1.Z);
                        renderer.Normal(this.Facets[index].V2.X, this.Facets[index].V2.Y, this.Facets[index].V2.Z);
                        renderer.Normal(this.Facets[index].V3.X, this.Facets[index].V3.Y, this.Facets[index].V3.Z);
                    }
                    renderer.End();
                    renderer.PopMatrix();
                    viewDefault.PrepareEndList();
                    this.Views[key] = listID;
                }
            }
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            this.RegenListID();
            this.isRegen = false;
        }

        public bool Draw(IView v)
        {
            if (!this.Views.ContainsKey(v))
            {
                this.Views.Add(v, 0U);
                this.isRegen = true;
            }
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            ViewDefault viewDefault = v as ViewDefault;
            OpenGL renderer = v.Renderer;
            uint view = this.Views[v];
            if (this.IsSelected)
            {
                renderer.Color(Config.EntitySelectedColor[0], Config.EntitySelectedColor[1], Config.EntitySelectedColor[2]);
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
            if (view > 0U && !viewDefault.IsRegeningListId)
            {
                renderer.PushMatrix();
                viewDefault.DrawList(view);
                renderer.PopMatrix();
            }
            else
            {
                switch (this.Mode)
                {
                    case Stereolithography.FillMode.Points:
                        renderer.PolygonMode(1032U, 6912U);
                        break;
                    case Stereolithography.FillMode.WireFrames:
                        renderer.PolygonMode(1032U, 6913U);
                        break;
                    case Stereolithography.FillMode.Fill:
                        renderer.PolygonMode(1032U, 6914U);
                        break;
                }
                renderer.PushMatrix();
                renderer.Translate(this.Location.X, this.Location.Y, this.Location.Z);
                renderer.Begin(4U);
                for (int index = 0; index < this.Facets.Count; ++index)
                {
                    renderer.Normal(this.Facets[index].Normal.X, this.Facets[index].Normal.Y, this.Facets[index].Normal.Z);
                    renderer.Normal(this.Facets[index].V1.X, this.Facets[index].V1.Y, this.Facets[index].V1.Z);
                    renderer.Normal(this.Facets[index].V2.X, this.Facets[index].V2.Y, this.Facets[index].V2.Z);
                    renderer.Normal(this.Facets[index].V3.X, this.Facets[index].V3.Y, this.Facets[index].V3.Z);
                }
                renderer.End();
                renderer.PopMatrix();
            }
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.location = new Vector3(Vector2.Transform(new Vector2(this.location.X, this.location.Y), Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter)), this.location.Z);
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
        }

        public bool HitTest(float x, float y, float threshold) => this.IsVisible && this.BoundRect.HitTest(x, y, threshold) && MathHelper.IntersectPointInCircle((double)this.Location.X, (double)this.Location.Y, (double)x, (double)y, (double)threshold);

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold) => this.IsVisible && this.BoundRect.HitTest(br, threshold) && MathHelper.IntersectPointInRect(br, (double)this.Location.X, (double)this.Location.Y, (double)threshold);

        public enum FillMode
        {
            Points,
            WireFrames,
            Fill,
        }
    }
}
