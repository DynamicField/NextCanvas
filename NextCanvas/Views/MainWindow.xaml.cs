using Fluent;
using NextCanvas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

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
            canvas.DefaultDrawingAttributes.FitToCurve = true;
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

        private void CutButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.CutSelection();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.CopySelection();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            StrokeCollection list = canvas.GetSelectedStrokes();
            canvas.Strokes.Remove(list);
            System.Collections.ObjectModel.ReadOnlyCollection<UIElement> elements = canvas.GetSelectedElements();
            if (elements.Any())
            {
                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    canvas.Children.Remove(elements[i]);
                }
            }
            Canvas_SelectionChanged(sender, null);
        }

        private void ColorGallery_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            canvas.DefaultDrawingAttributes.Color = colorGallery.SelectedColor ?? Colors.Black;
            foreach (Stroke item in canvas.GetSelectedStrokes())
            {
                item.DrawingAttributes.Color = colorGallery.SelectedColor ?? Colors.Black;
            }
        }

        private void InRibbonGallery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // canvas.DefaultDrawingAttributes.Color = colorGallery.SelectedColor ?? Colors.Black;
        }

        private void ResizeCanvasEvent(object sender, RoutedEventArgs e)
        {
            canvas.SizeChanged += Canvas_SizeChanged;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                ScrollParent.ScrollToRightEnd();
            }
            if (e.HeightChanged)
            {
                ScrollParent.ScrollToBottom();
            }
            canvas.SizeChanged -= Canvas_SizeChanged;
        }

        private void DebugStylusXD_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
