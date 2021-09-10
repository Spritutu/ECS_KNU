using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>Numerics.Vector2 변환기</summary>
    internal class Vector2Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType) => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(
          ITypeDescriptorContext context,
          CultureInfo culture,
          object value)
        {
            if (value is string)
            {
                string[] strArray = ((string)value).Trim().Split(',');
                if (2 == strArray.Length)
                    return (object)new Vector2(float.Parse(strArray[0]), float.Parse(strArray[1]));
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
          ITypeDescriptorContext context,
          CultureInfo culture,
          object value,
          System.Type destinationType)
        {
            return destinationType == typeof(string) ? (object)string.Format("{0:F3}, {1:F3}", (object)((Vector2)value).X, (object)((Vector2)value).Y) : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
