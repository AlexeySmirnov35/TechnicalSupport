using System;
using System.Globalization;
using System.Windows.Data;

namespace TechnicalSupport.Pages
{
    public class OverdueAndNotCompletedConverter //: IValueConverter
    {
       /* public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as YourItemType; // Замени YourItemType на тип ваших элементов

            if (item == null)
                return false;

            DateTime requestDateFinish;
            if (DateTime.TryParse(item.RequestDateFinish, out requestDateFinish))
            {
                if (requestDateFinish < DateTime.Now && item.StatusID != "Completed") // Замени "Completed" на соответствующий статус
                {
                    return true;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }*/
    }
}
