using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Fluent;
using Microsoft.Win32;
using NextCanvas.Controls;
using NextCanvas.Controls.Content;
using NextCanvas.ViewModels;

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow, INotifyPropertyChanged
    {
        public ElementCreationContext CreationContext => new ElementCreationContext(Canvas.SelectionHelper,
            ScrollParent.ContentHorizontalOffset, ScrollParent.ContentVerticalOffset, ScrollParent.ActualWidth,
            ScrollParent.ActualHeight);
        public MainWindow()
        {
            
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            Canvas.DefaultDrawingAttributes.FitToCurve = true;
            ApplicationCommands.Paste.CanExecuteChanged += Paste_CanExecuteChanged;
            ScrollParent.SizeChanged += ScrollParent_SizeChanged;
            ScrollParent.ScrollChanged += ScrollParent_ScrollChanged;
        }

        private void ScrollParent_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            CreationContextChanged();
        }

        private void ScrollParent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreationContextChanged();
        }

        private void CreationContextChanged()
        {
            OnPropertyChanged(nameof(CreationContext));
        }

        // The following is a little dirty hack because there you can't really paste to the center or just get the ApplicationCommands.Paste can execute binding
        // with the InkCanvas as a target which is dumb but whatever
        private void Paste_CanExecuteChanged(object sender, EventArgs e)
        {
            PasteButton.IsEnabled = Canvas.CanPaste();
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

        // TODO: Replace this with smth inside the NextInkCanvas class.
        private void PasteButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Paste to some kind of "center", because the default is dumb (0,0).            
            Canvas.Paste(new Point(ScrollParent.ContentHorizontalOffset + ScrollParent.ActualWidth / 2, ScrollParent.ContentVerticalOffset + ScrollParent.ActualHeight / 2));            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
