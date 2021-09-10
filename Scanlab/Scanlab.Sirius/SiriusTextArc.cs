// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.SiriusTextArc
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
    /// <summary>sirius text arc entity</summary>
    public class SiriusTextArc : SiriusText
    {
        protected float radius;
        protected DirectionWay direction;
        protected float startAngle;
        [JsonIgnore]
        private LwPolyline lwPolyline = new LwPolyline();

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [Description("엔티티의 이름")]
        public override string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public override EType EntityType => EType.SiriusTextArc;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Text")]
        [Description("텍스트 내용")]
        public override string FontText
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

        [Browsable(false)]
        public override bool ReverseMark
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

        [Browsable(true)]
        [ReadOnly(true)]
        public override Alignment Align
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

        [Browsable(true)]
        [ReadOnly(true)]
        public override float Angle
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
                this.startAngle += value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Center")]
        [Description("회전 기준 위치")]
        [TypeConverter(typeof(Vector2Converter))]
        public override Vector2 Location
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
        [Category("Data")]
        [DisplayName("Radius")]
        [Description("회전 반지름")]
        public float Radius
        {
            get => this.radius;
            set
            {
                if (this.Owner != null && this.isLocked || (double)value <= 0.0)
                    return;
                this.radius = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Direction")]
        [Description("회전 방향 (CW, CCW")]
        public DirectionWay Direction
        {
            get => this.direction;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.direction = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Start Angle")]
        [Description("회전 시작 각도")]
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

        public override string ToString() => string.Format("{0} : {1} : {2}", (object)this.Name, (object)this.fontText, (object)this.radius);

        public SiriusTextArc()
        {
            this.Name = "Sirius Text Arc";
            this.letterSpace = LetterSpaceWay.Fixed;
            this.radius = 10f;
            this.direction = DirectionWay.ClockWise;
            this.startAngle = 180f;
        }

        public SiriusTextArc(string text)
          : this()
        {
            this.FontText = text;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public override object Clone()
        {
            SiriusTextArc siriusTextArc1 = new SiriusTextArc();
            siriusTextArc1.Name = this.Name;
            siriusTextArc1.Description = this.Description;
            siriusTextArc1.Owner = this.Owner;
            siriusTextArc1.IsSelected = this.IsSelected;
            siriusTextArc1.isMarkerable = this.IsMarkerable;
            siriusTextArc1.IsDrawPath = this.IsDrawPath;
            siriusTextArc1.isVisible = this.IsVisible;
            siriusTextArc1.isLocked = this.IsLocked;
            siriusTextArc1.color = this.Color2;
            siriusTextArc1.Repeat = this.Repeat;
            siriusTextArc1.reverseMark = this.ReverseMark;
            siriusTextArc1.fontName = this.FontName;
            siriusTextArc1.width = this.width;
            siriusTextArc1.capHeight = this.CapHeight;
            siriusTextArc1.letterSpacing = this.LetterSpacing;
            siriusTextArc1.letterSpace = this.letterSpace;
            siriusTextArc1.wordSpacing = this.wordSpacing;
            siriusTextArc1.lineSpacing = this.LineSpacing;
            siriusTextArc1.fontText = this.fontText;
            siriusTextArc1.align = this.align;
            siriusTextArc1.location = this.location;
            siriusTextArc1.OriginLeftLocation = this.OriginLeftLocation;
            siriusTextArc1.OriginRightLocation = this.OriginRightLocation;
            siriusTextArc1.radius = this.Radius;
            siriusTextArc1.direction = this.Direction;
            siriusTextArc1.startAngle = this.StartAngle;
            siriusTextArc1.angle = this.angle;
            siriusTextArc1.Tag = this.Tag;
            siriusTextArc1.Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            };
            SiriusTextArc siriusTextArc2 = siriusTextArc1;
            List<IEntity> entityList = new List<IEntity>(this.list.Count);
            foreach (IEntity entity1 in this.list)
            {
                IEntity entity2 = entity1 is ICloneable cloneable2 ? (IEntity)cloneable2.Clone() : (IEntity)(object)null;
                entityList.Add(entity2);
            }
            siriusTextArc2.list.AddRange((IEnumerable<IEntity>)entityList);
            siriusTextArc2.lwPolyline = this.lwPolyline.Clone() as LwPolyline;
            return (object)siriusTextArc2;
        }

        protected override void RegenVertextList()
        {
            this.list.Clear();
            CxfHelper cxfHelper = CxfFontCollectionHelper.Instance(this.fontName);
            float num1 = 0.0f;
            float num2 = 0.0f;
            float x = this.width / cxfHelper.BBox.Width;
            float y = this.capHeight / cxfHelper.CapitalHeight;
            this.IsNoVertices = true;
            for (int index = 0; index < this.fontText.Length; ++index)
            {
                char ch = this.fontText[index];
                if (cxfHelper.Face.ContainsKey((int)ch))
                {
                    this.IsNoVertices = false;
                    Group group = (Group)cxfHelper.Face[(int)ch].Clone();
                    group.Scale(new Vector2(x, y), Vector2.Zero);
                    group.Regen();
                    float left = group.BoundRect.Left;
                    switch (this.letterSpace)
                    {
                        case LetterSpaceWay.Variable:
                            group.Transit(new Vector2(-left, 0.0f));
                            break;
                    }
                    if (index == this.fontText.Length - 1)
                    {
                        switch (this.letterSpace)
                        {
                            case LetterSpaceWay.Variable:
                                num1 = num1 + group.BoundRect.Width + this.letterSpacing;
                                break;
                            case LetterSpaceWay.Fixed:
                                num1 = num1 + this.width + this.letterSpacing;
                                break;
                        }
                    }
                    else
                    {
                        switch (this.letterSpace)
                        {
                            case LetterSpaceWay.Variable:
                                num1 = num1 + group.BoundRect.Width + this.letterSpacing;
                                break;
                            case LetterSpaceWay.Fixed:
                                num1 = num1 + this.width + this.letterSpacing;
                                break;
                        }
                    }
                    switch (this.direction)
                    {
                        case DirectionWay.ClockWise:
                            group.Rotate(-90f, Vector2.Zero);
                            group.Transit(new Vector2(this.radius, 0.0f));
                            float angle1 = this.StartAngle - (float)((double)num2 / (double)this.radius * 57.2957801818848);
                            group.Rotate(angle1, Vector2.Zero);
                            break;
                        case DirectionWay.CounterClockWise:
                            group.Rotate(90f, Vector2.Zero);
                            group.Transit(new Vector2(this.radius + this.CapHeight, 0.0f));
                            float angle2 = this.StartAngle + (float)((double)num2 / (double)this.radius * 57.2957801818848);
                            group.Rotate(angle2, Vector2.Zero);
                            break;
                    }
                    group.Transit(this.Location);
                    this.list.Add((IEntity)group);
                }
                else
                    num1 += this.WordSpacing;
                num2 = num1;
            }
            if (this.ReverseMark)
                this.list.Reverse();
            this.lwPolyline.Clear();
            double num3 = Math.Cos((double)this.StartAngle * (Math.PI / 180.0)) * (double)this.Radius + (double)this.Location.X;
            double num4 = Math.Sin((double)this.StartAngle * (Math.PI / 180.0)) * (double)this.Radius + (double)this.Location.Y;
            for (double num5 = 0.0; num5 < 360.0; num5 += (double)Config.AngleFactor)
                this.lwPolyline.Add(new LwPolyLineVertex((float)Math.Cos(num5 * (Math.PI / 180.0)) * this.Radius + this.Location.X, (float)Math.Sin(num5 * (Math.PI / 180.0)) * this.Radius + this.Location.Y));
            this.lwPolyline.IsClosed = true;
        }

        public override bool Mark(IMarkerArg markerArg)
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

        public override bool Draw(IView view)
        {
            OpenGL renderer = view.Renderer;
            if (this.IsSelected)
            {
                this.lwPolyline.Color2 = System.Drawing.Color.DarkGray;
                renderer.Enable(2852U);
                renderer.LineStipple(1, (ushort)21845);
                this.lwPolyline.Draw(view);
                renderer.Disable(2852U);
                renderer.Color(Config.EntityCenterColor);
                renderer.Begin(0U);
                renderer.Vertex(this.Location.X, this.Location.Y);
                renderer.End();
            }
            return base.Draw(view);
        }

        protected override void RegenBoundRect()
        {
            base.RegenBoundRect();
            this.BoundRect.Union(new BoundRect()
            {
                Width = (float)(((double)this.Radius + (double)this.CapHeight) * 2.0),
                Height = (float)(((double)this.Radius + (double)this.CapHeight) * 2.0),
                Center = this.Location
            });
        }

        public override void Transit(Vector2 delta)
        {
            base.Transit(delta);
            this.lwPolyline.Transit(delta);
        }

        public override void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.StartAngle += angle;
        }

        public override void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.StartAngle += angle;
            this.location = Vector2.Transform(this.location, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.lwPolyline.Rotate(angle, rotateCenter);
        }
    }
}
