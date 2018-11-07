using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NextCanvas.Controls.Content
{
    public static class WebBrowserHelper
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(WebBrowserHelper), new UIPropertyMetadata(null, SourcePropertyChanged));

        public static string GetSource(DependencyObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }
        public static void SourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is WebBrowser browser)) return;
            var uri = e.NewValue as string;
            browser.Source = !string.IsNullOrEmpty(uri) ? new Uri(uri) : null;
        }
    }
}
