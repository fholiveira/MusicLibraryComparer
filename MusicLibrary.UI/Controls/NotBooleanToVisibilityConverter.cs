using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace MusicLibrary.View.Controls
{
    [ValueConversion(typeof(Boolean), typeof(Visibility))]
    public class NotBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valor = (bool)value;
            return valor ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility valor = (Visibility)value;
            return valor == Visibility.Visible ? false : true;
        }
    }
}
