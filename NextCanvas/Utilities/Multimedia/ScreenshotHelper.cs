using System;
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
        ///     Takes a screenshot on the screen.
        /// </summary>
        /// <param name="basedOnCursorPosition">Chooses whether or not the screenshot is taken on the screen the cursor is pointing at</param>
        /// <returns>Your DAMN SCREENSHOT YOU WAITED FOR !!</returns>
        public static BitmapSource TakeScreenshot(bool basedOnCursorPosition = false)
        {
            if (Screen.AllScreens.Length == 1 || !basedOnCursorPosition) return TakeScreenshot(Screen.PrimaryScreen); // Primary screen
            var cursorPosition = Cursor.Position;
            var cursorScreen = Screen.AllScreens.First(screen =>
                screen.Bounds.X <= cursorPosition.X && screen.Bounds.Right >= cursorPosition.X);
            return TakeScreenshot(cursorScreen);
        }
    }
}