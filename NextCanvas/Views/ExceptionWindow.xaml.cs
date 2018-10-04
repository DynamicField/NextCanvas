using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            System.Diagnostics.Process.Start("https://github.com/jeuxjeux20/NextCanvas/issues");
            Clipboard.SetText(Info.Text);
            MessageBox.Show("Error data has been succesfully copied to your clipboard.", "Error details copied", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
