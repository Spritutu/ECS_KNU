using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ECS.UI.Converter
{
    public class NullDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = (Nullable<DateTime>)value;

            if (val.HasValue && val.Value > DateTime.MinValue)
                return val.Value.ToShortDateString();

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value.ToString();
            DateTime resultDateTime;
            return DateTime.TryParse(strValue, out resultDateTime) ? resultDateTime : value;
        }
    }
}
