using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.Pages;
namespace TechnicalSupport.Converters
{
    public class RequestStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var request = value as Request;
            if (request == null)
                return  Brushes.Transparent;
            
            DateTime finishDate;
            DateTime finishDatenull;
            DateTime deadlineDate;
            DateTime.TryParse(request.RequestDateFinish, out finishDate);

            DateTime.TryParse(request.RequestDeadline, out deadlineDate);
            if (string.IsNullOrEmpty(request.RequestDateFinish))
            {
                finishDatenull=DateTime.Today;
                if (finishDatenull > deadlineDate)
                {
                    return Brushes.Brown;
                }
            }


            if (deadlineDate == finishDate && request.RequestDateFinish==null)
            {
                return Brushes.DarkOrange;
            }
            if (deadlineDate<finishDate ) // Assuming 1 means completed
            {
                return Brushes.Brown; // Color for overdue and not completed
            }
            
            return Brushes.Transparent; // Default color
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
