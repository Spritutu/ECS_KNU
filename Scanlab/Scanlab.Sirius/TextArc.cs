// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.TextArc
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
    /// <summary>텍스트 Arc 엔티티 (TTF 폰트)</summary>
    public class TextArc : Text
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
        public override EType EntityType => EType.TextArc;

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
                this.Node.Text = this.ToString() ?? "";
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

        public override string ToString() => string.Format("{0} : {1}, {2:F3}", (object)this.Name, (object)this.fontText, (object)this.radius);

        public TextArc()
        {
            this.Name = "Text Arc";
            this.letterSpacing = 0.2f;
            this.wordSpacing = 2f;
            this.direction = DirectionWay.ClockWise;
            this.radius = 10f;
            this.startAngle = 180f;
        }

        public TextArc(string text)
          : this()
        {
            this.FontText = text;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public override object Clone()
        {
            TextArc textArc1 = new TextArc();
            textArc1.Name = this.Name;
            textArc1.Description = this.Description;
            textArc1.Owner = this.Owner;
            textArc1.IsSelected = this.IsSelected;
            textArc1.isMarkerable = this.IsMarkerable;
            textArc1.isVisible = this.IsVisible;
            textArc1.isLocked = this.IsLocked;
            textArc1.color = this.Color2;
            textArc1.isHatchable = this.isHatchable;
            textArc1.hatchMode = this.hatchMode;
            textArc1.hatchAngle = this.hatchAngle;
            textArc1.hatchInterval = this.hatchInterval;
            textArc1.hatchExclude = this.hatchExclude;
            textArc1.Repeat = this.Repeat;
            textArc1.reverseMark = this.ReverseMark;
            textArc1.fontName = this.FontName;
            textArc1.width = this.Width;
            textArc1.capHeight = this.CapHeight;
            textArc1.letterSpacing = this.LetterSpacing;
            textArc1.letterSpace = this.letterSpace;
            textArc1.wordSpacing = this.wordSpacing;
            textArc1.lineSpacing = this.LineSpacing;
            textArc1.fontText = this.fontText;
            textArc1.align = this.align;
            textArc1.location = this.location;
            textArc1.OriginLeftLocation = this.OriginLeftLocation;
            textArc1.OriginRightLocation = this.OriginRightLocation;
            textArc1.radius = this.Radius;
            textArc1.direction = this.Direction;
            textArc1.startAngle = this.StartAngle;
            textArc1.angle = this.angle;
            textArc1.Tag = this.Tag;
            textArc1.Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            };
            TextArc textArc2 = textArc1;
            List<IEntity> entityList = new List<IEntity>(this.list.Count);
            foreach (IEntity entity1 in this.list)
            {
                IEntity entity2 = entity1 is ICloneable cloneable2 ? (IEntity)cloneable2.Clone() : (IEntity)(object)null;
                entityList.Add(entity2);
            }
            textArc2.list.AddRange((IEnumerable<IEntity>)entityList);
            textArc2.hatch = this.hatch.Clone() as Group;
            textArc2.lwPolyline = this.lwPolyline.Clone() as LwPolyline;
            return (object)textArc2;
        }

        protected override void RegenVertextList()
        {
            this.list.Clear();
            float num1 = 0.0f;
            float num2 = 0.0f;
            for (int index = 0; index < this.fontText.Length; ++index)
            {
                FreeTypeHelper freeTypeHelper = new FreeTypeHelper(this.fontName, this.fontText[index].ToString(), this.Width, this.CapHeight);
                if (freeTypeHelper.List.Count > 2)
                {
                    Group group1 = new Group();
                    group1.Align = Alignment.LeftBottom;
                    group1.Location = Vector2.Zero;
                    group1.AddRange((IEnumerable<IEntity>)freeTypeHelper.List);
                    group1.Regen();
                    if (this.IsHatchable)
                    {
                        Group group2 = group1.Hatch(this.HatchMode, this.hatchAngle, this.HatchAngle2, this.hatchInterval, this.hatchExclude);
                        group2.IsEnableFastRendering = false;
                        group2.Regen();
                        group1.Insert(0, (IEntity)group2);
                    }
                    float left = group1.BoundRect.Left;
                    switch (this.letterSpace)
                    {
                        case LetterSpaceWay.Variable:
                            group1.Transit(new Vector2(-left, 0.0f));
                            break;
                    }
                    if (index == this.fontText.Length - 1)
                    {
                        switch (this.letterSpace)
                        {
                            case LetterSpaceWay.Variable:
                                num1 = num1 + group1.BoundRect.Width + this.letterSpacing;
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
                                num1 = num1 + group1.BoundRect.Width + this.letterSpacing;
                                break;
                            case LetterSpaceWay.Fixed:
                                num1 = num1 + this.width + this.letterSpacing;
                                break;
                        }
                    }
                    switch (this.direction)
                    {
                        case DirectionWay.ClockWise:
                            group1.Rotate(-90f, Vector2.Zero);
                            group1.Transit(new Vector2(this.radius, 0.0f));
                            float angle1 = this.StartAngle - (float)((double)num2 / (double)this.radius * 57.2957801818848);
                            group1.Rotate(angle1, Vector2.Zero);
                            break;
                        case DirectionWay.CounterClockWise:
                            group1.Rotate(90f, Vector2.Zero);
                            group1.Transit(new Vector2(this.radius + this.CapHeight, 0.0f));
                            float angle2 = this.StartAngle + (float)((double)num2 / (double)this.radius * 57.2957801818848);
                            group1.Rotate(angle2, Vector2.Zero);
                            break;
                    }
                    group1.Transit(this.Location);
                    this.list.Add((IEntity)group1);
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

        public override bool Mark(IMarkerArg markerArg) => base.Mark(markerArg);

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
