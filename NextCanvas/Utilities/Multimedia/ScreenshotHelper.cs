using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Size = System.Drawing.Size;

namespace NextCanvas.Utilities.Multimedia
{
    public static class ScreenshotHelper
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject([In] IntPtr hObject);

        public static BitmapSource TakeScreenshot(Screen screen)
        {
            var boundsHeight = screen.Bounds.Height;
            var boundsWidth = screen.Bounds.Width;
            using (var screenBmp = new Bitmap(boundsWidth, boundsHeight, PixelFormat.Format32bppArgb))
            {
                using (var bmpGraphics = Graphics.FromImage(screenBmp))
                {
                    bmpGraphics.CopyFromScreen(screen.Bounds.Left, screen.Bounds.Top, 0, 0,
                        new Size(boundsWidth, boundsHeight));
                    var bitmap = screenBmp.GetHbitmap();
                    try
                    {
                        return Imaging.CreateBitmapSourceFromHBitmap(bitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                    }
                    finally
                    {
                        DeleteObject(bitmap);
                    }
                }
            }
        }

        /// <summary>
        ///     Takes a screenshot on the screen showing the most of the window.
        /// </summary>
        /// <param name="windowCoordinates">The window to compare to.</param>
        /// <returns>Your DAMN SCREENSHOT YOU WAITED FOR !!</returns>
        public static BitmapSource TakeScreenshot(Window windowCoordinates)
        {
            if (Screen.AllScreens.Length == 1) return TakeScreenshot(); // Primary screen
            var screenPortions = new Dictionary<Screen, double>();
            foreach (var screen in Screen.AllScreens.OrderBy(s => s.Bounds.Left)) // 0 --> max
            {
                // TODO: Better math.
                double score = 0;
                score += windowCoordinates.ActualWidth + windowCoordinates.Left;
                score += screen.Bounds.Right - windowCoordinates.Left;
                screenPortions.Add(screen, score);
            }

            return TakeScreenshot(screenPortions.OrderByDescending(k => k.Value).First().Key);
        }

        public static BitmapSource TakeScreenshot()
        {
            return TakeScreenshot(Screen.PrimaryScreen);
        }
    }
}