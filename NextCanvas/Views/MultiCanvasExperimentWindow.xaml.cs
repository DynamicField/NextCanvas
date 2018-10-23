using System.Windows;
using System.Windows.Controls;

namespace NextCanvas.Views
{
    /// <summary>
    ///     Logique d'interaction pour MultiCanvasExperimentWindow.xaml
    /// </summary>
    public partial class MultiCanvasExperimentWindow : Window
    {
        public MultiCanvasExperimentWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ForegroundCanvas.EditingMode = BackgroundCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ForegroundCanvas.EditingMode = BackgroundCanvas.EditingMode = InkCanvasEditingMode.Select;
        }
    }
}