
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>Vector Begin 엔티티 객체</summary>
    [JsonObject]
    public class VectorBegin : IEntity, IMarkerable, ICloneable
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
        public EType EntityType => EType.VectorBegin;

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

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
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
        [Category("Data")]
        [DisplayName("Signal")]
        [System.ComponentModel.Description("벡터 의존적 자동 레이저 제어 신호")]
        public AutoLaserControlSignal Signal
        {
            get => this.signal;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.signal = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Starting Vaue")]
        [System.ComponentModel.Description("시작 출력값")]
        public float StartingValue
        {
            get => this.startingValue;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.startingValue = value;
            }
        }

        public override string ToString() => string.Format("{0}: {1}", (object)this.Name, (object)this.signal);

        public VectorBegin()
        {
            this.Node = new TreeNode();
            this.Name = "Vector Begin";
            this.IsSelected = false;
            this.isMarkerable = true;
            this.isLocked = false;
            this.BoundRect = BoundRect.Empty;
            this.signal = AutoLaserControlSignal.Analog1;
            this.startingValue = 2f;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public virtual object Clone() => (object)new VectorBegin()
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            isMarkerable = this.IsMarkerable,
            isLocked = this.IsLocked,
            signal = this.signal,
            startingValue = this.startingValue,
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
            if (markerArg.Rtc is IRtcAutoLaserControl rtc)
            {
                switch (this.Signal)
                {
                    case AutoLaserControlSignal.ExtDO8Bit:
                    case AutoLaserControlSignal.ExtDO16:
                        flag &= rtc.ListAlcByVectorBegin<uint>(this.Signal, (uint)this.StartingValue);
                        break;
                    default:
                        flag &= rtc.ListAlcByVectorBegin<float>(this.Signal, this.StartingValue);
                        break;
                }
            }
            return flag;
        }

        public virtual void Regen()
        {
        }
    }
}
