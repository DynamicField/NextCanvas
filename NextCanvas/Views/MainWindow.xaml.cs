using Fluent;
using NextCanvas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NextCanvas
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void SelectTrigger(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.Select;
        }

        private void WriteTrigger(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void EraseTrigger(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            canvas.EraserShape = new RectangleStylusShape(5, 5);
        }

        private void Canvas_SelectionChanged(object sender, EventArgs e)
        {
            if (canvas.GetSelectedElements().Count != 0 || canvas.GetSelectedStrokes().Count != 0)
            {
                CopyButton.IsEnabled = CutButton.IsEnabled = true;
            }
            else
            {
                CopyButton.IsEnabled = CutButton.IsEnabled = false;
            }
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            canvas.Paste(new Point(canvas.Width / 2, canvas.Height / 2));
        }
    }
}
