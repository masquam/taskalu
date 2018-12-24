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
            string date = (string)value;

            //if( number < 0.0 )
            // return -1;
            //if( number == 0.0 )
            // return 0;
            return +1;
        }

        public object ConvertBack(
         object value, Type targetType,
         object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack not supported");
        }
    }
}


/*

                <Style TargetType="{x:Type ListViewItem}">
                    <Style.Resources>
                        <local:DateToColorConverter x:Key="DateToColorConv" />
                    </Style.Resources>
                    <Style.Triggers>
                        <DataTrigger
                            Binding="{Binding DueDate, Converter={StaticResource DateToColorConv}}"
                            Value="+1">
                            <Setter Property="Foreground" Value="Red" />
                        </DataTrigger>
                        <DataTrigger
                            Binding="{Binding DueDate, Converter={StaticResource DateToColorConv}}"
                            Value="-1">
                            <Setter Property="Foreground" Value="Blue" />
                        </DataTrigger>
                    </Style.Triggers>
                </Style>
 
 */
