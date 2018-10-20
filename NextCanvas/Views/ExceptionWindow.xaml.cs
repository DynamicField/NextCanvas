using System.Diagnostics;
using System.Windows;

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionWindow : Window
    {
        public ExceptionWindow()
        {
            InitializeComponent();
        }
        public ExceptionWindow(string exception) : this()
        {
            Info.Text = exception;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/jeuxjeux20/NextCanvas/issues");
            Clipboard.SetText(Info.Text);
            MessageBox.Show("Error data has been succesfully copied to your clipboard.", "Error details copied", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
