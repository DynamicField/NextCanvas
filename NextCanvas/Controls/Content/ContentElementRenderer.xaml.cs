#region

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace NextCanvas.Controls.Content
{
    /// <summary>
    ///     Logique d'interaction pour ContentElementRenderer.xaml
    /// </summary>
    public partial class ContentElementRenderer : UserControl
    {
        public ContentElementRenderer()
        {
            InitializeComponent();
        }

        public void FocusChild()
        {
            var canFocus = VisualTreeHelper.GetChildrenCount(ElementContentPresenter) > 0;
            if (canFocus)
            {
                ((UIElement) VisualTreeHelper.GetChild(ElementContentPresenter, 0)).Focus();
            }
        }
        public void Initialize(object v)
        {
            DataContext = v;
        }

        private void WebBrowserUnloaded(object sender, RoutedEventArgs e)
        {
            ((WebBrowser) sender).Source = null; // Clear the web browser resources 
            ((WebBrowser)sender).Dispose();
            GC.Collect();
        }
    }
}