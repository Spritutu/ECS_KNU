// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.PenReturn
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>Pen Return 엔티티 객체</summary>
    [JsonObject]
    public class PenReturn : IEntity, IMarkerable, ICloneable
    {
        protected string name;
        protected bool isMarkerable;
        protected bool isLocked;

        [JsonIgnore]
        [Browsable(false)]
        public virtual IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual EType EntityType => EType.PenReturn;

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
                this.Node.Text = "Pen: " + value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Description")]
        [System.ComponentModel.Description("엔티티에 대한 설명")]
        public virtual string Description { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [JsonIgnore]
        [Browsable(false)]
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
        [DisplayName("Locked")]
        [System.ComponentModel.Description("편집 금지 여부")]
        public virtual bool IsLocked
        {
            get => this.isLocked;
            set => this.isLocked = value;
        }

        [JsonIgnore]
        [Browsable(false)]
        public virtual TreeNode Node { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual int Index { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual object Tag { get; set; }

        [Browsable(false)]
        [ReadOnly(true)]
        public virtual uint Repeat { get; set; }

        public override string ToString() => "Pen: " + this.Name;

        /// <summary>생성자 (중복펜 파라메터 사용시 이전 설정 값으로 자동 전환되는 유틸리티 펜 개체)</summary>
        public PenReturn()
        {
            this.Node = new TreeNode();
            this.Name = "Return";
            this.IsSelected = false;
            this.isMarkerable = true;
            this.isLocked = false;
            this.BoundRect = BoundRect.Empty;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public virtual object Clone() => (object)new PenReturn()
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            isMarkerable = this.IsMarkerable,
            isLocked = this.IsLocked,
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
        public virtual bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            bool flag = true;
            if (markerArg.PenStack.Count >= 2)
            {
                markerArg.PenStack.TryPop(out IPen _);
                IPen pen = markerArg.PenStack.Last<IPen>();
                flag &= pen.Mark(markerArg);
                if (flag)
                    markerArg.PenStack.TryPop(out IPen _);
            }
            return flag;
        }

        public virtual void Regen()
        {
        }
    }
}
