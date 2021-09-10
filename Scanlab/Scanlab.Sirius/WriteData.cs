
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>write data entity</summary>
    public class WriteData : IEntity, IMarkerable, ICloneable
    {
        private string name;
        private bool isMarkerable;
        private bool isLocked;
        private ExtensionChannel outputChannel;
        private float outputValue;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.WriteData;

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

        [Browsable(false)]
        public Alignment Align { get; set; }

        [Browsable(false)]
        public Vector2 Location { get; set; }

        [Browsable(false)]
        public uint Repeat { get; set; }

        [Browsable(false)]
        public float Angle { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Output Channel")]
        [System.ComponentModel.Description("출력할 확장 채널")]
        public ExtensionChannel OutputChannel
        {
            get => this.outputChannel;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.outputChannel = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Output Value")]
        [System.ComponentModel.Description("출력값 : 디지털 출력(uint), 기타(float)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float OutputValue
        {
            get => this.outputValue;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.outputValue = value;
                this.Node.Text = this.ToString() ?? "";
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

        public override string ToString() => string.Format("{0}: {1}/{2}", (object)this.name, (object)this.outputChannel, (object)this.outputValue);

        public WriteData()
        {
            this.Node = new TreeNode();
            this.Name = "Write Data";
            this.IsSelected = false;
            this.isMarkerable = true;
            this.isLocked = false;
            this.BoundRect = BoundRect.Empty;
            this.Repeat = 1U;
            this.OutputChannel = ExtensionChannel.ExtAO2;
            this.OutputValue = 2f;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new WriteData()
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            isMarkerable = this.isMarkerable,
            isLocked = this.IsLocked,
            BoundRect = this.BoundRect.Clone(),
            Repeat = this.Repeat,
            Angle = this.Angle,
            outputChannel = this.OutputChannel,
            outputValue = this.OutputValue,
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
            bool flag1 = true;
            IRtc rtc = markerArg.Rtc;
            bool flag2;
            switch (this.outputChannel)
            {
                case ExtensionChannel.ExtDO2:
                case ExtensionChannel.ExtDO8:
                case ExtensionChannel.ExtDO16:
                    uint outputValue = (uint)this.outputValue;
                    flag2 = flag1 & rtc.ListWriteData<uint>(this.OutputChannel, outputValue);
                    break;
                case ExtensionChannel.ExtAO1:
                case ExtensionChannel.ExtAO2:
                    flag2 = flag1 & rtc.ListWriteData<float>(this.OutputChannel, this.outputValue);
                    break;
                default:
                    flag2 = false;
                    break;
            }
            return flag2;
        }

        public virtual void Regen()
        {
        }
    }
}
