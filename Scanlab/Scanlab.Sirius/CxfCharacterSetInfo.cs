using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    internal class CxfCharacterSetInfo : ICharacterSetInfo, IEquatable<CxfCharacterSetInfo>
    {
        public string FontName;
        public float Width;
        public float CapHeight;
        public float LetterSpacing;
        public LetterSpaceWay LetterSpace;
        public float Angle;

        public CxfCharacterSetInfo(
          string fontName,
          float width,
          float capHeight,
          float letterSpacing,
          LetterSpaceWay letterSpace,
          float angle)
        {
            this.FontName = fontName;
            this.Width = width;
            this.CapHeight = capHeight;
            this.LetterSpacing = letterSpacing;
            this.LetterSpace = letterSpace;
            this.Angle = angle;
        }

        /// <summary>다를 경우 RTC 에 캐릭터 집합을 다운로드</summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CxfCharacterSetInfo other) => other != null && other.FontName == this.FontName && (MathHelper.IsEqual(other.Width, this.Width) && MathHelper.IsEqual(other.CapHeight, this.CapHeight)) && (MathHelper.IsEqual(other.LetterSpacing, this.LetterSpacing) && other.LetterSpace == this.LetterSpace) && MathHelper.IsEqual(other.Angle, this.Angle);
    }
}
