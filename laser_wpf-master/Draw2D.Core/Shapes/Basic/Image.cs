using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Infini.Draw2D.Core.Handles;
using Infini.Draw2D.Core.Layout;
using Infini.Draw2D.Core.Utlils;
using ZXing;
using ZXing.Presentation;
using ZXing.Rendering;

namespace Infini.Draw2D.Core.Shapes.Basic
{
    public class Image : VectorFigure
    {

        private VectorFigure _centerHandle;

        private string guid = Guid.NewGuid().ToString();

        public string ImagePath { get; set; } = ""; // Default image path value

        public string OriginalFileName { get; set; } = "";

        public Image(float x, float y, float width, float height)
        {
            SetSnapTargets(SnapTargets.MidPoints | SnapTargets.Center);
        }

        public override void Render(DrawingContext dc)
        {
            var screenPoint = Canvas.CoordinateSystem.ToScreenSpace(Position);
            var offset = new System.Windows.Point((float)screenPoint[0] - X, (float)screenPoint[1] - Y - Height);
            var strokeBrush = new SolidColorBrush(StrokeColor);
            strokeBrush.Freeze();
            var pen = new System.Windows.Media.Pen(strokeBrush, StrokeThickness)
            {
                DashStyle = DashStyle
            };
            pen.Freeze();
            var fillBrush = new SolidColorBrush(FillColor);
            fillBrush.Freeze();
            dc.PushTransform(new TranslateTransform(offset.X, offset.Y));

            var imageBitmap = ReadImage(ImagePath);            
            if (imageBitmap != null)
            {
                // ResizeBmp(OriginalFileName, System.IO.Path.GetFileNameWithoutExtension(OriginalFileName) + "_resized.bmp", (int)Width, (int)Height);
                var img = LoadImageWithoutLock(ImagePath);

                // Define the area where the image will be drawn
                Rect rect = new Rect(X, Y, Width, Height);
                dc.DrawImage(img, rect);
                dc.Pop();
            }
        }

        // Display the image in the Image control
        private BitmapImage ReadImage(string filename)
        {
            if(filename == "")
            {
                return null;
            }

            BitmapImage bitmap = new BitmapImage();

            using (var fileStream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            using (var memoryStream = new System.IO.MemoryStream())
            {
                fileStream.CopyTo(memoryStream); // Copy file content to memory
                memoryStream.Position = 0; // Reset position to start

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Ensure it's fully loaded into memory
                bitmap.StreamSource = memoryStream;
                bitmap.EndInit();
                bitmap.Freeze(); // Make it immutable and thread-safe
            }

            // MyBmp.Source = bitmap;
            return bitmap;
        }

        public BitmapImage LoadImageWithoutLock(string filePath)
        {
            byte[] imageData = File.ReadAllBytes(filePath);

            using (var ms = new MemoryStream(imageData))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze(); // Make it read-only
                return bitmap;
            }
        }

        public override bool OnDragStart(Canvas canvas, float x, float y)
        {
            base.OnDragStart(canvas, x, y);

            if (_centerHandle == null)
            {
                _centerHandle = new Cross(BoundingBox.Center.X, BoundingBox.Center.Y, 10, 10)
                {
                    IsSelectable = false,
                    IsDragable = false,
                    FillColor = Colors.Transparent,
                    CanBeSnapTarget = false

                };
                canvas.AddFigure(_centerHandle);
            }
            _centerHandle.BringToFront();


            return true;
        }

        public override void OnDrag(Canvas canvas, float dxSum, float dySum, float dx, float dy, bool isShiftKey, bool isCtrlKey)
        {
            base.OnDrag(canvas, dxSum, dySum, dx, dy, isShiftKey, isCtrlKey);
            ResizeBmp(OriginalFileName, System.IO.Path.GetFileNameWithoutExtension(OriginalFileName) + "_resized.bmp", (int)Width, (int)Height);
            // ImagePath = "temp.png";
            _centerHandle?.ForceSetPositionOfCenter(BoundingBox.Center);
        }

        private void ResizeBmp(string filename, string newFilename, int width, int height)
        {
            BitmapImage bitmap = new BitmapImage();
            // Load the image into memory to avoid file locking issues
            using (var stream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Fully load the image into memory
                bitmap.StreamSource = stream;
                bitmap.DecodePixelWidth = width;  // Resize while loading
                bitmap.DecodePixelHeight = height;
                bitmap.EndInit();
            }

            bitmap.Freeze(); // Freezes for multi-threaded access


            // Resize using TransformedBitmap
            TransformedBitmap transformedBitmap = new TransformedBitmap(bitmap,
                new System.Windows.Media.ScaleTransform((double)width / bitmap.PixelWidth, (double)height / bitmap.PixelHeight));

            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(transformedBitmap));

            // Save to new file
            using (var fileStream = new System.IO.FileStream(newFilename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                encoder.Save(fileStream);
            }
            ImagePath = newFilename;
        }

        public override void OnDragEnd(Canvas canvas, bool isShiftKey, bool isCtrlKey)
        {
            base.OnDragEnd(canvas, isShiftKey, isCtrlKey);
            canvas.RemoveFigure(_centerHandle);
            _centerHandle = null;
        }

        public void InstallLineHandle()
        {
            var handle = new ImageHandle(new Ellipse(this.X, this.Y, 10, 10), this);
            handle.SetSnapTargets(SnapTargets.Center);

            AddHandle(handle);
        }                
    }
}
