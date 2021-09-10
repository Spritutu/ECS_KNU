// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.BlockInsert
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
    /// <summary>블럭 인서트 엔티티 (autocad 의 blockinsert 구현)</summary>
    [JsonObject]
    public class BlockInsert : IEntity, IMarkerable, IDrawable, ICloneable, IExplodable
    {
        private string name;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private string masterBlockName;
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.BlockInsert;

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
        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Offset")]
        [System.ComponentModel.Description("오프셋 항목")]
        public InsertVertex Offset { get; set; }

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
        [DisplayName("Master Block Name")]
        [System.ComponentModel.Description("마스터 블럭의 이름")]
        public string MasterBlockName
        {
            get => this.masterBlockName;
            set => this.masterBlockName = value;
        }

        /// <summary>offset 의 angle 사용된</summary>
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

        public override string ToString() => this.Name + ": " + this.masterBlockName;

        public BlockInsert() => this.isRegen = true;

        public BlockInsert(string msterBlockName)
          : this()
        {
            this.Node = new TreeNode();
            this.Name = "Insert";
            this.masterBlockName = msterBlockName;
            this.Offset = InsertVertex.Zero;
            this.Name = "Insert";
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.Color2 = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.isRegen = true;
            this.Repeat = 1U;
        }

        public BlockInsert(string msterBlockName, InsertVertex offset)
          : this(msterBlockName)
        {
            this.Offset = offset;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new BlockInsert(this.masterBlockName)
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            IsHighlighted = this.IsHighlighted,
            isVisible = this.isVisible,
            isMarkerable = this.isMarkerable,
            isLocked = this.IsLocked,
            Color2 = this.Color2,
            BoundRect = this.BoundRect.Clone(),
            Repeat = this.Repeat,
            Tag = this.Tag,
            Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            }
        };

        public List<IEntity> Explode() => new List<IEntity>();

        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            bool flag = true;
            IRtc rtc = markerArg.Rtc;
            ILaser laser = markerArg.Laser;
            Block block = ((this.Owner as Layer).Owner as IDocument).Blocks.NameOf(this.masterBlockName);
            if (block == null)
                return false;
            rtc.MatrixStack.Push(this.Offset.ToMatrix);
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                foreach (IEntity entity in (List<IEntity>)block)
                {
                    IMarkerable markerable = entity as IMarkerable;
                    flag &= markerable.Mark(markerArg);
                    if (!flag)
                        break;
                }
                if (!flag)
                    break;
            }
            rtc.MatrixStack.Pop();
            return flag;
        }

        private void RegenVertextList()
        {
            if (!(this.Owner is Layer owner) || owner.Owner == null)
                return;
            foreach (IEntity entity in (List<IEntity>)(owner.Owner as IDocument).Blocks.NameOf(this.masterBlockName))
                entity.Regen();
        }

        private void RegenBoundRect()
        {
            this.BoundRect.Clear();
            if (!(this.Owner is Layer owner1) || !(owner1.Owner is IDocument owner2))
                return;
            Block block = owner2.Blocks.NameOf(this.masterBlockName);
            if (block == null)
                return;
            foreach (IEntity entity in (List<IEntity>)block)
                this.BoundRect.Union(entity.BoundRect);
            this.BoundRect.Transit(this.Offset.Transit);
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
            if (!(this.Owner is Layer owner1) || !(owner1.Owner is IDocument owner2))
                return false;
            Block block = owner2.Blocks.NameOf(this.masterBlockName);
            if (block == null)
                return false;
            renderer.PushMatrix();
            OpenGL openGl1 = renderer;
            double x1 = (double)this.Offset.Scale.X;
            InsertVertex offset = this.Offset;
            double y1 = (double)offset.Scale.Y;
            openGl1.Scale((float)x1, (float)y1, 1f);
            OpenGL openGl2 = renderer;
            offset = this.Offset;
            double x2 = (double)offset.Scale.X;
            offset = this.Offset;
            double y2 = (double)offset.Scale.Y;
            openGl2.Translate((float)x2, (float)y2, 1f);
            OpenGL openGl3 = renderer;
            offset = this.Offset;
            double angle = (double)offset.Angle;
            openGl3.Rotate(0.0f, 0.0f, (float)angle);
            foreach (IEntity entity in (List<IEntity>)block)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Draw(view);
            }
            renderer.PopMatrix();
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            this.Offset = InsertVertex.DoTransit(this.Offset, delta);
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.Offset = InsertVertex.Rotate(this.Offset, angle, this.BoundRect.Center);
            this.Regen();
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.Offset = InsertVertex.Rotate(this.Offset, angle, rotateCenter);
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

        public bool HitTest(float x, float y, float threshold) => this.IsVisible && this.BoundRect.HitTest(x, y, threshold);

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold) => this.IsVisible && this.BoundRect.HitTest(br, threshold);
    }
}
