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
    /// Logique d'interaction pour MultiCanvasExperimentWindow.xaml
    /// </summary>
    public partial class MultiCanvasExperimentWindow : Window
    {
        public MultiCanvasExperimentWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foregroundCanvas.EditingMode = backgroundCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foregroundCanvas.EditingMode = backgroundCanvas.EditingMode = InkCanvasEditingMode.Select;
        }
    }
}
