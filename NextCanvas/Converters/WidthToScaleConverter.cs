using System;
using System.Globalization;
using System.Windows.Data;

namespace NextCanvas.Converters
{
    public class WidthToScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)(parameter ?? 100) / (int)(value ?? 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)(value ?? 1) / (double)(parameter ?? 100);
        }
    }

    public class WidthToScaleMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)(values[1] ?? 100) / (int)(values[0] ?? 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] {1, 1};
        }
    }
}
