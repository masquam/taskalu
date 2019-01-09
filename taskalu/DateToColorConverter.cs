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
                "G",
                System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.None,
                out date);

            if (DateTime.Compare(DateTime.Now, date) > 0)
            {
                return "Red";
            }
            else if (DateTime.Compare(DateTime.Today, date.Date) == 0)
            {
                return "Blue";
            }
            else
            {
                return "Black";
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
