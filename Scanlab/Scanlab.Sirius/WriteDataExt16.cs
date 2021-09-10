
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>write data ext16 entity</summary>
    public class WriteDataExt16 : IEntity, IMarkerable, ICloneable
    {
        private string name;
        private bool isMarkerable;
        private bool isLocked;
        private ushort bitPosition;
        protected string bitName;
        private bool outputValue;
        internal static string[] InputNames;
        internal static string[] OutputNames;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.WriteDataExt16;

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
        [DisplayName("Bit Position")]
        [System.ComponentModel.Description("출력할 비트의 위치 (0~15)")]
        public ushort BitPosition
        {
            get => this.bitPosition;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.bitPosition = value;
                this.bitName = WriteDataExt16.OutputNames[(int)this.bitPosition];
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Output Name")]
        [System.ComponentModel.Description("접점의 이름")]
        [TypeConverter(typeof(WriteDataExt16StringConverter))]
        public virtual string BitName
        {
            get => this.bitName;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.bitName = value;
                this.BitPosition = (ushort)Array.FindIndex<string>(WriteDataExt16.OutputNames, (Predicate<string>)(i => i == this.bitName));
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Output Value")]
        [System.ComponentModel.Description("출력값 : on/off")]
        public bool OutputValue
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

        public override string ToString() => string.Format("{0}: [{1}] {2}", (object)this.name, (object)this.bitPosition, (object)this.outputValue);

        public WriteDataExt16()
        {
            if (WriteDataExt16.InputNames == null || WriteDataExt16.OutputNames == null)
            {
                WriteDataExt16.InputNames = new string[16];
                WriteDataExt16.OutputNames = new string[16];
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "extio.ini");
                for (int index = 0; index < 16; ++index)
                {
                    WriteDataExt16.InputNames[index] = NativeMethods.ReadIni<string>(fileName, "DIN", string.Format("{0}", (object)index));
                    WriteDataExt16.OutputNames[index] = NativeMethods.ReadIni<string>(fileName, "DOUT", string.Format("{0}", (object)index));
                }
            }
            this.Node = new TreeNode();
            this.Name = "Write Data Ext16";
            this.IsSelected = false;
            this.isMarkerable = true;
            this.isLocked = false;
            this.BoundRect = BoundRect.Empty;
            this.Repeat = 1U;
            this.BitPosition = (ushort)0;
            this.outputValue = false;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new WriteDataExt16()
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
            bitPosition = this.BitPosition,
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
        public bool Mark(IMarkerArg markerArg) => !this.IsMarkerable || (1 & (markerArg.Rtc.ListWriteExtDO16(this.BitPosition, this.OutputValue) ? 1 : 0)) != 0;

        public virtual void Regen()
        {
        }
    }
}
