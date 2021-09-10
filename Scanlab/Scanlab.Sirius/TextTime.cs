// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.TextTime
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>텍스트 Time 엔티티 (TTF 폰트)</summary>
    public class TextTime : Text
    {
        protected TimeFormat timeFormat;
        protected bool isLeadingWithZero;
        private bool isRegisteredIntoRtc;
        private CharacterSet characterSet;

        [JsonIgnore]
        [Browsable(false)]
        public override EType EntityType => EType.TextTime;

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
        [Browsable(false)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch")]
        [Description("Hatch 여부")]
        public override bool IsHatchable
        {
            get => this.isHatchable;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isHatchable = value;
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

        public TextTime()
        {
            this.Name = "Text Time";
            this.timeFormat = TimeFormat.Hours24;
            this.isLeadingWithZero = true;
            this.isRegisteredIntoRtc = false;
        }

        public TextTime(string text)
          : this()
        {
            this.FontText = text;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public override object Clone()
        {
            TextTime textTime1 = new TextTime();
            textTime1.Name = this.Name;
            textTime1.Description = this.Description;
            textTime1.Owner = this.Owner;
            textTime1.IsSelected = this.IsSelected;
            textTime1.isMarkerable = this.IsMarkerable;
            textTime1.isVisible = this.IsVisible;
            textTime1.isLocked = this.IsLocked;
            textTime1.color = this.Color2;
            textTime1.isHatchable = this.isHatchable;
            textTime1.hatchMode = this.hatchMode;
            textTime1.hatchAngle = this.hatchAngle;
            textTime1.hatchInterval = this.hatchInterval;
            textTime1.hatchExclude = this.hatchExclude;
            textTime1.Repeat = this.Repeat;
            textTime1.reverseMark = this.ReverseMark;
            textTime1.fontName = this.FontName;
            textTime1.width = this.Width;
            textTime1.capHeight = this.CapHeight;
            textTime1.letterSpacing = this.LetterSpacing;
            textTime1.letterSpace = this.letterSpace;
            textTime1.wordSpacing = this.wordSpacing;
            textTime1.lineSpacing = this.LineSpacing;
            textTime1.fontText = this.fontText;
            textTime1.align = this.align;
            textTime1.location = this.location;
            textTime1.OriginLeftLocation = this.OriginLeftLocation;
            textTime1.OriginRightLocation = this.OriginRightLocation;
            textTime1.angle = this.angle;
            textTime1.timeFormat = this.timeFormat;
            textTime1.isLeadingWithZero = this.isLeadingWithZero;
            textTime1.Tag = this.Tag;
            textTime1.Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            };
            TextTime textTime2 = textTime1;
            List<IEntity> entityList = new List<IEntity>(this.list.Count);
            foreach (IEntity entity1 in this.list)
            {
                IEntity entity2 = entity1 is ICloneable cloneable2 ? (IEntity)cloneable2.Clone() : (IEntity)(object)null;
                entityList.Add(entity2);
            }
            textTime2.list.AddRange((IEnumerable<IEntity>)entityList);
            textTime2.hatch = this.hatch.Clone() as Group;
            return (object)textTime2;
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
            bool flag = true & RtcCharacterSetHelper.Regen(rtc, (ICharacterSetInfo)new TTFCharacterSetInfo(this.FontName, this.Width, this.CapHeight, this.LetterSpacing, this.letterSpace, this.Angle, this.IsHatchable, this.HatchMode, this.HatchAngle, this.HatchInterval, this.HatchExclude), out this.characterSet);
            this.isRegisteredIntoRtc = flag;
            return flag;
        }
    }
}
