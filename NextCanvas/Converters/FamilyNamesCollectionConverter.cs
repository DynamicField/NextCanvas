using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

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
            var name = string.Empty;
            var xmlLanguage = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
            if (family.FamilyNames.Any(kv => kv.Key.Equals(xmlLanguage)))
            {
                name = family.FamilyNames[xmlLanguage];
            }
            return string.IsNullOrEmpty(name) ? family.ToString() : name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
