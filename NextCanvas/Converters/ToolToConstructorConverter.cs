using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using NextCanvas.ViewModels;

namespace NextCanvas.Converters
{
    public class ToolToConstructorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StrokeToolViewModel s)
            {
                return s.StrokeConstructor;
            }
            return DependencyProperty.UnsetValue; // aka null
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
