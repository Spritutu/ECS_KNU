// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.SiriusTextDate
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>sirius text date entity</summary>
    public class SiriusTextDate : SiriusText
    {
        protected DateFormat dateFormat;
        protected bool isLeadingWithZero;
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
        public override EType EntityType => EType.SiriusTextDate;

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
        [DisplayName("Date Format")]
        [Description("날짜 포맷")]
        public DateFormat DateFormat
        {
            get => this.dateFormat;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.dateFormat = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Leading With Zero")]
        [Description("앞을 0 으로 채우기")]
        public bool IsLeadingWithZero
        {
            get => this.isLeadingWithZero;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isLeadingWithZero = value;
                this.isRegen = true;
            }
        }

        public override string ToString() => string.Format("{0} : {1} ", (object)this.Name, (object)this.DateFormat);

        public SiriusTextDate()
        {
            this.Name = "Sirius Text Date";
            this.letterSpace = LetterSpaceWay.Fixed;
            this.dateFormat = DateFormat.MonthDigit;
            this.isLeadingWithZero = true;
            this.isRegisteredIntoRtc = false;
        }

        public SiriusTextDate(string text)
          : this()
        {
            this.FontText = text;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public override object Clone()
        {
            SiriusTextDate siriusTextDate = new SiriusTextDate();
            siriusTextDate.Name = this.Name;
            siriusTextDate.Description = this.Description;
            siriusTextDate.Owner = this.Owner;
            siriusTextDate.IsSelected = this.IsSelected;
            siriusTextDate.isMarkerable = this.IsMarkerable;
            siriusTextDate.IsDrawPath = this.IsDrawPath;
            siriusTextDate.isVisible = this.IsVisible;
            siriusTextDate.isLocked = this.IsLocked;
            siriusTextDate.color = this.Color2;
            siriusTextDate.Repeat = this.Repeat;
            siriusTextDate.reverseMark = this.ReverseMark;
            siriusTextDate.fontName = this.FontName;
            siriusTextDate.width = this.width;
            siriusTextDate.capHeight = this.CapHeight;
            siriusTextDate.letterSpacing = this.LetterSpacing;
            siriusTextDate.letterSpace = this.letterSpace;
            siriusTextDate.wordSpacing = this.wordSpacing;
            siriusTextDate.lineSpacing = this.LineSpacing;
            siriusTextDate.fontText = this.fontText;
            siriusTextDate.align = this.align;
            siriusTextDate.location = this.location;
            siriusTextDate.OriginLeftLocation = this.OriginLeftLocation;
            siriusTextDate.OriginRightLocation = this.OriginRightLocation;
            siriusTextDate.angle = this.angle;
            siriusTextDate.dateFormat = this.dateFormat;
            siriusTextDate.isLeadingWithZero = this.isLeadingWithZero;
            siriusTextDate.Tag = this.Tag;
            siriusTextDate.Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            };
            return (object)siriusTextDate;
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
                flag = flag & rtc.ListJump(this.Location) & rtcCharacterSet.ListDate(this.dateFormat, this.IsLeadingWithZero, this.characterSet);
                if (!flag)
                    break;
            }
            return flag;
        }

        public override void Regen()
        {
            switch (this.DateFormat)
            {
                case DateFormat.Year2Digits:
                    this.FontText = "YY";
                    break;
                case DateFormat.Day:
                    this.FontText = "DD";
                    break;
                case DateFormat.Year4Digits:
                    this.FontText = "YYYY";
                    break;
                case DateFormat.MonthDigit:
                    this.FontText = "MM";
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
