using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Taskalu
{

    [ValueConversion(typeof(object), typeof(int))]
    public class DateToColorConverter : IValueConverter
    {
        public object Convert(
        object value, Type targetType,
        object parameter, System.Globalization.CultureInfo culture)
        {
            string datestring = (string)value;

            DateTime date = DateTime.ParseExact(datestring, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            if (DateTime.Compare(DateTime.Now, date) > 0)
            {
                return +1;
            }
            else if (DateTime.Compare(DateTime.Today, date.Date) == 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        }

        public object ConvertBack(
         object value, Type targetType,
         object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack not supported");
        }
    }
}

