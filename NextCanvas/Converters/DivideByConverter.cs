using System;
using System.Globalization;
using System.Windows.Data;

// ReSharper disable PossibleNullReferenceException

namespace NextCanvas.Converters
{
    public class DivideByConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double) value / (double) parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double) value * (double) parameter;
        }
    }
}