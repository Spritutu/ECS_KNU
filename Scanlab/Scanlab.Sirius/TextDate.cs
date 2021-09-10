
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>텍스트 Date 엔티티 (TTF 폰트)</summary>
    public class TextDate : Text
    {
        protected DateFormat dateFormat;
        protected bool isLeadingWithZero;
        private bool isRegisteredIntoRtc;
        private CharacterSet characterSet;

        [JsonIgnore]
        [Browsable(false)]
        public override EType EntityType => EType.TextDate;

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

        public override string ToString() => string.Format("{0} : {1}", (object)this.Name, (object)this.dateFormat);

        public TextDate()
        {
            this.Name = "Text Date";
            this.dateFormat = DateFormat.MonthDigit;
            this.isLeadingWithZero = true;
            this.isRegisteredIntoRtc = false;
        }

        public TextDate(string text)
          : this()
        {
            this.FontText = text;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public override object Clone()
        {
            TextDate textDate1 = new TextDate();
            textDate1.Name = this.Name;
            textDate1.Description = this.Description;
            textDate1.Owner = this.Owner;
            textDate1.IsSelected = this.IsSelected;
            textDate1.isMarkerable = this.IsMarkerable;
            textDate1.isVisible = this.IsVisible;
            textDate1.isLocked = this.IsLocked;
            textDate1.color = this.Color2;
            textDate1.isHatchable = this.isHatchable;
            textDate1.hatchMode = this.hatchMode;
            textDate1.hatchAngle = this.hatchAngle;
            textDate1.hatchInterval = this.hatchInterval;
            textDate1.hatchExclude = this.hatchExclude;
            textDate1.Repeat = this.Repeat;
            textDate1.reverseMark = this.ReverseMark;
            textDate1.fontName = this.FontName;
            textDate1.width = this.Width;
            textDate1.capHeight = this.CapHeight;
            textDate1.letterSpacing = this.LetterSpacing;
            textDate1.letterSpace = this.letterSpace;
            textDate1.wordSpacing = this.wordSpacing;
            textDate1.lineSpacing = this.LineSpacing;
            textDate1.fontText = this.fontText;
            textDate1.align = this.align;
            textDate1.location = this.location;
            textDate1.OriginLeftLocation = this.OriginLeftLocation;
            textDate1.OriginRightLocation = this.OriginRightLocation;
            textDate1.angle = this.angle;
            textDate1.dateFormat = this.dateFormat;
            textDate1.isLeadingWithZero = this.isLeadingWithZero;
            textDate1.Tag = this.Tag;
            textDate1.Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            };
            TextDate textDate2 = textDate1;
            List<IEntity> entityList = new List<IEntity>(this.list.Count);
            foreach (IEntity entity1 in this.list)
            {
                IEntity entity2 = entity1 is ICloneable cloneable2 ? (IEntity)cloneable2.Clone() : (IEntity)(object)null;
                entityList.Add(entity2);
            }
            textDate2.list.AddRange((IEnumerable<IEntity>)entityList);
            textDate2.hatch = this.hatch.Clone() as Group;
            return (object)textDate2;
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
            bool flag = true & RtcCharacterSetHelper.Regen(rtc, (ICharacterSetInfo)new TTFCharacterSetInfo(this.FontName, this.Width, this.CapHeight, this.LetterSpacing, this.letterSpace, this.Angle, this.IsHatchable, this.HatchMode, this.HatchAngle, this.HatchInterval, this.HatchExclude), out this.characterSet);
            this.isRegisteredIntoRtc = flag;
            return flag;
        }
    }
}
