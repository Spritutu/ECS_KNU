// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Group
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Scanlab.Sirius.ClipperLib;
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
    /// <summary>그룹 엔티티 (엔티티를 모아놓은 컨테이너롤 동작)</summary>
    [JsonObject]
    public class Group :
      ObservableList<IEntity>,
      IEntity,
      IMarkerable,
      IDrawable,
      ICloneable,
      IExplodable,
      IHatchable,
      IDisposable
    {
        [JsonIgnore]
        [Browsable(false)]
        internal Dictionary<IView, uint> Views = new Dictionary<IView, uint>();
        private string name;
        private System.Drawing.Color color;
        private List<Offset> offsets = new List<Offset>();
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private bool isHatchable;
        private HatchMode hatchMode;
        private float hatchAngle;
        private float hatchAngle2;
        private float hatchInterval;
        private float hatchExclude;
        private Group hatch;
        private bool isHitTest;
        private uint repeatCount;
        private Alignment align;
        private Vector2 location;
        private float angle;
        private bool isRegen;
        private bool disposed;

        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Group;

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
                foreach (IEntity entity in (ObservableList<IEntity>)this)
                {
                    if (entity is IDrawable drawable1)
                        drawable1.Color2 = this.color;
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
        [System.ComponentModel.Description("그룹을 구성하는 엔티티 항목(들)")]
        [Editor(typeof(GroupEditor), typeof(UITypeEditor))]
        public IEntity[] Items
        {
            get => this.ToArray();
            set
            {
                if (value == null)
                    return;
                this.Clear();
                this.AddRange((IEnumerable<IEntity>)value);
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Offsets")]
        [System.ComponentModel.Description("위치 배열 구성하는 항목(들)")]
        [Editor(typeof(OffsetsEditor), typeof(UITypeEditor))]
        public Offset[] Offsets
        {
            get => this.offsets.ToArray();
            set
            {
                if (value == null)
                    return;
                lock (this.SyncRoot)
                {
                    this.offsets.Clear();
                    this.offsets.AddRange((IEnumerable<Offset>)value);
                }
                this.isRegen = true;
            }
        }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Basic")]
        [DisplayName("Offset Counts")]
        [System.ComponentModel.Description("오프셋 배열 개수")]
        public int OffsetCounts => this.offsets.Count;

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
        [Category("Status")]
        [DisplayName("Hit Test")]
        [System.ComponentModel.Description("마우스 선택 기능")]
        public bool IsHitTest
        {
            get => this.isHitTest;
            set => this.isHitTest = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Repeat")]
        [System.ComponentModel.Description("반복 가공 회수")]
        public uint Repeat
        {
            get => this.repeatCount;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.repeatCount = value;
                if (this.repeatCount > 0U)
                    return;
                this.repeatCount = 1U;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Width")]
        [System.ComponentModel.Description("폭 (mm)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Width
        {
            get => this.BoundRect.Width;
            set
            {
                if (this.Owner != null && this.isLocked || ((double)value <= 0.0 || this.BoundRect.IsEmpty))
                    return;
                float num = value / this.BoundRect.Width;
                if (this.IsFixedAspectRatio)
                    this.Scale(new Vector2(num, num));
                else
                    this.Scale(new Vector2(num, 1f));
                this.isRegen = true;
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
            get => this.BoundRect.Height;
            set
            {
                if (this.Owner != null && this.isLocked || ((double)value <= 0.0 || this.BoundRect.IsEmpty))
                    return;
                float num = value / this.BoundRect.Height;
                if (this.IsFixedAspectRatio)
                    this.Scale(new Vector2(num, num));
                else
                    this.Scale(new Vector2(1f, num));
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(false)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Fixed Aspect Ratio")]
        [System.ComponentModel.Description("고정 비율 유지 여부")]
        public bool IsFixedAspectRatio { get; set; }

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
                if (/*this.Owner != null &&*/ this.isLocked)
                    return;
                if (/*this.Owner != null &&*/ this.align != value)
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
                if (this.Owner == null)
                    return;
                this.Transit(delta);
            }
        }

        /// <summary>
        /// opengl 의 고속 렌더링을 지원하여 렌더링 속도를 올리기 위한 내부 옵션
        /// false : 모두 새로 그림
        /// true : openg의 buffer를 이용한 버퍼 그리기
        /// </summary>
        [Browsable(false)]
        public bool IsEnableFastRendering { get; set; }

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

        public override string ToString() => string.Format("{0}: {1} [{2}]", (object)this.Name, (object)this.Count, (object)this.OffsetCounts);

        public Group()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Group);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.align = Alignment.Center;
            this.location = Vector2.Zero;
            this.Repeat = 1U;
            this.color = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.IsFixedAspectRatio = true;
            this.isRegen = true;
            this.isHitTest = true;
            this.hatchInterval = 0.2f;
            this.hatchAngle = 90f;
            this.HatchAngle2 = 0.0f;
        }

        public Group(List<IEntity> entities)
          : this()
        {
            this.AddRange((IEnumerable<IEntity>)entities);
        }

        ~Group()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed || !disposing)
                return;
            foreach (IView key in this.Views.Keys.ToList<IView>())
            {
                ViewDefault viewDefault = key as ViewDefault;
                uint view = this.Views[key];
                ref uint local = ref view;
                viewDefault.DeleteList(ref local);
                this.Views[key] = view;
            }
            this.disposed = true;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone()
        {
            List<IEntity> entities = new List<IEntity>(this.Count);
            foreach (IEntity entity1 in (ObservableList<IEntity>)this)
            {
                IEntity entity2 = entity1 is ICloneable cloneable2 ? (IEntity)cloneable2.Clone() : (IEntity)(object)null;
                entities.Add(entity2);
            }
            Group group = new Group(entities)
            {
                Name = this.Name,
                Description = this.Description,
                Owner = this.Owner,
                IsSelected = this.IsSelected,
                IsHighlighted = this.IsHighlighted,
                isVisible = this.isVisible,
                isMarkerable = this.isMarkerable,
                isLocked = this.IsLocked,
                isHitTest = this.IsHitTest,
                align = this.align,
                location = this.location,
                color = this.Color2,
                BoundRect = this.BoundRect.Clone(),
                isHatchable = this.isHatchable,
                hatchMode = this.hatchMode,
                hatchAngle = this.hatchAngle,
                hatchAngle2 = this.hatchAngle2,
                hatchInterval = this.hatchInterval,
                hatchExclude = this.hatchExclude,
                Repeat = this.Repeat,
                IsEnableFastRendering = this.IsEnableFastRendering,
                IsFixedAspectRatio = this.IsFixedAspectRatio,
                Width = this.Width,
                Height = this.Height,
                angle = this.angle,
                Tag = this.Tag,
                Node = new TreeNode()
                {
                    Text = this.Node.Text,
                    Tag = this.Node.Tag
                }
            };
            group.offsets = new List<Offset>((IEnumerable<Offset>)this.offsets);
            if (this.hatch != null)
                group.hatch = this.hatch.Clone() as Group;
            return (object)group;
        }

        public List<IEntity> Explode()
        {
            if (this.offsets.Count == 0)
            {
                List<IEntity> entityList = new List<IEntity>(this.Count);
                foreach (IEntity entity in (ObservableList<IEntity>)this)
                {
                    if (entity is ICloneable cloneable3)
                        entityList.Add((IEntity)cloneable3.Clone());
                }
                if (this.hatch != null)
                {
                    Group group = (Group)this.hatch.Clone();
                    entityList.Add((IEntity)group);
                }
                return entityList;
            }
            this.offsets.Insert(0, Offset.Zero);
            List<IEntity> entityList1 = new List<IEntity>(this.offsets.Count);
            foreach (Offset offset in this.offsets)
            {
                Group group1 = new Group(((ObservableList<IEntity>)this.Clone()).ToList());
                group1.Angle = offset.Angle;
                group1.Transit(new Vector2(offset.X, offset.Y));
                entityList1.Add((IEntity)group1);
                if (this.hatch != null)
                {
                    Group group2 = (Group)this.hatch.Clone();
                    group2.Angle = offset.Angle;
                    group2.Transit(new Vector2(offset.X, offset.Y));
                    entityList1.Add((IEntity)group2);
                }
            }
            this.offsets.RemoveAt(0);
            return entityList1;
        }

        public Group Hatch(
          HatchMode mode,
          float angle,
          float angle2,
          float interval,
          float exclude)
        {
            if ((double)interval <= 0.0)
                return (Group)null;
            Group group = new Group();
            group.Owner = (IEntity)this;
            group.Name = nameof(Hatch);
            group.IsEnableFastRendering = true;
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
                    Vector2 vector2_1 = Vector2.Transform(new Vector2(this.BoundRect.Left - num1, y), Matrix3x2.CreateRotation(angle2 * ((float)Math.PI / 180f), this.BoundRect.Center));
                    Vector2 vector2_2 = Vector2.Transform(new Vector2(this.BoundRect.Right + num1, y), Matrix3x2.CreateRotation(angle2 * ((float)Math.PI / 180f), this.BoundRect.Center));
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
            }
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            ClipperOffset clipperOffset = new ClipperOffset();
            foreach (IEntity entity in (ObservableList<IEntity>)this)
            {
                if (entity is LwPolyline)
                {
                    List<Vertex> vertices = (entity as LwPolyline).ToVertices();
                    List<IntPoint> path = new List<IntPoint>(vertices.Count);
                    foreach (Vertex vertex in vertices)
                        path.Add(new IntPoint(Math.Round((double)vertex.Location.X, 3) * 1000.0, Math.Round((double)vertex.Location.Y, 3) * 1000.0));
                    clipperOffset.AddPath(path, JoinType.jtMiter, EndType.etClosedPolygon);
                }
            }
            clipperOffset.Execute(ref solution, -(double)exclude * 1000.0);
            clipper.AddPaths(solution, PolyType.ptClip, true);
            PolyTree polytree = new PolyTree();
            clipper.Execute(ClipType.ctIntersection, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
            List<List<IntPoint>> intPointListList = Clipper.OpenPathsFromPolyTree(polytree);
            for (int index = 0; index < intPointListList.Count; ++index)
            {
                float startX = (float)Math.Round((double)intPointListList[index][0].X / 1000.0, 3);
                float startY = (float)Math.Round((double)intPointListList[index][0].Y / 1000.0, 3);
                float endX = (float)Math.Round((double)intPointListList[index][1].X / 1000.0, 3);
                float endY = (float)Math.Round((double)intPointListList[index][1].Y / 1000.0, 3);
                group.Add((IEntity)new Line(startX, startY, endX, endY));
            }
            group.Regen();
            return group;
        }

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            this.offsets.Insert(0, Offset.Zero);
            bool flag = true;
            if (this.hatch != null)
            {
                foreach (Offset offset in this.offsets)
                {
                    for (int index = 0; (long)index < (long)this.Repeat; ++index)
                    {
                        markerArg.Rtc.MatrixStack.Push(offset.ToMatrix);
                        flag &= this.hatch.Mark(markerArg);
                        markerArg.Rtc.MatrixStack.Pop();
                        if (!flag)
                            return false;
                    }
                    if (!flag)
                        return false;
                }
            }
            foreach (Offset offset in this.offsets)
            {
                if (flag)
                {
                    for (int index = 0; (long)index < (long)this.Repeat; ++index)
                    {
                        foreach (IEntity e in (ObservableList<IEntity>)this)
                        {
                            if (flag)
                                flag &= sub_entity_mark(markerArg, e, offset);
                            else
                                break;
                        }
                    }
                }
                else
                    break;
            }
            this.offsets.RemoveAt(0);
            return flag;

            
        }

        static bool sub_entity_mark(IMarkerArg arg, IEntity e, Offset offset)
        {
            bool flag1 = true;
            if (!(e is IMarkerable markerable))
                return true;
            arg.Rtc.MatrixStack.Push(offset.ToMatrix);
            bool flag2 = flag1 & markerable.Mark(arg);
            arg.Rtc.MatrixStack.Pop();
            return flag2;
        }

        private void RegenVertextList()
        {
            foreach (IEntity entity in (ObservableList<IEntity>)this)
                entity.Regen();
        }

        private void RegenBoundRect()
        {
            this.BoundRect.Clear();
            foreach (IEntity entity in (ObservableList<IEntity>)this)
                this.BoundRect.Union(entity.BoundRect);
            BoundRect boundRect = this.BoundRect.Clone();
            foreach (Offset offset in this.offsets)
            {
                BoundRect br = boundRect.Clone();
                br.Transit(offset.ToVector2);
                this.BoundRect.Union(br);
            }
            this.location = this.BoundRect.LocationByAlign(this.align);
        }

        private void RegenListID()
        {
            foreach (IView view in this.Views.Keys.ToList<IView>())
            {
                OpenGL renderer = view.Renderer;
                ViewDefault viewDefault = view as ViewDefault;
                if (!viewDefault.IsRegeningListId)
                {
                    uint listID = this.Views[view];
                    viewDefault.DeleteList(ref listID);
                    this.Views[view] = listID;
                    listID = viewDefault.PrepareStartList();
                    foreach (IEntity entity in (ObservableList<IEntity>)this)
                    {
                        IDrawable drawable = entity as IDrawable;
                        bool isSelected = entity.IsSelected;
                        entity.IsSelected = false;
                        if (drawable != null)
                        {
                            drawable.Color2 = this.color;
                            drawable.Draw(view);
                        }
                        entity.IsSelected = isSelected;
                    }
                    viewDefault.PrepareEndList();
                    this.Views[view] = listID;
                }
            }
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            if (this.IsEnableFastRendering)
                this.RegenListID();
            this.hatch = !this.IsHatchable ? (Group)null : this.Hatch(this.hatchMode, this.hatchAngle, this.hatchAngle2, this.hatchInterval, this.hatchExclude);
            this.Node.Text = this.ToString() ?? "";
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
            if (this.IsEnableFastRendering && view > 0U && !viewDefault.IsRegeningListId)
            {
                viewDefault.DrawList(view);
                if (this.isHatchable && this.hatch != null)
                {
                    this.hatch.IsSelected = this.IsSelected;
                    this.hatch.IsDrawPath = this.IsDrawPath;
                    this.hatch.Color2 = this.color;
                    this.hatch.Draw(v);
                }
                foreach (Offset offset in this.offsets)
                {
                    renderer.PushMatrix();
                    renderer.Translate(offset.X, offset.Y, 0.0f);
                    renderer.Rotate(0.0f, 0.0f, offset.Angle);
                    viewDefault.DrawList(this.Views[v]);
                    if (this.isHatchable && this.hatch != null)
                    {
                        this.hatch.IsSelected = this.IsSelected;
                        this.hatch.IsDrawPath = this.IsDrawPath;
                        this.hatch.Color2 = this.color;
                        this.hatch.Draw(v);
                    }
                    renderer.PopMatrix();
                }
            }
            else
            {
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
                foreach (IEntity entity in (ObservableList<IEntity>)this)
                {
                    entity.IsSelected = this.IsSelected;
                    if (entity is IDrawable drawable9)
                    {
                        drawable9.IsDrawPath = this.IsDrawPath;
                        drawable9.Color2 = this.color;
                        drawable9.Draw(v);
                    }
                }
                if (this.isHatchable && this.hatch != null)
                {
                    this.hatch.IsSelected = this.IsSelected;
                    this.hatch.IsDrawPath = this.IsDrawPath;
                    this.hatch.Color2 = this.color;
                    this.hatch.Draw(v);
                }
                object obj = Config.EntitySelectedColor.Clone();
                Config.EntitySelectedColor = !this.IsSelected ? Config.EntityGroupOffsetNormalColor : Config.EntityGroupOffsetSelectedColor;
                Vector2 center = this.BoundRect.Center;
                foreach (Offset offset in this.offsets)
                {
                    renderer.PushMatrix();
                    renderer.Translate(offset.X, offset.Y, 0.0f);
                    renderer.Rotate(0.0f, 0.0f, offset.Angle);
                    foreach (IEntity entity in (ObservableList<IEntity>)this)
                    {
                        entity.IsSelected = true;
                        if (entity is IDrawable drawable10)
                        {
                            drawable10.IsDrawPath = false;
                            drawable10.Draw(v);
                        }
                    }
                    if (this.isHatchable && this.hatch != null)
                    {
                        this.hatch.IsSelected = this.IsSelected;
                        this.hatch.IsDrawPath = this.IsDrawPath;
                        this.hatch.Color2 = this.color;
                        this.hatch.Draw(v);
                    }
                    renderer.PopMatrix();
                }
                Config.EntitySelectedColor = (float[])obj;
            }
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            foreach (IEntity entity in (ObservableList<IEntity>)this)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Transit(delta);
            }
            this.location = Vector2.Add(this.location, delta);
            this.BoundRect.Transit(delta);
            this.hatch?.Transit(delta);
            this.Regen();
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            foreach (IEntity entity in (ObservableList<IEntity>)this)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Rotate(angle, this.location);
            }
            this.hatch?.Rotate(angle, this.location);
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
            this.isRegen = true;
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            foreach (IEntity entity in (ObservableList<IEntity>)this)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Rotate(angle, rotateCenter);
            }
            this.hatch?.Rotate(angle, rotateCenter);
            this.location = Vector2.Transform(this.location, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
            this.isRegen = true;
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            foreach (IEntity entity in (ObservableList<IEntity>)this)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Scale(scale, this.location);
            }
            this.hatch?.Scale(scale, this.location);
            this.Regen();
            this.isRegen = true;
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            foreach (IEntity entity in (ObservableList<IEntity>)this)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Scale(scale, scaleCenter);
            }
            this.location = (this.location - scaleCenter) * scale + scaleCenter;
            this.hatch?.Scale(scale, scaleCenter);
            this.Regen();
            this.isRegen = true;
        }

        public bool HitTest(float x, float y, float threshold)
        {
            if (!this.IsVisible || !this.IsHitTest || !this.BoundRect.HitTest(x, y, threshold))
                return false;
            int num1 = -1;
            int num2 = 0;
            foreach (IEntity entity in (ObservableList<IEntity>)this)
            {
                if (entity is IDrawable drawable1)
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

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.IsHitTest && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold)
        {
            if (!this.IsVisible || !this.IsHitTest || !this.BoundRect.HitTest(br, threshold))
                return false;
            int num1 = -1;
            int num2 = 0;
            foreach (IEntity entity in (ObservableList<IEntity>)this)
            {
                if (entity is IDrawable drawable1)
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
