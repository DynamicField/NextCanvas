#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

// ReSharper disable PossibleNullReferenceException

namespace NextCanvas.Converters
{
    public class IsEqualToConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value.Equals(parameter);
        }
    }
}