using System.Windows;
using System.Windows.Controls;

namespace NextCanvas.Controls.Editor
{
    /// <summary>
    /// Logique d'interaction pour ControlElementEditorControl.xaml
    /// </summary>
    public partial class ControlElementEditorControl : UserControl
    {
        public ControlElementEditorControl()
        {
            InitializeComponent();
        }

        public bool ShowCoordinates
        {
            get { return (bool)GetValue(ShowCoordinatesProperty); }
            set { SetValue(ShowCoordinatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowCoordinates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCoordinatesProperty =
            DependencyProperty.Register("ShowCoordinates", typeof(bool), typeof(ControlElementEditorControl), new PropertyMetadata(true));

    }
}
