// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.SiriusTextTime
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>sirius text time entity</summary>
    public class SiriusTextTime : SiriusText
    {
        protected TimeFormat timeFormat;
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
        public override EType EntityType => EType.SiriusTextTime;

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
        [DisplayName("Time Format")]
        [Description("시간 포맷")]
        public TimeFormat TimeFormat
        {
            get => this.timeFormat;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.timeFormat = value;
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

        public override string ToString() => string.Format("{0} : {1}", (object)this.Name, (object)this.timeFormat);

        public SiriusTextTime()
        {
            this.Name = "Sirius Text Time";
            this.letterSpace = LetterSpaceWay.Fixed;
            this.timeFormat = TimeFormat.Hours24;
            this.isLeadingWithZero = true;
            this.isRegisteredIntoRtc = false;
        }

        public SiriusTextTime(string text)
          : this()
        {
            this.FontText = text;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public override object Clone()
        {
            SiriusTextTime siriusTextTime = new SiriusTextTime();
            siriusTextTime.Name = this.Name;
            siriusTextTime.Description = this.Description;
            siriusTextTime.Owner = this.Owner;
            siriusTextTime.IsSelected = this.IsSelected;
            siriusTextTime.isMarkerable = this.IsMarkerable;
            siriusTextTime.IsDrawPath = this.IsDrawPath;
            siriusTextTime.isVisible = this.IsVisible;
            siriusTextTime.isLocked = this.IsLocked;
            siriusTextTime.color = this.Color2;
            siriusTextTime.Repeat = this.Repeat;
            siriusTextTime.reverseMark = this.ReverseMark;
            siriusTextTime.fontName = this.FontName;
            siriusTextTime.width = this.width;
            siriusTextTime.capHeight = this.CapHeight;
            siriusTextTime.letterSpacing = this.LetterSpacing;
            siriusTextTime.letterSpace = this.letterSpace;
            siriusTextTime.wordSpacing = this.wordSpacing;
            siriusTextTime.lineSpacing = this.LineSpacing;
            siriusTextTime.fontText = this.fontText;
            siriusTextTime.align = this.align;
            siriusTextTime.location = this.location;
            siriusTextTime.OriginLeftLocation = this.OriginLeftLocation;
            siriusTextTime.OriginRightLocation = this.OriginRightLocation;
            siriusTextTime.angle = this.angle;
            siriusTextTime.timeFormat = this.timeFormat;
            siriusTextTime.isLeadingWithZero = this.isLeadingWithZero;
            siriusTextTime.Tag = this.Tag;
            siriusTextTime.Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            };
            return (object)siriusTextTime;
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
                flag = flag & rtc.ListJump(this.Location) & rtcCharacterSet.ListTime(this.timeFormat, this.IsLeadingWithZero, this.characterSet);
                if (!flag)
                    break;
            }
            return flag;
        }

        public override void Regen()
        {
            switch (this.TimeFormat)
            {
                case TimeFormat.Hours24:
                    this.FontText = "HH";
                    break;
                case TimeFormat.Minutes:
                    this.FontText = "MM";
                    break;
                case TimeFormat.Seconds:
                    this.FontText = "SS";
                    break;
                case TimeFormat.Hours12:
                    this.FontText = "HH";
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
