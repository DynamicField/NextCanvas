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
