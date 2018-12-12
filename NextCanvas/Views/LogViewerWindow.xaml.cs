using System.Windows;

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour LogViewerWindow.xaml
    /// </summary>
    public partial class LogViewerWindow : Window
    {
        public LogViewerWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Wow hard code
        }
    }
}
