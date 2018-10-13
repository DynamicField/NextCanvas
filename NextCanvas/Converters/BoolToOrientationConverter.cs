using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace NextCanvas.Converters
{
    public class BoolToOrientationConverter : IValueConverter
    {
        /// <summary>
        /// Converts the thing to orientation
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">true to reverse</param>
        /// <param name="culture"></param>
        /// <returns>orientation :3</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            bool parameterBool = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out parameterBool);
            }
            if (parameterBool)
            {
                val = !val; // reverse it :v
            }
            if (val) // true so vertical
            {
                return Orientation.Horizontal;
            }
            else
            {
                return Orientation.Vertical;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
