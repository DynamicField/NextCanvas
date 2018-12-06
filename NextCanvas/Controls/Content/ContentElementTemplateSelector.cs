using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NextCanvas.Controls.Content
{
    class ContentElementTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var resources = ((FrameworkElement) container).Resources;
            var addonTemplates = App.GetCurrent().Addons.SelectMany(a => a.AvailableDataTemplates);
            var final = addonTemplates.Concat(resources.OfType<DataTemplate>());
            return final.FirstOrDefault(d => d.DataType.Equals(item?.GetType()));
        }
    }
}
