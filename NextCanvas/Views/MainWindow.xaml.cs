using Fluent;
using Microsoft.Win32;
using NextCanvas.ViewModels;
using NextCanvas.Views;
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
            ApplicationCommands.Paste.CanExecuteChanged += Paste_CanExecuteChanged;
        }
        // The following is a little dirty hack because there you can't really paste to the center or just get the ApplicationCommands.Paste can execute binding
        // with the InkCanvas as a target which is dumb but whatever
        private void Paste_CanExecuteChanged(object sender, EventArgs e)
        {
            PasteButton.IsEnabled = canvas.CanPaste();
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
            new StylusDebugWindow { Owner = this }.Show();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            var dialog = new SaveFileDialog
            {
                Filter = "NextCanvas document (*.ncd)|*.ncd"
            };
            if (dialog.ShowDialog() ?? false)
            {
                vm.SavePath = dialog.FileName;
            }
            else
            {
                vm.SavePath = null;
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            var dialog = new OpenFileDialog
            {
                Filter = "NextCanvas document (*.ncd)|*.ncd"
            };
            if (dialog.ShowDialog() ?? false)
            {
                vm.OpenPath = dialog.FileName;
            }
            else
            {
                vm.OpenPath = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MultiCanvasExperimentWindow().Show();
        }

        private void PasteButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Paste to some kind of "center", because the default is dumb (0,0).            
            canvas.Paste(new Point(ScrollParent.ContentHorizontalOffset + ScrollParent.ActualWidth / 2, ScrollParent.ContentVerticalOffset + ScrollParent.ActualHeight / 2));            
        }
    }
}
