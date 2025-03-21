using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
    public class Barcode : VectorFigure
    {

        private VectorFigure _centerHandle;

        private string guid = Guid.NewGuid().ToString();

        public string BarcodeValue { get; set; } = "123456789"; // Default barcode value

        public Barcode(float x, float y, float width, float height)
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


            var barcodeBitmap = GenerateBarcode(BarcodeValue);
            ConvertBitmap(barcodeBitmap);
            if (barcodeBitmap != null)
            {
                var barcodeImage = LoadImageWithoutLock(guid + "-" + "temp.png");

                // Define the area where the image will be drawn
                Rect rect = new Rect(X, Y, barcodeImage.Width, barcodeImage.Height);
                dc.DrawImage(barcodeImage, rect);
                dc.Pop();
            }
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

            _centerHandle?.ForceSetPositionOfCenter(BoundingBox.Center);
        }

        public override void OnDragEnd(Canvas canvas, bool isShiftKey, bool isCtrlKey)
        {
            base.OnDragEnd(canvas, isShiftKey, isCtrlKey);
            canvas.RemoveFigure(_centerHandle);
            _centerHandle = null;
        }

        public void InstallLineHandle()
        {
            var handle = new BarcodeHandle(new Ellipse(this.X, this.Y, 10, 10), this);
            handle.SetSnapTargets(SnapTargets.Center);

            AddHandle(handle);
        }

        private Bitmap GenerateBarcode(string data)
        {
            var writer = new ZXing.BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = (int)Height,
                    Width = (int)Width,
                    Margin = 0
                }
            };
            return writer.Write(data);
        }


        // Convert System.Drawing.Bitmap to BitmapImage (WPF-friendly)
        private BitmapImage ConvertBitmap(System.Drawing.Bitmap src)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                src.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Make it read-only
                // Save the bitmap to a file.
                SaveBitmapImageToFile(bitmapImage, "temp.png");
                return bitmapImage;
            }
        }

        public void SaveBitmapImageToFile(BitmapImage bitmapImage, string filePath)
        {
            if (bitmapImage == null)
                throw new ArgumentNullException(nameof(bitmapImage));

            // Choose an encoder based on file extension
            BitmapEncoder encoder;
            string extension = System.IO.Path.GetExtension(filePath).ToLower();
            string fileName = System.IO.Path.GetFileName(filePath);

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    encoder = new JpegBitmapEncoder();
                    break;
                case ".png":
                    encoder = new PngBitmapEncoder();
                    break;
                case ".bmp":
                    encoder = new BmpBitmapEncoder();
                    break;
                case ".gif":
                    encoder = new GifBitmapEncoder();
                    break;
                case ".tiff":
                    encoder = new TiffBitmapEncoder();
                    break;
                default:
                    throw new NotSupportedException("File format not supported.");
            }

            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (var fileStream = new FileStream(guid + "-" + filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }
}
