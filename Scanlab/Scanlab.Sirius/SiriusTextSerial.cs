
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>sirius text serial entity</summary>
    public class SiriusTextSerial : SiriusText
    {
        protected uint numOfDigits;
        protected SerialFormat serialFormat;
        private uint serialNo;
        private uint incrementStep;
        private bool isRegisteredIntoRtc;
        private CharacterSet characterSet;

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
        public override EType EntityType => EType.SiriusTextSerial;

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

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Digits")]
        [Description("자리수")]
        public uint NumOfDigits
        {
            get => this.numOfDigits;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.numOfDigits = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Format")]
        [Description("포맷")]
        public SerialFormat SerialFormat
        {
            get => this.serialFormat;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.serialFormat = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Serial No")]
        [Description("시리얼 시작 번호")]
        public uint SerialNo
        {
            get => this.serialNo;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.serialNo = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Increment Step")]
        [Description("시리얼 시작 번호")]
        public uint IncrementStep
        {
            get => this.incrementStep;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.incrementStep = value;
                this.isRegen = true;
            }
        }

        public override string ToString() => string.Format("{0} : {1}", (object)this.Name, (object)this.serialFormat);

        public SiriusTextSerial()
        {
            this.Name = "Sirius Text Serial";
            this.letterSpace = LetterSpaceWay.Fixed;
            this.numOfDigits = 4U;
            this.serialFormat = SerialFormat.LeadingWithZero;
            this.isRegisteredIntoRtc = false;
            this.incrementStep = 1U;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public override object Clone()
        {
            SiriusTextSerial siriusTextSerial = new SiriusTextSerial();
            siriusTextSerial.Name = this.Name;
            siriusTextSerial.Description = this.Description;
            siriusTextSerial.Owner = this.Owner;
            siriusTextSerial.IsSelected = this.IsSelected;
            siriusTextSerial.isMarkerable = this.IsMarkerable;
            siriusTextSerial.IsDrawPath = this.IsDrawPath;
            siriusTextSerial.isVisible = this.IsVisible;
            siriusTextSerial.isLocked = this.IsLocked;
            siriusTextSerial.color = this.Color2;
            siriusTextSerial.Repeat = this.Repeat;
            siriusTextSerial.reverseMark = this.ReverseMark;
            siriusTextSerial.fontName = this.FontName;
            siriusTextSerial.width = this.width;
            siriusTextSerial.capHeight = this.CapHeight;
            siriusTextSerial.letterSpacing = this.LetterSpacing;
            siriusTextSerial.letterSpace = this.letterSpace;
            siriusTextSerial.wordSpacing = this.wordSpacing;
            siriusTextSerial.lineSpacing = this.LineSpacing;
            siriusTextSerial.fontText = this.fontText;
            siriusTextSerial.align = this.align;
            siriusTextSerial.location = this.location;
            siriusTextSerial.OriginLeftLocation = this.OriginLeftLocation;
            siriusTextSerial.OriginRightLocation = this.OriginRightLocation;
            siriusTextSerial.angle = this.angle;
            siriusTextSerial.numOfDigits = this.numOfDigits;
            siriusTextSerial.serialFormat = this.serialFormat;
            siriusTextSerial.serialNo = this.serialNo;
            siriusTextSerial.Tag = this.Tag;
            siriusTextSerial.Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            };
            return (object)siriusTextSerial;
        }

        public override bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            IRtc rtc = markerArg.Rtc;
            if (!(rtc is IRtcCharacterSet rtcCharacterSet))
                return true;
            if (!this.isRegisteredIntoRtc)
            {
                Logger.Log(Logger.Type.Error, "try register character set into rtc at first : " + this.Name, Array.Empty<object>());
                return false;
            }
            bool flag = true;
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                flag = flag & rtc.ListJump(this.Location) & rtcCharacterSet.ListSerial(this.numOfDigits, this.serialFormat, this.characterSet);
                if (!flag)
                    break;
            }
            return flag;
        }

        public override void Regen()
        {
            switch (this.serialFormat)
            {
                case SerialFormat.LeadingWithZero:
                    this.FontText = this.serialNo.ToString(string.Format("D{0}", (object)this.numOfDigits));
                    break;
                case SerialFormat.NoLeadingAndLeftAligned:
                    this.FontText = this.serialNo.ToString();
                    break;
                case SerialFormat.LeadingWithBlank:
                    this.FontText = this.serialNo.ToString();
                    int length = this.fontText.Length;
                    if ((long)this.numOfDigits > (long)length)
                    {
                        this.FontText.PadLeft((int)this.numOfDigits - length);
                        break;
                    }
                    break;
            }
            base.Regen();
            this.isRegisteredIntoRtc = false;
        }

        public override bool RegisterCharacterSetIntoRtc(IRtc rtc)
        {
            bool flag = true & RtcCharacterSetHelper.Regen(rtc, (ICharacterSetInfo)new CxfCharacterSetInfo(this.FontName, this.Width, this.CapHeight, this.LetterSpacing, this.LetterSpace, this.Angle), out this.characterSet);
            this.isRegisteredIntoRtc = flag;
            return flag;
        }
    }
}
