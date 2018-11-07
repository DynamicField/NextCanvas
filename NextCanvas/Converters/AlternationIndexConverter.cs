#region

using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

#endregion

namespace NextCanvas.Converters
{
    public class AlternationIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var itemsControl = values[0] as ItemsControl;
            var item = values[1];
            var itemContainer = itemsControl?.ItemContainerGenerator.ContainerFromItem(item);

            // It may not yet be in the collection...yeah...
            if (itemContainer == null)
            {
                return Binding.DoNothing;
            }

            var itemIndex = itemsControl.ItemContainerGenerator.IndexFromContainer(itemContainer);
            return itemIndex + 1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return targetTypes.Select(t => Binding.DoNothing).ToArray();
        }
    }
}
