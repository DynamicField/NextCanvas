#region

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
            ((UIElement) VisualTreeHelper.GetChild(ElementContentPresenter, 0)).Focus();
        }
        public void Initialize(object v)
        {
            DataContext = v;
        }
    }
}