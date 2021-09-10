// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.AlcVectorBegin
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>ALC by Vector Dependent Begin 엔티티 객체</summary>
    [JsonObject]
    public class AlcVectorBegin : IEntity, IMarkerable, ICloneable
    {
        protected string name;
        protected bool isMarkerable;
        protected bool isLocked;
        private AutoLaserControlSignal signal;
        private float startingValue;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.AlcVectorBegin;

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

        [RefreshProperties(RefreshProperties.All)]
        [JsonIgnore]
        [Browsable(false)]
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
        [ReadOnly(true)]
        public uint Repeat { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("ALC")]
        [DisplayName("Signal")]
        [System.ComponentModel.Description("출력 신호 종류")]
        public AutoLaserControlSignal Signal
        {
            get => this.signal;
            set
            {
                this.signal = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("ALC")]
        [DisplayName("Starting Value")]
        [System.ComponentModel.Description("출력 시작 값 (AO: ~10, DO: ~255, 65535, PW: us, FREQ: Hz")]
        public float StartingValue
        {
            get => this.startingValue;
            set => this.startingValue = value;
        }

        public override string ToString() => string.Format("{0}: {1}", (object)this.name, (object)this.signal);

        /// <summary>생성자</summary>
        public AlcVectorBegin()
        {
            this.Node = new TreeNode();
            this.Name = "ALC Vector Begin";
            this.IsSelected = false;
            this.isMarkerable = true;
            this.isLocked = false;
            this.BoundRect = BoundRect.Empty;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public virtual object Clone() => (object)new AlcVectorBegin()
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            isMarkerable = this.IsMarkerable,
            isLocked = this.IsLocked,
            signal = this.Signal,
            startingValue = this.StartingValue,
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
            bool flag1 = true;
            if (!(markerArg.Rtc is IRtcAutoLaserControl rtc))
                return flag1;
            bool flag2;
            switch (this.signal)
            {
                case AutoLaserControlSignal.ExtDO8Bit:
                case AutoLaserControlSignal.ExtDO16:
                    flag2 = flag1 & rtc.ListAlcByVectorBegin<uint>(this.signal, (uint)this.StartingValue);
                    break;
                default:
                    flag2 = flag1 & rtc.ListAlcByVectorBegin<float>(this.signal, this.StartingValue);
                    break;
            }
            return flag2;
        }

        public virtual void Regen()
        {
        }
    }
}
