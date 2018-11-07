#region

using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Fluent;
using Microsoft.Win32;
using NextCanvas.Interactivity;
using NextCanvas.Interactivity.Multimedia;
using NextCanvas.Interactivity.Progress;
using NextCanvas.Utilities;
using NextCanvas.Utilities.Content;
using NextCanvas.ViewModels;
using NextCanvas.Views.Editor;

#endregion

namespace NextCanvas.Views
{
    /// <summary>
    ///     Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            ModifyProvider = new DelegateInteractionProvider<IModifyObjectInteraction>(() => new ModifyObjectWindow(this));
            InitializeComponent();
            try
            {
                PropertyChangedObject.Contexts.Add(Thread.CurrentThread, SynchronizationContext.Current);
            }
            catch (ArgumentException) { }
            DataContext = new MainWindowViewModel
            {
                ElementCreationContext = CreationContext
            };
            pageViewerFactory = new UniqueWindowFactory<PageCollectionViewer>(() => new PageCollectionViewer((MainWindowViewModel)DataContext));
            Canvas.DefaultDrawingAttributes.FitToCurve = true;
        }

        // Creation context for commands. Nice. Nice. Nice. Nice. Nice. Nice.
        public ElementCreationContext CreationContext => new ElementCreationContext(Canvas, ScrollParent);

        public DelegateInteractionProvider<IProgressInteraction> ProgressProvider =>
            new DelegateInteractionProvider<IProgressInteraction>(() => new ProgressWindow(this));

        public DelegateInteractionProvider<IScreenshotInteraction> ScreenshotProvider =>
            new DelegateInteractionProvider<IScreenshotInteraction>(() => new ScreenshotWindow(this));

        public static DelegateInteractionProvider<IModifyObjectInteraction> ModifyProvider { get; private set; }

        private void ColorGallery_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            if (ColorGallery.SelectedColor is null)
            {
                return;
            }

            Canvas.DefaultDrawingAttributes.Color = ColorGallery.SelectedColor ?? Colors.Black;
            var strokes = Canvas.GetSelectedStrokes();
            foreach (var item in strokes)
            {
                item.DrawingAttributes.Color = ColorGallery.SelectedColor ?? Colors.Black;
            }

            if (strokes.Any())
            {
                ColorGallery.SelectedColor = null;
            }
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
            var vm = (MainWindowViewModel)DataContext;
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
            var vm = (MainWindowViewModel)DataContext;
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

        private void InsertClick_GoToHome(object sender, RoutedEventArgs e)
        {
            GoHome();
        }

        private void GoHome()
        {
            Ribbon.SelectedTabIndex = 0;
        }

        private void NewImage_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;
            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.bmp, *.png)|*.jpg; *.bmp; *.png"
            };
            if (dialog.ShowDialog() ?? false)
            {
                vm.OpenImagePath = dialog.FileName;
            }
            else
            {
                vm.OpenImagePath = null;
            }
            GoHome();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            new SettingsWindow { Owner = this }.ShowDialog();
        }

        private readonly UniqueWindowFactory<PageCollectionViewer> pageViewerFactory;
        private void PagesTabLauncherClick(object sender, RoutedEventArgs e)
        {
            pageViewerFactory.TryShowWindow();
        }
    }
}