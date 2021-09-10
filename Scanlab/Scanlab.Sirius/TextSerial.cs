
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>텍스트 Serial 엔티티 (TTF 폰트)</summary>
    public class TextSerial : Text
    {
        protected uint numOfDigits;
        protected SerialFormat serialFormat;
        private uint serialNo;
        private uint incrementStep;
        private bool isRegisteredIntoRtc;
        private CharacterSet characterSet;

        [JsonIgnore]
        [Browsable(false)]
        public override EType EntityType => EType.TextSerial;

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

        [Browsable(false)]
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

        public TextSerial()
        {
            this.Name = "Text Serial";
            this.numOfDigits = 4U;
            this.serialFormat = SerialFormat.LeadingWithZero;
            this.isRegisteredIntoRtc = false;
            this.incrementStep = 1U;
        }

        public TextSerial(string text)
          : this()
        {
            this.FontText = text;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public override object Clone()
        {
            TextSerial textSerial1 = new TextSerial();
            textSerial1.Name = this.Name;
            textSerial1.Description = this.Description;
            textSerial1.Owner = this.Owner;
            textSerial1.IsSelected = this.IsSelected;
            textSerial1.isMarkerable = this.IsMarkerable;
            textSerial1.isVisible = this.IsVisible;
            textSerial1.isLocked = this.IsLocked;
            textSerial1.color = this.Color2;
            textSerial1.isHatchable = this.isHatchable;
            textSerial1.hatchMode = this.hatchMode;
            textSerial1.hatchAngle = this.hatchAngle;
            textSerial1.hatchInterval = this.hatchInterval;
            textSerial1.hatchExclude = this.hatchExclude;
            textSerial1.Repeat = this.Repeat;
            textSerial1.reverseMark = this.ReverseMark;
            textSerial1.fontName = this.FontName;
            textSerial1.width = this.Width;
            textSerial1.capHeight = this.CapHeight;
            textSerial1.letterSpacing = this.LetterSpacing;
            textSerial1.letterSpace = this.letterSpace;
            textSerial1.wordSpacing = this.wordSpacing;
            textSerial1.lineSpacing = this.LineSpacing;
            textSerial1.fontText = this.fontText;
            textSerial1.align = this.align;
            textSerial1.location = this.location;
            textSerial1.OriginLeftLocation = this.OriginLeftLocation;
            textSerial1.OriginRightLocation = this.OriginRightLocation;
            textSerial1.angle = this.angle;
            textSerial1.numOfDigits = this.numOfDigits;
            textSerial1.serialFormat = this.serialFormat;
            textSerial1.serialNo = this.serialNo;
            textSerial1.Tag = this.Tag;
            textSerial1.Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            };
            TextSerial textSerial2 = textSerial1;
            List<IEntity> entityList = new List<IEntity>(this.list.Count);
            foreach (IEntity entity1 in this.list)
            {
                IEntity entity2 = entity1 is ICloneable cloneable2 ? (IEntity)cloneable2.Clone() : (IEntity)(object)null;
                entityList.Add(entity2);
            }
            textSerial2.list.AddRange((IEnumerable<IEntity>)entityList);
            textSerial2.hatch = this.hatch.Clone() as Group;
            return (object)textSerial2;
        }

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
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
            bool flag = true & RtcCharacterSetHelper.Regen(rtc, (ICharacterSetInfo)new TTFCharacterSetInfo(this.FontName, this.Width, this.CapHeight, this.LetterSpacing, this.letterSpace, this.Angle, this.IsHatchable, this.HatchMode, this.HatchAngle, this.HatchInterval, this.HatchExclude), out this.characterSet);
            this.isRegisteredIntoRtc = flag;
            return flag;
        }
    }
}
