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

        public bool FocusChild()
        {
            var canFocus = VisualTreeHelper.GetChildrenCount(ElementContentPresenter) > 0;
            if (canFocus)
            {
                ((UIElement) VisualTreeHelper.GetChild(ElementContentPresenter, 0)).Focus();
            }

            return canFocus;
        }
        public void Initialize(object v)
        {
            DataContext = v;
        }
    }
}