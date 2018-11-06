using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using NextCanvas.Interactivity.Multimedia;
using NextCanvas.Utilities.Multimedia;

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

        private Point clickPosition;

        private bool isCapturing;

        public ScreenshotWindow(Window owner)
        {
            Owner = owner;
            owner.WindowState = WindowState.Minimized;
            owner.Hide();
            FullScreenshot = ScreenshotHelper.TakeScreenshot(true);
            owner.Show();
            InitializeComponent();
        }

        public ScreenshotWindow()
        {
            FullScreenshot = ScreenshotHelper.TakeScreenshot(true);
            InitializeComponent();
        }

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
            isCapturing = true; // to not focus after autoclose
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

            base.CloseInteraction();
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
            isCapturing = true;
            var position = e.GetPosition(CropCanvas);
            clickPosition = position;
            Canvas.SetLeft(SelectionRectangle, position.X);
            Canvas.SetTop(SelectionRectangle, position.Y);
            SetRectangleCrop();
        }

        private void CropCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isCapturing || e.LeftButton == MouseButtonState.Released) return;
            var position = e.GetPosition(CropCanvas);
            Canvas.SetLeft(SelectionRectangle, clickPosition.X < position.X ? clickPosition.X : position.X);
            Canvas.SetTop(SelectionRectangle, clickPosition.Y < position.Y ? clickPosition.Y : position.Y);
            SelectionRectangle.Width = Math.Abs(clickPosition.X - position.X);
            SelectionRectangle.Height = Math.Abs(clickPosition.Y - position.Y);
            SetRectangleCrop();
        }

        private void SetRectangleCrop()
        {
            var rect = new Int32Rect
            {
                X = (int) Canvas.GetLeft(SelectionRectangle),
                Y = (int) Canvas.GetTop(SelectionRectangle),
                Width = (int) Math.Max(SelectionRectangle.Width, 1),
                Height = (int) Math.Max(SelectionRectangle.Height, 1)
            };
            Geometry.Rect = new Rect
            {
                X = rect.X,
                Y = rect.Y,
                Width = rect.Width,
                Height = rect.Height
            };
            RectangleCrop = rect;
        }

        private void MainGrid_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isCapturing) return;
            ((IInputElement) CropCanvas).ReleaseMouseCapture();
            ActionComplete?.Invoke(this,
                new ScreenshotTakenEventArgs(new CroppedBitmap(FullScreenshot, RectangleCrop)));
            Close();
        }

        private async void ScreenshotWindow_OnContentRendered(object sender, EventArgs e)
        {
            await Task.Delay(TimeBeforeAutoClose);
            if (!isCapturing) Close();
        }
    }
}