// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.LwPolyline
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using Scanlab.Sirius.ClipperLib;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{

    /// <summary>
    /// LW polyline (polyline vertex container)
    /// 임시 객체로 라인 + 호 조합의 Group 으로 최종 변환됨
    /// </summary>
    [JsonObject]
    public class LwPolyline :
      ObservableList<LwPolyLineVertex>,
      IEntity,
      IMarkerable,
      IDrawable,
      ICloneable,
      IExplodable,
      IHatchable
    {
        private string name;
        private System.Drawing.Color color;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private bool isClosed;
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
        private float angle;
        private Alignment align;
        private Vector2 location;
        private bool isReverseWinding;
        /// <summary>regen 시 polylinevertex 를 explode 하여 저장하는 방식</summary>
        private List<IEntity> outlineList = new List<IEntity>();
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.LWPolyline;

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
        public System.Drawing.Color Color2
        {
            get => this.color;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.color = value;
                foreach (IEntity outline in this.outlineList)
                {
                    if (outline is IDrawable drawable1)
                        drawable1.Color2 = this.color;
                }
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

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Count")]
        [System.ComponentModel.Description("개수")]
        public new int Count => base.Count;

        [RefreshProperties(RefreshProperties.All)]
        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Items")]
        [System.ComponentModel.Description("폴리라인을 구성하는 정점 항목(들)")]
        //[Editor(typeof(LwPolyLineVertexEditor), typeof(UITypeEditor))]
        public LwPolyLineVertex[] Items
        {
            get => this.ToArray();
            set
            {
                if (value == null)
                    return;
                this.Clear();
                this.AddRange((IEnumerable<LwPolyLineVertex>)value);
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
        [DisplayName("Closed")]
        [System.ComponentModel.Description("폐곡선 여부")]
        public bool IsClosed
        {
            get => this.isClosed;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isClosed = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch")]
        [System.ComponentModel.Description("Hatch 여부 (폐곡선:Closed 만 유효함)")]
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
                if (this.Owner != null && (double)value != (double)this.angle)
                    this.Rotate(value - this.angle);
                this.angle = value;
            }
        }

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
                if (this.Owner != null)
                    this.Transit(value - this.location);
                this.location = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Repeat")]
        [System.ComponentModel.Description("가공 반복 횟수")]
        public uint Repeat { get; set; }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Reverse Winding")]
        [System.ComponentModel.Description("내부 정점들의 순서 뒤집기")]
        public bool ToReverseWinding
        {
            get => this.isReverseWinding;
            set
            {
                if (this.isReverseWinding != value)
                {
                    this.Reverse();
                    this.isRegen = true;
                }
                this.isReverseWinding = value;
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

        public override string ToString() => string.Format("{0}: ({1})", (object)this.Name, (object)this.Count);

        public LwPolyline()
        {
            this.Node = new TreeNode();
            this.Name = "LWPolyline";
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.align = Alignment.Center;
            this.location = Vector2.Zero;
            this.color = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.isClosed = false;
            this.Repeat = 1U;
            this.hatchInterval = 0.5f;
            this.hatchAngle = 90f;
            this.HatchAngle2 = 0.0f;
            this.isRegen = true;
            this.isReverseWinding = false;
        }

        public LwPolyline(List<LwPolyLineVertex> list)
          : this()
        {
            this.AddRange((IEnumerable<LwPolyLineVertex>)list);
            this.Regen();
            this.location = this.BoundRect.LocationByAlign(this.align);
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone()
        {
            LwPolyline lwPolyline = new LwPolyline()
            {
                Name = this.Name,
                Description = this.Description,
                Owner = this.Owner,
                IsSelected = this.IsSelected,
                IsHighlighted = this.IsHighlighted,
                isVisible = this.isVisible,
                isMarkerable = this.isMarkerable,
                isLocked = this.isLocked,
                Repeat = this.Repeat,
                align = this.align,
                color = this.Color2,
                BoundRect = this.BoundRect.Clone(),
                isClosed = this.IsClosed,
                isHatchable = this.isHatchable,
                hatchMode = this.hatchMode,
                hatchAngle = this.hatchAngle,
                hatchAngle2 = this.hatchAngle2,
                hatchInterval = this.hatchInterval,
                hatchExclude = this.hatchExclude,
                isReverseWinding = this.isReverseWinding,
                angle = this.angle,
                Tag = this.Tag,
                Node = new TreeNode()
                {
                    Text = this.Node.Text,
                    Tag = this.Node.Tag
                }
            };
            lwPolyline.AddRange((IEnumerable<LwPolyLineVertex>)this);
            lwPolyline.outlineList = new List<IEntity>(this.outlineList.Count);
            foreach (IEntity outline in this.outlineList)
            {
                if (outline is ICloneable cloneable1)
                    lwPolyline.outlineList.Add((IEntity)cloneable1.Clone());
            }
            lwPolyline.hatch = this.hatch.Clone() as Group;
            return (object)lwPolyline;
        }

        public List<IEntity> Explode()
        {
            List<IEntity> entityList = new List<IEntity>();
            entityList.AddRange((IEnumerable<IEntity>)this.ToEntity(true));
            if (this.hatch != null)
            {
                Group group = this.hatch.Clone() as Group;
                entityList.Add((IEntity)group);
            }
            return entityList;
        }

        public List<IEntity> ToEntity(bool withHatch = false)
        {
            List<IEntity> entityList = new List<IEntity>(base.Count);
            int num1 = 0;
            foreach (LwPolyLineVertex lwPolyLineVertex1 in (ObservableList<LwPolyLineVertex>)this)
            {
                float bulge = lwPolyLineVertex1.Bulge;
                LwPolyLineVertex v1;
                LwPolyLineVertex lwPolyLineVertex2 = new LwPolyLineVertex();
                LwPolyLineVertex lwPolyLineVertex3;
                if (num1 == this.Count - 1)
                {
                    if (this.isClosed)
                    {
                        v1 = new LwPolyLineVertex(lwPolyLineVertex1.X, lwPolyLineVertex1.Y, 0.0f, lwPolyLineVertex1.Ramp);
                        ref LwPolyLineVertex local = ref lwPolyLineVertex2;
                        lwPolyLineVertex3 = this[0];
                        double x = (double)lwPolyLineVertex3.X;
                        lwPolyLineVertex3 = this[0];
                        double y = (double)lwPolyLineVertex3.Y;
                        lwPolyLineVertex3 = this[0];
                        double ramp = (double)lwPolyLineVertex3.Ramp;
                        local = new LwPolyLineVertex((float)x, (float)y, 0.0f, (float)ramp);
                    }
                    else
                        break;
                }
                else
                {
                    v1 = new LwPolyLineVertex(lwPolyLineVertex1.X, lwPolyLineVertex1.Y, 0.0f, lwPolyLineVertex1.Ramp);
                    ref LwPolyLineVertex local = ref lwPolyLineVertex2;
                    lwPolyLineVertex3 = this[num1 + 1];
                    double x = (double)lwPolyLineVertex3.X;
                    lwPolyLineVertex3 = this[num1 + 1];
                    double y = (double)lwPolyLineVertex3.Y;
                    lwPolyLineVertex3 = this[num1 + 1];
                    double ramp = (double)lwPolyLineVertex3.Ramp;
                    local = new LwPolyLineVertex((float)x, (float)y, 0.0f, (float)ramp);
                }
                if (v1.Equals(lwPolyLineVertex2))
                {
                    ++num1;
                }
                else
                {
                    if (MathHelper.IsZero(bulge))
                    {
                        entityList.Add((IEntity)new Line(v1.X, v1.Y, lwPolyLineVertex2.X, lwPolyLineVertex2.Y, v1.Ramp, lwPolyLineVertex2.Ramp)
                        {
                            Color2 = this.Color2,
                            Owner = (IEntity)this
                        });
                    }
                    else
                    {
                        double num2 = 4.0 * Math.Atan((double)Math.Abs(bulge));
                        double num3 = LwPolyLineVertex.Distance(v1, lwPolyLineVertex2) / 2.0 / Math.Sin(num2 / 2.0);
                        if (MathHelper.IsZero((float)num3))
                        {
                            entityList.Add((IEntity)new Line(v1.X, v1.Y, lwPolyLineVertex2.X, lwPolyLineVertex2.Y, v1.Ramp, lwPolyLineVertex2.Ramp)
                            {
                                Color2 = this.Color2,
                                Owner = (IEntity)this
                            });
                        }
                        else
                        {
                            double num4 = (Math.PI - num2) / 2.0;
                            double num5 = LwPolyLineVertex.Angle(v1, lwPolyLineVertex2) + (double)Math.Sign(bulge) * num4;
                            LwPolyLineVertex lwPolyLineVertex4 = new LwPolyLineVertex(v1.X + (float)(num3 * Math.Cos(num5)), v1.Y + (float)(num3 * Math.Sin(num5)));
                            double num6 = LwPolyLineVertex.Angle(v1 - lwPolyLineVertex4) * 57.2957801818848;
                            double num7 = LwPolyLineVertex.Angle(lwPolyLineVertex2 - lwPolyLineVertex4) * 57.2957801818848;
                            double num8;
                            if ((double)bulge > 0.0)
                            {
                                num8 = num7 - num6;
                                if (num8 < 0.0)
                                    num8 = 360.0 + num8;
                            }
                            else
                            {
                                num8 = num7 - num6;
                                if (num8 > 0.0)
                                    num8 -= 360.0;
                            }
                            entityList.Add((IEntity)new Arc(lwPolyLineVertex4.X, lwPolyLineVertex4.Y, (float)num3, (float)num6, (float)num8)
                            {
                                Color2 = this.Color2,
                                Owner = (IEntity)this
                            });
                        }
                    }
                    ++num1;
                }
            }
            if (withHatch && this.isHatchable)
                entityList.AddRange((IEnumerable<IEntity>)this.hatch.Explode());
            return entityList;
        }

        /// <summary>
        /// 폴리라인을 개별 정점으로( 호는 직선 집합으로 근사) 생성
        /// explode -&gt; 점과 호로
        /// to vertices -&gt; 점
        /// </summary>
        /// <returns></returns>
        public List<Vertex> ToVertices()
        {
            List<Vertex> vertexList = new List<Vertex>(base.Count);
            int num1 = 0;
            foreach (LwPolyLineVertex lwPolyLineVertex1 in (ObservableList<LwPolyLineVertex>)this)
            {
                float bulge = lwPolyLineVertex1.Bulge;
                Vertex v1 = Vertex.Zero;
                Vertex zero = Vertex.Zero;
                LwPolyLineVertex lwPolyLineVertex2;
                if (num1 == this.Count - 1)
                {
                    if (this.isClosed)
                    {
                        v1 = new Vertex(lwPolyLineVertex1.X, lwPolyLineVertex1.Y);
                        ref Vertex local = ref zero;
                        lwPolyLineVertex2 = this[0];
                        double x = (double)lwPolyLineVertex2.X;
                        lwPolyLineVertex2 = this[0];
                        double y = (double)lwPolyLineVertex2.Y;
                        local = new Vertex((float)x, (float)y);
                    }
                    else
                        break;
                }
                else
                {
                    v1 = new Vertex(lwPolyLineVertex1.X, lwPolyLineVertex1.Y);
                    ref Vertex local = ref zero;
                    lwPolyLineVertex2 = this[num1 + 1];
                    double x = (double)lwPolyLineVertex2.X;
                    lwPolyLineVertex2 = this[num1 + 1];
                    double y = (double)lwPolyLineVertex2.Y;
                    local = new Vertex((float)x, (float)y);
                }
                if (v1.Equals((object)zero))
                {
                    ++num1;
                }
                else
                {
                    if (MathHelper.IsZero(bulge))
                    {
                        vertexList.Add(v1);
                        vertexList.Add(zero);
                    }
                    else
                    {
                        double num2 = 4.0 * Math.Atan((double)Math.Abs(bulge));
                        double num3 = Vertex.Distance(v1, zero) / 2.0 / Math.Sin(num2 / 2.0);
                        if (MathHelper.IsZero((float)num3))
                        {
                            vertexList.Add(v1);
                            vertexList.Add(zero);
                        }
                        else
                        {
                            double num4 = (Math.PI - num2) / 2.0;
                            double num5 = Vertex.Angle(v1, zero) + (double)Math.Sign(bulge) * num4;
                            Vertex vertex = new Vertex(v1.Location.X + (float)(num3 * Math.Cos(num5)), v1.Location.Y + (float)(num3 * Math.Sin(num5)));
                            double num6 = Vertex.Angle(v1 - vertex) * 57.2957801818848;
                            double num7 = Vertex.Angle(zero - vertex) * 57.2957801818848;
                            double num8;
                            if ((double)bulge > 0.0)
                            {
                                num8 = num7 - num6;
                                if (num8 < 0.0)
                                    num8 = 360.0 + num8;
                            }
                            else
                            {
                                num8 = num7 - num6;
                                if (num8 > 0.0)
                                    num8 -= 360.0;
                            }
                            double num9 = Math.Cos(num6 * (Math.PI / 180.0)) * num3 + (double)vertex.Location.X;
                            double num10 = Math.Sin(num6 * (Math.PI / 180.0)) * num3 + (double)vertex.Location.Y;
                            if (num8 > 0.0)
                            {
                                for (double num11 = num6; num11 < num6 + num8; num11 += 5.0)
                                {
                                    double num12 = Math.Cos(num11 * (Math.PI / 180.0)) * num3 + (double)vertex.Location.X;
                                    double num13 = Math.Sin(num11 * (Math.PI / 180.0)) * num3 + (double)vertex.Location.Y;
                                    vertexList.Add(new Vertex((float)num12, (float)num13));
                                }
                            }
                            else
                            {
                                for (double num14 = num6; num14 > num6 + num8; num14 -= 5.0)
                                {
                                    double num15 = Math.Cos(num14 * (Math.PI / 180.0)) * num3 + (double)vertex.Location.X;
                                    double num16 = Math.Sin(num14 * (Math.PI / 180.0)) * num3 + (double)vertex.Location.Y;
                                    vertexList.Add(new Vertex((float)num15, (float)num16));
                                }
                            }
                            double num17 = Math.Cos((num6 + num8) * (Math.PI / 180.0)) * num3 + (double)vertex.Location.X;
                            double num18 = Math.Sin((num6 + num8) * (Math.PI / 180.0)) * num3 + (double)vertex.Location.Y;
                            vertexList.Add(new Vertex((float)num17, (float)num18));
                        }
                    }
                    ++num1;
                }
            }
            return vertexList;
        }

        public Group Hatch(
          HatchMode mode,
          float angle,
          float angle2,
          float interval,
          float exclude)
        {
            if (!this.isClosed)
                return (Group)null;
            if ((double)interval <= 0.0)
                return (Group)null;
            Group group = new Group();
            group.Owner = (IEntity)this;
            group.Name = nameof(Hatch);
            group.IsEnableFastRendering = true;
            group.Color2 = this.color;
            Clipper clipper = new Clipper();
            float num1 = Math.Max(this.BoundRect.Width, this.BoundRect.Height);
            int num2 = (int)(2.0 * (double)num1 / (double)interval);
            float num3 = this.BoundRect.Bottom - num1 * 0.5f;
            for (int index = 0; index < num2 + 1; ++index)
            {
                float y = num3 + (float)index * interval;
                Vector2 vector2_1 = Vector2.Transform(new Vector2(this.BoundRect.Left - num1, y), Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), this.BoundRect.Center));
                Vector2 vector2_2 = Vector2.Transform(new Vector2(this.BoundRect.Right + num1, y), Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), this.BoundRect.Center));
                List<IntPoint> pg = new List<IntPoint>(2);
                if (index % 2 == 0)
                {
                    pg.Add(new IntPoint(Math.Round((double)vector2_1.X, 3) * 1000.0, Math.Round((double)vector2_1.Y, 3) * 1000.0));
                    pg.Add(new IntPoint(Math.Round((double)vector2_2.X, 3) * 1000.0, Math.Round((double)vector2_2.Y, 3) * 1000.0));
                }
                else
                {
                    pg.Add(new IntPoint(Math.Round((double)vector2_2.X, 3) * 1000.0, Math.Round((double)vector2_2.Y, 3) * 1000.0));
                    pg.Add(new IntPoint(Math.Round((double)vector2_1.X, 3) * 1000.0, Math.Round((double)vector2_1.Y, 3) * 1000.0));
                }
                clipper.AddPath(pg, PolyType.ptSubject, false);
            }
            if (mode == HatchMode.CrossLine)
            {
                for (int index = 0; index < num2 + 1; ++index)
                {
                    float y = num3 + (float)index * interval;
                    Vector2 vector2_3 = Vector2.Transform(new Vector2(this.BoundRect.Left - num1, y), Matrix3x2.CreateRotation(angle2 * ((float)Math.PI / 180f), this.BoundRect.Center));
                    Vector2 vector2_4 = Vector2.Transform(new Vector2(this.BoundRect.Right + num1, y), Matrix3x2.CreateRotation(angle2 * ((float)Math.PI / 180f), this.BoundRect.Center));
                    List<IntPoint> pg = new List<IntPoint>(2);
                    if (index % 2 == 0)
                    {
                        pg.Add(new IntPoint(Math.Round((double)vector2_3.X, 3) * 1000.0, Math.Round((double)vector2_3.Y, 3) * 1000.0));
                        pg.Add(new IntPoint(Math.Round((double)vector2_4.X, 3) * 1000.0, Math.Round((double)vector2_4.Y, 3) * 1000.0));
                    }
                    else
                    {
                        pg.Add(new IntPoint(Math.Round((double)vector2_4.X, 3) * 1000.0, Math.Round((double)vector2_4.Y, 3) * 1000.0));
                        pg.Add(new IntPoint(Math.Round((double)vector2_3.X, 3) * 1000.0, Math.Round((double)vector2_3.Y, 3) * 1000.0));
                    }
                    clipper.AddPath(pg, PolyType.ptSubject, false);
                }
            }
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            ClipperOffset clipperOffset = new ClipperOffset();
            List<Vertex> vertices = this.ToVertices();
            List<IntPoint> path = new List<IntPoint>(vertices.Count);
            foreach (Vertex vertex in vertices)
                path.Add(new IntPoint(Math.Round((double)vertex.Location.X, 3) * 1000.0, Math.Round((double)vertex.Location.Y, 3) * 1000.0));
            clipperOffset.AddPath(path, JoinType.jtMiter, EndType.etClosedPolygon);
            clipperOffset.Execute(ref solution, -(double)exclude * 1000.0);
            clipper.AddPaths(solution, PolyType.ptClip, true);
            PolyTree polytree = new PolyTree();
            clipper.Execute(ClipType.ctIntersection, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
            List<List<IntPoint>> intPointListList = Clipper.OpenPathsFromPolyTree(polytree);
            LwPolyline lwPolyline = (LwPolyline)null;
            for (int index = 0; index < intPointListList.Count; ++index)
            {
                float startX = (float)Math.Round((double)intPointListList[index][0].X / 1000.0, 3);
                float startY = (float)Math.Round((double)intPointListList[index][0].Y / 1000.0, 3);
                float endX = (float)Math.Round((double)intPointListList[index][1].X / 1000.0, 3);
                float endY = (float)Math.Round((double)intPointListList[index][1].Y / 1000.0, 3);
                group.Add((IEntity)new Line(startX, startY, endX, endY));
            }
            if (lwPolyline != null)
                group.Add((IEntity)lwPolyline);
            group.Regen();
            return group;
        }

        /// <summary>정점의 순서 뒤집기 (Bulge 값 내부에서 다시 계산됨)</summary>
        public override void Reverse()
        {
            lock (this.SyncRoot)
            {
                List<LwPolyLineVertex> lwPolyLineVertexList = new List<LwPolyLineVertex>(this.Count);
                for (int index = 0; index < this.Count; ++index)
                {
                    LwPolyLineVertex lwPolyLineVertex = this[index];
                    lwPolyLineVertex.Bulge = 0.0f;
                    lwPolyLineVertexList.Add(lwPolyLineVertex);
                }
                for (int index = 0; index < this.Count; ++index)
                {
                    LwPolyLineVertex lwPolyLineVertex1 = this[index];
                    if ((double)lwPolyLineVertex1.Bulge != 0.0)
                    {
                        if (index == this.Count - 1)
                        {
                            LwPolyLineVertex lwPolyLineVertex2 = lwPolyLineVertexList[0];
                            lwPolyLineVertex2.Bulge = -lwPolyLineVertex1.Bulge;
                            lwPolyLineVertexList[0] = lwPolyLineVertex2;
                        }
                        else
                        {
                            LwPolyLineVertex lwPolyLineVertex3 = lwPolyLineVertexList[index + 1];
                            lwPolyLineVertex3.Bulge = -lwPolyLineVertex1.Bulge;
                            lwPolyLineVertexList[index + 1] = lwPolyLineVertex3;
                        }
                    }
                }
                this.Clear();
                for (int index = lwPolyLineVertexList.Count - 1; index >= 0; --index)
                    this.Add(lwPolyLineVertexList[index]);
            }
            this.isRegen = true;
        }

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            bool flag = true;
            if (this.isHatchable)
            {
                for (int index = 0; (long)index < (long)this.Repeat; ++index)
                {
                    flag &= this.hatch.Mark(markerArg);
                    if (!flag)
                        break;
                }
            }
            for (int index = 0; (long)index < (long)this.Repeat && flag; ++index)
            {
                foreach (IEntity outline in this.outlineList)
                {
                    if (outline is IMarkerable markerable3)
                        flag &= markerable3.Mark(markerArg);
                    if (!flag)
                        break;
                }
            }
            return flag;
        }

        private void RegenVertextList()
        {
            this.outlineList.Clear();
            this.outlineList = this.ToEntity();
            foreach (IEntity outline in this.outlineList)
                outline.Regen();
        }

        private void RegenBoundRect()
        {
            this.BoundRect.Clear();
            foreach (IEntity outline in this.outlineList)
                this.BoundRect.Union(outline.BoundRect);
            this.location = this.BoundRect.LocationByAlign(this.align);
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            if (this.IsHatchable)
                this.hatch = this.Hatch(this.hatchMode, this.hatchAngle, this.hatchAngle2, this.hatchInterval, this.hatchExclude);
            else
                this.hatch.Clear();
            this.Node.Text = this.ToString() ?? "";
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
            foreach (IEntity outline in this.outlineList)
            {
                outline.IsSelected = this.IsSelected;
                if (outline is IDrawable drawable2)
                {
                    drawable2.IsDrawPath = this.IsDrawPath;
                    drawable2.Color2 = this.color;
                    drawable2.Draw(view);
                }
            }
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
            for (int index = 0; index < this.Count; ++index)
                this[index] = LwPolyLineVertex.Translate(this[index], delta);
            foreach (IEntity outline in this.outlineList)
            {
                if (outline is IDrawable drawable1)
                    drawable1.Transit(delta);
            }
            this.location = Vector2.Add(this.location, delta);
            this.hatch.Transit(delta);
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            for (int index = 0; index < base.Count; ++index)
                this[index] = LwPolyLineVertex.Rotate(this[index], angle, this.location);
            this.hatch.Rotate(angle, this.location);
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.location = Vector2.Transform(this.location, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            for (int index = 0; index < base.Count; ++index)
                this[index] = LwPolyLineVertex.Rotate(this[index], angle, rotateCenter);
            this.hatch.Rotate(angle, rotateCenter);
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            for (int index = 0; index < base.Count; ++index)
                this[index] = LwPolyLineVertex.Scale(this[index], scale, this.location);
            foreach (IEntity outline in this.outlineList)
            {
                if (outline is IDrawable drawable1)
                    drawable1.Scale(scale);
            }
            this.hatch.Scale(scale, this.location);
            this.Regen();
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            for (int index = 0; index < base.Count; ++index)
                this[index] = LwPolyLineVertex.Scale(this[index], scale, scaleCenter);
            this.hatch.Scale(scale, scaleCenter);
            this.Regen();
        }

        public bool HitTest(float x, float y, float threshold)
        {
            if (!this.IsVisible || !this.BoundRect.HitTest(x, y, threshold))
                return false;
            int num1 = -1;
            int num2 = 0;
            foreach (IEntity outline in this.outlineList)
            {
                if (outline is IDrawable drawable1)
                {
                    if (drawable1.HitTest(x, y, threshold))
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
            foreach (IEntity outline in this.outlineList)
            {
                if (outline is IDrawable drawable1)
                {
                    if (drawable1.HitTest(br, threshold))
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
