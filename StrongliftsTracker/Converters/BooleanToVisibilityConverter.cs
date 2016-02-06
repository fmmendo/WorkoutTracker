using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace StrongliftsTracker.Converters
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return (value is bool && (bool)value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return value is Visibility && (Visibility)value == Visibility.Visible;
            }
            else
                return value is Visibility && (Visibility)value == Visibility.Collapsed;
        }
    }
}
