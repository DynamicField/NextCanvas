#region

using System.Diagnostics;
using System.Windows;
using NextCanvas.Properties;

#endregion

namespace NextCanvas.Views
{
    /// <summary>
    ///     Logique d'interaction pour ExceptionWindow.xaml
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
            MessageBox.Show(ErrorResources.Exception_ClipboardCopySuccessfulContent, ErrorResources.Exception_ClipboardCopySuccessfulTitle,
                MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}