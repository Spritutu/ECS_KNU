using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>Vector2 타입 변환기</summary>
    internal class Vector3Converter : TypeConverter
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
                if (3 == strArray.Length)
                    return (object)new Vector3(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
          ITypeDescriptorContext context,
          CultureInfo culture,
          object value,
          System.Type destinationType)
        {
            return destinationType == typeof(string) ? (object)string.Format("{0:F3}, {1:F3}, {2:F3}", (object)((Vector3)value).X, (object)((Vector3)value).Y, (object)((Vector3)value).Z) : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
