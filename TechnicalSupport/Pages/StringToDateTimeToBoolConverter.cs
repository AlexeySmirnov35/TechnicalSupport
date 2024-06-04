using System;
using System.Globalization;
using System.Windows.Data;

namespace TechnicalSupport.Pages
{
    public class StringToDateTimeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString && DateTime.TryParse(dateString, out DateTime dateTime))
            {
                return dateTime < DateTime.Now;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
