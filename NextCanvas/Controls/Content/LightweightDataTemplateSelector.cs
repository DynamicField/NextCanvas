using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NextCanvas.Controls.Content
{
    class LightweightDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NormalRenderDataTemplate { get; set; }
        public DataTemplate LightweightRenderDataTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;
            if ((bool) container.GetValue(NextInkCanvas.IsHostedInLightweightRenderingProperty))
            {
                return LightweightRenderDataTemplate;
            }
            else
            {
                return NormalRenderDataTemplate;
            }
        }
    }
}
