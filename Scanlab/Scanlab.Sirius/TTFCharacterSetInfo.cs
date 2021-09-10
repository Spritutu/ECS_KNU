
using System;

namespace Scanlab.Sirius
{
    /// <summary>TTF 폰트용 CharacterSet Info 객체</summary>
    internal class TTFCharacterSetInfo : ICharacterSetInfo, IEquatable<TTFCharacterSetInfo>
    {
        public string FontName;
        public float Width;
        public float CapHeight;
        public float LetterSpacing;
        public LetterSpaceWay LetterSpace;
        public float Angle;
        public bool IsHatchable;
        public HatchMode HatchMode;
        public float HatchAngle;
        public float HatchInterval;
        public float HatchExclude;

        public TTFCharacterSetInfo(
          string fontName,
          float width,
          float capHeight,
          float letterSpacing,
          LetterSpaceWay letterSpace,
          float angle,
          bool isHatchable,
          HatchMode hatchMode,
          float hatchAngle,
          float hatchInterval,
          float hatchExclude)
        {
            this.FontName = fontName;
            this.Width = width;
            this.CapHeight = capHeight;
            this.LetterSpacing = letterSpacing;
            this.LetterSpace = letterSpace;
            this.Angle = angle;
            this.IsHatchable = isHatchable;
            this.HatchMode = hatchMode;
            this.HatchAngle = hatchAngle;
            this.HatchInterval = hatchInterval;
            this.HatchExclude = hatchExclude;
        }

        /// <summary>다를 경우 RTC 에 캐릭터 집합을 다운로드</summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TTFCharacterSetInfo other) => other != null && other.FontName == this.FontName && (MathHelper.IsEqual(other.CapHeight, this.CapHeight) && MathHelper.IsEqual(other.Width, this.Width)) && (MathHelper.IsEqual(other.LetterSpacing, this.LetterSpacing) && other.LetterSpace == this.LetterSpace && (MathHelper.IsEqual(other.Angle, this.Angle) && other.IsHatchable == this.IsHatchable)) && (other.HatchMode == this.HatchMode && MathHelper.IsEqual(other.HatchAngle, this.HatchAngle) && MathHelper.IsEqual(other.HatchInterval, this.HatchInterval)) && MathHelper.IsEqual(other.HatchExclude, this.HatchExclude);
    }
}
