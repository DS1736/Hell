using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;

namespace InIWorkspace.Extensions
{
    public static class BitmapExtensions
    {
        public static BitmapImage ToImageSource(this Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Save the Bitmap to the MemoryStream
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                // Reset the stream position to the beginning
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Create a BitmapImage
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Freeze for thread safety

                return bitmapImage;
            }
        }

        public static SKBitmap ToSKBitmap(this Bitmap bitmap)
        {
            // Create a new SKBitmap with the same dimensions as the input Bitmap
            SKBitmap skBitmap = new SKBitmap(bitmap.Width, bitmap.Height, SKColorType.Bgra8888, SKAlphaType.Premul);

            // Lock the bitmap's bits to access raw pixel data
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb); // Ensure it's a compatible format

            try
            {
                // Copy the pixel data from Bitmap to SKBitmap
                skBitmap.InstallPixels(
                    new SKImageInfo(bitmap.Width, bitmap.Height, SKColorType.Bgra8888, SKAlphaType.Premul),
                    bitmapData.Scan0,
                    bitmapData.Stride);
            }
            finally
            {
                // Unlock the bits
                bitmap.UnlockBits(bitmapData);
            }

            return skBitmap;
        }
    }
}
