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

            DateTime date;
            DateTime.TryParseExact(datestring,
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out date);

            if (DateTime.Compare(DateTime.Now, date) > 0)
            {
                return "2000-01-01 00:00:00";
            }
            else if (DateTime.Compare(DateTime.Today, date.Date) == 0)
            {
                return "2000-01-02 00:00:00";
            }
            else
            {
                return "2000-01-03 00:00:00";
            }
        }

        public object ConvertBack(
         object value, Type targetType,
         object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotSupportedException("ConvertBack not supported");
            return value;
        }
    }
}

/*

                                                    <DataTrigger Binding="{Binding DueDate, Converter={StaticResource DateToColorConverter}}" Value="2000-01-01 00:00:00">
                                                        <Setter Property="Foreground" Value="Red" />
                                                    </DataTrigger>
                                                    <DataTrigger Binding="{Binding DueDate, Converter={StaticResource DateToColorConverter}}" Value="2000-01-02 00:00:00">
                                                        <Setter Property="Foreground" Value="Blue" />
                                                    </DataTrigger>
 */
