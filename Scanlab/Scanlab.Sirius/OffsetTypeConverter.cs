
using System.ComponentModel;
using System.Globalization;

namespace Scanlab.Sirius
{
    /// <summary>Offset 타입 변환기</summary>
    internal class OffsetTypeConverter : TypeConverter
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
                    return (object)new Offset(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
          ITypeDescriptorContext context,
          CultureInfo culture,
          object value,
          System.Type destinationType)
        {
            return destinationType == typeof(string) ? (object)value.ToString() : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
