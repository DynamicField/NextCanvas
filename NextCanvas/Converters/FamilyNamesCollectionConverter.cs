using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace NextCanvas.Converters
{
    public class FamilyNamesCollectionConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return "<Font>";
            }
#endif
            if (value is null || !(value is FontFamily family)) return "";
            var name = family.FamilyNames[XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.Name)];
            return string.IsNullOrEmpty(name) ? family.ToString() : name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
