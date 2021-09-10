
using System;
using System.ComponentModel;
using System.Globalization;

namespace Scanlab.Sirius
{
    /// <summary>부동소수점 변환기</summary>
    internal class FloatTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType) => sourceType == typeof(string);

        public override object ConvertFrom(
          ITypeDescriptorContext context,
          CultureInfo culture,
          object value)
        {
            return value is string ? (object)float.Parse((string)value) : base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
          ITypeDescriptorContext context,
          CultureInfo culture,
          object value,
          System.Type destinationType)
        {
            return destinationType == typeof(string) && !(value is string) ? (object)((float)value).ToString("F3", (IFormatProvider)culture) : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
