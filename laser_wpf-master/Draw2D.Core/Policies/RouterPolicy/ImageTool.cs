using Infini.Draw2D.Core.Handles;
using Infini.Draw2D.Core.Layout.Connection;
using Infini.Draw2D.Core.Shapes.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infini.Draw2D.Core.Geo;
using System.Threading.Tasks;
using System.Windows.Media;
using Infini.Draw2D.Core.Constants;
using Infini.Draw2D.Core.Policies.FigurePolicy;
using Infini.Draw2D.Core.Shapes.FigureExtensions;
using System.Windows;
using Infini.Draw2D.Core.Dialog;

namespace Infini.Draw2D.Core.Policies.RouterPolicy
{
    public class ImageTool : ToolBase
    {
        private Shapes.Basic.Image _image;

        private bool _isImageAdded;

        private int _clickCount;
        private bool _isInitialized;
        private ImageHandle _imageHandle;

        public SelectionFeedbackPolicy SelectionFeedbackPolicy { get; set; } = new SelectionFeedbackPolicy();

        public override void OnMouseMove(Canvas canvas, float mouseX, float mouseY, bool isShiftKey, bool isCtrlKey)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _image = new Shapes.Basic.Image(0, 0, 0, 0);
                _image.SetSnapTargets(SnapTargets.Center);
                _imageHandle = new ImageHandle(new Ellipse(mouseX, mouseY, 10, 10), _image)
                {
                    CanBeSnapTarget = false,
                    FillColor = Colors.Transparent
                };

                foreach (var snapPolicy in canvas.GetSnapPolicies())
                {
                    snapPolicy.InitSnap(canvas, _imageHandle.GetSnapPoints(), new[] { _imageHandle });
                }

                canvas.Selection.Clear();

                canvas.AddFigure(_imageHandle);
            }

            var snapPoint = new Geo.Point(mouseX, mouseY);
            var isSnapped = false;

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                Geo.Point delta;
                isSnapped = snapPolicy.Snap(canvas, snapPoint, 0, 0, mouseX, mouseY, out snapPoint, out delta,
                    new[] { _imageHandle });

                if (isSnapped)
                    break;
            }

            if (isSnapped)
            {
                mouseX = snapPoint.X;
                mouseY = snapPoint.Y;
            }

            _imageHandle.ForceSetPositionCenter(mouseX, mouseY);


            _image.SetPostion(mouseX, mouseY);
        }

        public override void OnMouseLeftDown(Canvas canvas, float mouseX, float mouseY, bool isShiftKey,
            bool isCtrlKey)
        {
            _clickCount++;

            if (_clickCount == 1)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".bmp";
                dlg.Filter = "Bitmap Files (*.bmp)|*.bmp";

                // Get the selected file name and display in a TextBox
                if (dlg.ShowDialog() == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    _image.ImagePath = filename;
                    _image.OriginalFileName = filename;
                }

                var snapPoint = new Geo.Point(mouseX, mouseY);
                var isSnapped = false;

                foreach (var snapPolicy in canvas.GetSnapPolicies())
                {
                    Geo.Point delta;
                    isSnapped = snapPolicy.Snap(canvas, new Geo.Point(mouseX, mouseY), 0, 0, mouseX, mouseY, out snapPoint, out delta, new[] { _imageHandle });

                    if (isSnapped)
                        break;
                }

                if (isSnapped)
                {
                    mouseX = snapPoint.X;
                    mouseY = snapPoint.Y;
                }

                _image.SetPostion(mouseX, mouseY);

            }

            if (_clickCount == 2)
            {
                ExecuteOnDone(canvas);
            }

            if (!_isImageAdded)
            {
                _image.ForceSetDimensions(new Geo.Rectangle(mouseX, mouseY, 200, 150));
                _image.AddHandlesCornerDirections(canvas, HandleSizes.Small, HandleShapeType.Round);
                _image.InstallEditPolicy(SelectionFeedbackPolicy);
                canvas.AddFigure(_image);
                _isImageAdded = true;
            }
        }

        private void ExecuteOnDone(Canvas canvas)
        {
            InstallHandles();
            _image.Select();

            canvas.RemoveFigure(_imageHandle.HandleShape);//Todo: Check this.
            canvas.RemoveFigure(_imageHandle);

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                snapPolicy.EndSnapping(canvas);
            }

            OnDone(this);
        }

        private void InstallHandles()
        {

            _image.InstallLineHandle();

        }

        public override void OnMouseRightDown(Canvas canvas, float mouseX, float mouseY, bool isShiftKey,
            bool isCtrlKey)
        {
            _clickCount--;
            if (_clickCount == 0)
            {
                Cancel(canvas);
            }

        }

        public override void Cancel(Canvas canvas)
        {
            base.Cancel(canvas);

            canvas.RemoveFigure(_image);
            canvas.RemoveFigure(_imageHandle.HandleShape); //Todo: Check this
            canvas.RemoveFigure(_imageHandle);

            _isInitialized = false;
        }

        public override void Reroute(IReadOnlyList<Infini.Draw2D.Core.Geo.Point> points)
        {

        }
    }
}
