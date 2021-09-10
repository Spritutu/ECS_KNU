// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.SiriusText
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
    /// <summary>sirius text entity</summary>
    public class SiriusText : IEntity, IMarkerable, IDrawable, ICloneable, IExplodable
    {
        protected string name;
        protected System.Drawing.Color color;
        protected bool isVisible;
        protected bool isMarkerable;
        protected bool isLocked;
        protected bool reverseMark;
        protected string fontName;
        protected bool isFixedAspectRatio;
        protected float width;
        protected float capHeight;
        protected float letterSpacing;
        protected LetterSpaceWay letterSpace;
        protected float wordSpacing;
        protected float lineSpacing;
        protected string fontText;
        protected Alignment align;
        protected Vector2 location;
        protected float angle;
        protected List<IEntity> list = new List<IEntity>();
        protected bool isRegen;
        internal bool IsNoVertices;

        [JsonIgnore]
        [Browsable(false)]
        public virtual IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual EType EntityType => EType.SiriusText;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [System.ComponentModel.Description("엔티티의 이름")]
        public virtual string Name
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
        public virtual string Description { get; set; }

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
                    foreach (IEntity entity in this.list)
                    {
                        if (entity is IDrawable drawable2)
                            drawable2.Color2 = this.color;
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
        public virtual BoundRect BoundRect { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Selected")]
        [System.ComponentModel.Description("선택여부")]
        public virtual bool IsSelected { get; set; }

        [Browsable(false)]
        public virtual bool IsHighlighted { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Visible")]
        [System.ComponentModel.Description("스크린에 출력 여부")]
        public virtual bool IsVisible
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
        public virtual bool IsMarkerable
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
        public virtual bool IsDrawPath { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Locked")]
        [System.ComponentModel.Description("편집 금지 여부")]
        public virtual bool IsLocked
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
        public virtual uint Repeat { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Reverse Mark")]
        [System.ComponentModel.Description("역 방향 가공 여부")]
        public virtual bool ReverseMark
        {
            get => this.reverseMark;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.reverseMark = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Font Name")]
        [System.ComponentModel.Description("폰트 종류")]
        [TypeConverter(typeof(SiriusFontStringConverter))]
        public virtual string FontName
        {
            get => this.fontName;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.fontName = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Fixed Aspect Ratio")]
        [System.ComponentModel.Description("고정 비율 유지 여부로 사용시 크기는 Height 를 통해 조절된다")]
        public virtual bool IsFixedAspectRatio
        {
            get => this.isFixedAspectRatio;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isFixedAspectRatio = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Width")]
        [System.ComponentModel.Description("폰트 폭")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Width
        {
            get => this.width;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.width = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Cap. Height")]
        [System.ComponentModel.Description("폰트 대문자 높이")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float CapHeight
        {
            get => this.capHeight;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.capHeight = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Letter Spacing")]
        [System.ComponentModel.Description("글자간 간격")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float LetterSpacing
        {
            get => this.letterSpacing;
            set
            {
                if (this.Owner != null && this.isLocked || (double)value < 0.0 && (double)Math.Abs(value) > (double)this.width)
                    return;
                this.letterSpacing = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Letter Space")]
        [System.ComponentModel.Description("글자간 간격을 가변 혹은 고정폭으로 처리")]
        public virtual LetterSpaceWay LetterSpace
        {
            get => this.letterSpace;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.letterSpace = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Word Spacing")]
        [System.ComponentModel.Description("단어간 간격")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float WordSpacing
        {
            get => this.wordSpacing;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.wordSpacing = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Line Spacing")]
        [System.ComponentModel.Description("줄 간격")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float LineSpacing
        {
            get => this.lineSpacing;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.lineSpacing = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Text")]
        [System.ComponentModel.Description("텍스트 내용")]
        public virtual string FontText
        {
            get => this.fontText;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.fontText = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Align")]
        [System.ComponentModel.Description("정렬 기준위치")]
        public virtual Alignment Align
        {
            get => this.align;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                if (this.Owner != null && this.align != value)
                    this.location = this.LocationByAlign(value);
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
        public virtual Vector2 Location
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

        [Browsable(false)]
        public Vector2 OriginLeftLocation { get; set; }

        [Browsable(false)]
        public Vector2 OriginRightLocation { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Angle")]
        [System.ComponentModel.Description("회전 각도")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Angle
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

        public override string ToString() => this.Name + " : " + this.fontText;

        public SiriusText()
        {
            this.Node = new TreeNode();
            this.Name = "Sirius Text";
            this.IsSelected = false;
            this.isMarkerable = true;
            this.isVisible = true;
            this.isLocked = false;
            this.color = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.fontName = SiriusFontStringConverter.GetFirst();
            this.width = 2f;
            this.capHeight = 2f;
            this.isFixedAspectRatio = false;
            this.letterSpacing = 0.2f;
            this.wordSpacing = 2f;
            this.align = Alignment.LeftMiddle;
            this.letterSpace = LetterSpaceWay.Fixed;
            this.location = Vector2.Zero;
            this.OriginLeftLocation = Vector2.Zero;
            this.OriginRightLocation = Vector2.Zero;
            this.isRegen = true;
            this.Repeat = 1U;
            this.IsNoVertices = true;
        }

        public SiriusText(string text)
          : this()
        {
            this.FontText = text;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public virtual object Clone()
        {
            SiriusText siriusText = new SiriusText()
            {
                Name = this.Name,
                Description = this.Description,
                Owner = this.Owner,
                IsSelected = this.IsSelected,
                isMarkerable = this.IsMarkerable,
                IsDrawPath = this.IsDrawPath,
                isVisible = this.IsVisible,
                isLocked = this.IsLocked,
                color = this.Color2,
                Repeat = this.Repeat,
                reverseMark = this.ReverseMark,
                fontName = this.FontName,
                width = this.width,
                capHeight = this.CapHeight,
                isFixedAspectRatio = this.isFixedAspectRatio,
                letterSpacing = this.LetterSpacing,
                letterSpace = this.letterSpace,
                wordSpacing = this.wordSpacing,
                lineSpacing = this.LineSpacing,
                fontText = this.fontText,
                align = this.align,
                location = this.location,
                OriginLeftLocation = this.OriginLeftLocation,
                OriginRightLocation = this.OriginRightLocation,
                angle = this.angle,
                Tag = this.Tag,
                Node = new TreeNode()
                {
                    Text = this.Node.Text,
                    Tag = this.Node.Tag
                }
            };
            List<IEntity> entityList = new List<IEntity>(this.list.Count);
            foreach (IEntity entity1 in this.list)
            {
                IEntity entity2 = entity1 is ICloneable cloneable2 ? (IEntity)cloneable2.Clone() : (IEntity)(object)null;
                entityList.Add(entity2);
            }
            siriusText.list.AddRange((IEnumerable<IEntity>)entityList);
            return (object)siriusText;
        }

        public virtual List<IEntity> Explode()
        {
            List<IEntity> entityList = new List<IEntity>();
            foreach (IEntity entity1 in this.list)
            {
                if (!(entity1 is Point) && entity1 is ICloneable cloneable1)
                {
                    IEntity entity2 = (IEntity)cloneable1.Clone();
                    entityList.Add(entity1);
                }
            }
            return entityList;
        }

        public virtual bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            bool flag = true;
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                foreach (IEntity entity in this.list)
                {
                    if (entity is IMarkerable markerable4)
                        flag &= markerable4.Mark(markerArg);
                    if (!flag)
                        break;
                }
                if (!flag)
                    break;
            }
            return flag;
        }

        protected virtual Vector2 LocationByAlign(Alignment align)
        {
            switch (align)
            {
                case Alignment.LeftTop:
                    return new Vector2(this.BoundRect.Left, this.BoundRect.Top);
                case Alignment.MiddleTop:
                    return new Vector2(this.BoundRect.Center.X, this.BoundRect.Top);
                case Alignment.RightTop:
                    return new Vector2(this.BoundRect.Right, this.BoundRect.Top);
                case Alignment.LeftMiddle:
                    return this.OriginLeftLocation;
                case Alignment.Center:
                    return (this.OriginLeftLocation + this.OriginRightLocation) / 2f;
                case Alignment.RightMiddle:
                    return this.OriginRightLocation;
                case Alignment.LeftBottom:
                    return new Vector2(this.BoundRect.Left, this.BoundRect.Bottom);
                case Alignment.MiddleBottom:
                    return new Vector2(this.BoundRect.Center.X, this.BoundRect.Bottom);
                case Alignment.RightBottom:
                    return new Vector2(this.BoundRect.Right, this.BoundRect.Bottom);
                default:
                    throw new InvalidOperationException("invalid alignment value !");
            }
        }

        protected virtual void RegenVertextList()
        {
            this.list.Clear();
            this.list.Add((IEntity)new Point()
            {
                IsVisible = false,
                IsMarkerable = false,
                Location = new Vector2(0.0f, 0.0f),
                Color2 = this.color
            });
            CxfHelper cxfHelper = CxfFontCollectionHelper.Instance(this.fontName);
            float x1 = 0.0f;
            float x2 = this.width / cxfHelper.BBox.Width;
            float y = this.capHeight / cxfHelper.CapitalHeight;
            if (this.isFixedAspectRatio)
            {
                x2 = y;
                this.width = cxfHelper.BBox.Width * x2;
            }
            this.IsNoVertices = true;
            for (int index = 0; index < this.fontText.Length; ++index)
            {
                char ch = this.fontText[index];
                if (cxfHelper.Face.ContainsKey((int)ch))
                {
                    this.IsNoVertices = false;
                    Group group = (Group)cxfHelper.Face[(int)ch].Clone();
                    group.Scale(new Vector2(x2, y), Vector2.Zero);
                    float left = group.BoundRect.Left;
                    group.Transit(new Vector2(x1, 0.0f));
                    group.Color2 = this.color;
                    switch (this.letterSpace)
                    {
                        case LetterSpaceWay.Variable:
                            group.Transit(new Vector2(-left, 0.0f));
                            break;
                    }
                    this.list.Add((IEntity)group);
                    if (index == this.fontText.Length - 1)
                    {
                        switch (this.letterSpace)
                        {
                            case LetterSpaceWay.Variable:
                                x1 = x1 + group.BoundRect.Width + this.letterSpacing;
                                continue;
                            case LetterSpaceWay.Fixed:
                                x1 = x1 + this.width + this.letterSpacing;
                                continue;
                            default:
                                continue;
                        }
                    }
                    else
                    {
                        switch (this.letterSpace)
                        {
                            case LetterSpaceWay.Variable:
                                x1 = x1 + group.BoundRect.Width + this.letterSpacing;
                                continue;
                            case LetterSpaceWay.Fixed:
                                x1 = x1 + this.width + this.letterSpacing;
                                continue;
                            default:
                                continue;
                        }
                    }
                }
                else
                    x1 += this.WordSpacing;
            }
            if ((double)x1 > 0.0)
                this.list.Add((IEntity)new Point()
                {
                    IsVisible = false,
                    IsMarkerable = false,
                    Location = new Vector2(x1, 0.0f),
                    Color2 = this.color
                });
            BoundRect boundRect = new BoundRect();
            foreach (IEntity entity in this.list)
            {
                entity.Regen();
                entity.Owner = (IEntity)this;
                boundRect.Union(entity.BoundRect);
            }
            if (boundRect.IsEmpty)
                return;
            Vector2 vector2 = Vector2.Zero;
            switch (this.align)
            {
                case Alignment.LeftTop:
                    vector2 = new Vector2(this.location.X, this.location.Y - boundRect.Top);
                    break;
                case Alignment.MiddleTop:
                    vector2 = new Vector2(this.location.X - boundRect.Center.X, this.location.Y - boundRect.Top);
                    break;
                case Alignment.RightTop:
                    vector2 = new Vector2(this.location.X - boundRect.Right, this.location.Y - boundRect.Top);
                    break;
                case Alignment.LeftMiddle:
                    vector2 = this.location;
                    break;
                case Alignment.Center:
                    vector2 = new Vector2(this.location.X - boundRect.Center.X, this.location.Y);
                    break;
                case Alignment.RightMiddle:
                    vector2 = new Vector2(this.location.X - boundRect.Right, this.location.Y);
                    break;
                case Alignment.LeftBottom:
                    vector2 = new Vector2(this.location.X, this.location.Y - boundRect.Bottom);
                    break;
                case Alignment.MiddleBottom:
                    vector2 = new Vector2(this.location.X - boundRect.Center.X, this.location.Y - boundRect.Bottom);
                    break;
                case Alignment.RightBottom:
                    vector2 = new Vector2(this.location.X - boundRect.Right, this.location.Y - boundRect.Bottom);
                    break;
            }
            this.OriginLeftLocation = Vector2.Zero;
            this.OriginRightLocation = new Vector2(x1, 0.0f);
            this.OriginLeftLocation = Vector2.Add(this.OriginLeftLocation, vector2);
            this.OriginRightLocation = Vector2.Add(this.OriginRightLocation, vector2);
            foreach (IEntity entity in this.list)
            {
                if (entity is IDrawable drawable2)
                    drawable2.Transit(vector2);
                else
                    drawable2 = null;

                if (!MathHelper.IsZero(this.angle) && drawable2 != null)
                    drawable2.Rotate(this.angle, this.location);
            }
            this.OriginLeftLocation = Vector2.Transform(this.OriginLeftLocation, Matrix3x2.CreateRotation(this.angle * ((float)Math.PI / 180f), this.location));
            this.OriginRightLocation = Vector2.Transform(this.OriginRightLocation, Matrix3x2.CreateRotation(this.angle * ((float)Math.PI / 180f), this.location));
            if (!this.ReverseMark)
                return;
            this.list.Reverse();
        }

        protected virtual void RegenBoundRect()
        {
            this.BoundRect.Clear();
            if (this.list == null)
                return;
            foreach (IEntity entity in this.list)
                this.BoundRect.Union(entity.BoundRect);
        }

        public virtual void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            this.isRegen = false;
        }

        public virtual bool Draw(IView view)
        {
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            OpenGL renderer = view.Renderer;
            foreach (IEntity entity in this.list)
            {
                entity.IsSelected = this.IsSelected;
                if (entity is IDrawable drawable2)
                {
                    drawable2.IsDrawPath = this.IsDrawPath;
                    drawable2.Draw(view);
                }
            }
            return true;
        }

        public virtual void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            foreach (IEntity entity in this.list)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Transit(delta);
            }
            this.location += delta;
            this.OriginLeftLocation += delta;
            this.OriginRightLocation += delta;
            this.BoundRect.Transit(delta);
        }

        public virtual void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public virtual void Rotate(float angle, Vector2 rotateCenter)
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
            if (this.IsLocked || scale == Vector2.Zero)
                return;
            int num = scale == Vector2.One ? 1 : 0;
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero)
                return;
            int num = scale == Vector2.One ? 1 : 0;
        }

        public bool HitTest(float x, float y, float threshold)
        {
            if (!this.IsVisible || !this.BoundRect.HitTest(x, y, threshold))
                return false;
            int num1 = -1;
            int num2 = 0;
            foreach (IEntity entity in this.list)
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

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public virtual bool HitTest(BoundRect br, float threshold)
        {
            if (!this.IsVisible || !this.BoundRect.HitTest(br, threshold))
                return false;
            int num1 = -1;
            int num2 = 0;
            foreach (IEntity entity in this.list)
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

        public virtual bool RegisterCharacterSetIntoRtc(IRtc rtc) => true;
    }
}
