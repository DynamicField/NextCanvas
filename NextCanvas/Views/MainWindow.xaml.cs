using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Fluent;
using Microsoft.Win32;
using NextCanvas.ViewModels;

namespace NextCanvas.Views
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
            Canvas.DefaultDrawingAttributes.FitToCurve = true;
            ApplicationCommands.Paste.CanExecuteChanged += Paste_CanExecuteChanged;
        }
        // The following is a little dirty hack because there you can't really paste to the center or just get the ApplicationCommands.Paste can execute binding
        // with the InkCanvas as a target which is dumb but whatever
        private void Paste_CanExecuteChanged(object sender, EventArgs e)
        {
            PasteButton.IsEnabled = Canvas.CanPaste();
        }

        private void Canvas_SelectionChanged(object sender, EventArgs e)
        {
            if (Canvas.GetSelectedElements().Count != 0 || Canvas.GetSelectedStrokes().Count != 0)
            {
                CopyButton.IsEnabled = CutButton.IsEnabled = true;
            }
            else
            {
                CopyButton.IsEnabled = CutButton.IsEnabled = false;
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var list = Canvas.GetSelectedStrokes();
            Canvas.Strokes.Remove(list);
            var elements = Canvas.GetSelectedElements();
            if (elements.Any())
            {
                for (var i = elements.Count - 1; i >= 0; i--)
                {
                    Canvas.Children.Remove(elements[i]);
                }
            }
            Canvas_SelectionChanged(sender, null);
        }

        private void ColorGallery_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            Canvas.DefaultDrawingAttributes.Color = ColorGallery.SelectedColor ?? Colors.Black;
            foreach (var item in Canvas.GetSelectedStrokes())
            {
                item.DrawingAttributes.Color = ColorGallery.SelectedColor ?? Colors.Black;
            }
        }

        private void InRibbonGallery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // canvas.DefaultDrawingAttributes.Color = colorGallery.SelectedColor ?? Colors.Black;
        }

        private void ResizeCanvasEvent(object sender, RoutedEventArgs e)
        {
            Canvas.SizeChanged += Canvas_SizeChanged;
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
            Canvas.SizeChanged -= Canvas_SizeChanged;
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
            var vm = (MainWindowViewModel) DataContext;
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
            var vm = (MainWindowViewModel) DataContext;
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
            Canvas.Paste(new Point(ScrollParent.ContentHorizontalOffset + ScrollParent.ActualWidth / 2, ScrollParent.ContentVerticalOffset + ScrollParent.ActualHeight / 2));            
        }
    }
}
