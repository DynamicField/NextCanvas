using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace NextCanvas.Interactivity.Multimedia
{
    public class ScreenshotTakenEventArgs : EventArgs
    {
        public ScreenshotTakenEventArgs(BitmapSource bitmap, ImageFormat format = null)
        {
            if (format == null) format = ImageFormat.Png;
            BitmapEncoder encoder = new PngBitmapEncoder();
            if (format.Equals(ImageFormat.Bmp)) encoder = new BmpBitmapEncoder();
            if (format.Equals(ImageFormat.Gif)) encoder = new GifBitmapEncoder();
            if (format.Equals(ImageFormat.Jpeg)) encoder = new JpegBitmapEncoder();
            if (format.Equals(ImageFormat.Tiff)) encoder = new TiffBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            var memoryStream = new MemoryStream();
            encoder.Save(memoryStream);
            ImageData = memoryStream;
            ImageExtension =
                encoder.CodecInfo.FileExtensions.Split(',')
                    [0]; // First extension. Usually the best. But it doesn't matter much. Yes.
        }

        public MemoryStream ImageData { get; }
        public string ImageExtension { get; }
    }
}