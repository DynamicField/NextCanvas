using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace NextCanvas.Controls.Content
{
    public static class WebBrowserHelper
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(WebBrowserHelper), new UIPropertyMetadata(null, SourcePropertyChanged));
        public static readonly DependencyProperty IgnoreErrorsProperty =
            DependencyProperty.RegisterAttached("IgnoreErrors", typeof(bool), typeof(WebBrowserHelper), new UIPropertyMetadata(false, IgnoreErrorsPropertyChanged));

        public static string GetSource(DependencyObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }

        public static bool GetIgnoreErrors(DependencyObject obj) => (bool) obj.GetValue(IgnoreErrorsProperty);
        public static void SetIgnoreErrors(DependencyObject obj, bool value) => obj.SetValue(SourceProperty, value);

        public static void SourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is WebBrowser browser)) return;
            var uri = e.NewValue as string;
            browser.Source = !string.IsNullOrEmpty(uri) ? new Uri(uri) : null;
        }

        public static void IgnoreErrorsPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is WebBrowser browser)) return;
            var newBool = (bool) e.NewValue;
            if ((bool) e.OldValue != newBool)
            {
                HideScriptErrors(browser, newBool);
            }
        }
        public static void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }
    }
}
