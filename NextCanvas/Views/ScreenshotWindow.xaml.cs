#region

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using NextCanvas.Interactivity.Multimedia;
using NextCanvas.Utilities.Multimedia;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

#endregion

namespace NextCanvas.Views
{
    /// <summary>
    ///     Logique d'interaction pour ScreenshotWindow.xaml
    /// </summary>
    public partial class ScreenshotWindow : InteractionWindow, IScreenshotInteraction
    {
        private const int TimeBeforeAutoClose = 15000;

        // Using a DependencyProperty as the backing store for fullScreenShot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FullScreenshotProperty =
            DependencyProperty.Register("FullScreenshot", typeof(BitmapSource), typeof(ScreenshotWindow),
                new FrameworkPropertyMetadata());

        // Using a DependencyProperty as the backing store for RectangleCrop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectangleCropProperty =
            DependencyProperty.Register("RectangleCrop", typeof(Int32Rect), typeof(ScreenshotWindow),
                new FrameworkPropertyMetadata(new Int32Rect()));

        private Point _clickPosition;

        private bool _isCapturing;

        public ScreenshotWindow(Window owner = null)
        {
            Initialize(owner);
            InitializeComponent();
        }
        private void Initialize(Window owner = null)
        {
            if (owner != null)
            {
                Owner = owner;
                owner.WindowState = WindowState.Minimized;
                owner.Hide();
                FullScreenshot = ScreenshotHelper.TakeScreenshot(true, out _usedScreen);
                owner.Show();
            }
            else
            {
                FullScreenshot = ScreenshotHelper.TakeScreenshot(true, out _usedScreen);
            }
            LogManager.AddLogItem($"UsedScreen: left: {_usedScreen.Bounds.Left}", "Screenshot.Initialize");
            Left = _usedScreen.Bounds.Left; // ensure multi screen
            Top = 0;
            Width = 1; // This is a trick to reduce the maximizing visuals.
            Height = 1;
            WindowState = WindowState.Maximized;
#if DEBUG
            LogManager.AddLogItem($"My left is: {Left}");
#endif
        }
        private Screen _usedScreen;
        public BitmapSource FullScreenshot
        {
            get => (BitmapSource) GetValue(FullScreenshotProperty);
            set => SetValue(FullScreenshotProperty, value);
        }


        public Int32Rect RectangleCrop
        {
            get => (Int32Rect) GetValue(RectangleCropProperty);
            set => SetValue(RectangleCropProperty, value);
        }


        public event EventHandler<ScreenshotTakenEventArgs> ActionComplete;
        public event EventHandler ActionCanceled;

        private void ScreenshotWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) CancelScreenshot();
        }

        private void CancelScreenshot()
        {
            _isCapturing = true; // to not focus after autoclose
            ActionCanceled?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private new void Close()
        {
            if (Owner != null)
            {
                Owner.WindowState = WindowState.Normal;
                Owner.Focus();
            }
            CloseInteraction();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            CancelScreenshot();
        }

        private void CaptureWholeScreenClick(object sender, RoutedEventArgs e)
        {
            ActionComplete?.Invoke(this, new ScreenshotTakenEventArgs(FullScreenshot));
            Close();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(MainGrid);
            ScreenshotOptions.Visibility = Visibility.Collapsed;
            _isCapturing = true;
            var position = e.GetPosition(CropCanvas);
            _clickPosition = position;
            Canvas.SetLeft(SelectionRectangle, position.X);
            Canvas.SetTop(SelectionRectangle, position.Y);
            SetRectangleCrop();
        }

        private void CropCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isCapturing || e.LeftButton == MouseButtonState.Released) return;
            var position = e.GetPosition(CropCanvas);
            Canvas.SetLeft(SelectionRectangle, _clickPosition.X < position.X ? _clickPosition.X : position.X);
            Canvas.SetTop(SelectionRectangle, _clickPosition.Y < position.Y ? _clickPosition.Y : position.Y);
            SelectionRectangle.Width = Math.Abs(_clickPosition.X - position.X);
            SelectionRectangle.Height = Math.Abs(_clickPosition.Y - position.Y);
            SetRectangleCrop();
        }

        private void SetRectangleCrop()
        {
            var rect = new Int32Rect
            {
                X = (int) (Canvas.GetLeft(SelectionRectangle) * DpiRatio),
                Y = (int) (Canvas.GetTop(SelectionRectangle) * DpiRatio),
                Width = (int) (Math.Max(SelectionRectangle.Width, 1) * DpiRatio),
                Height = (int) (Math.Max(SelectionRectangle.Height, 1) * DpiRatio)
            };
            Geometry.Rect = new Rect
            {
                X = rect.X / DpiRatio,
                Y = rect.Y / DpiRatio,
                Width = rect.Width / DpiRatio,
                Height = rect.Height / DpiRatio
            };
            RectangleCrop = rect;
        }
        private double DpiRatio => FullScreenshot.PixelHeight / ActualHeight;
        private void MainGrid_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isCapturing) return;
            ((IInputElement) CropCanvas).ReleaseMouseCapture();
            ActionComplete?.Invoke(this,
                new ScreenshotTakenEventArgs(new CroppedBitmap(FullScreenshot, RectangleCrop)));
            Close();
        }

        private async void ScreenshotWindow_OnContentRendered(object sender, EventArgs e)
        {
            Focus();
            await Task.Delay(TimeBeforeAutoClose);
            if (!_isCapturing) Close();
        }
    }
}