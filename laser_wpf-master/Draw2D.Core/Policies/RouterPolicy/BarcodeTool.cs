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
    public class BarcodeTool : ToolBase
    {
        private Shapes.Basic.Barcode _barcode;

        private bool _isBarcodeAdded;

        private int _clickCount;
        private bool _isInitialized;
        private BarcodeHandle _barcodeHandle;

        public SelectionFeedbackPolicy SelectionFeedbackPolicy { get; set; } = new SelectionFeedbackPolicy();

        public override void OnMouseMove(Canvas canvas, float mouseX, float mouseY, bool isShiftKey, bool isCtrlKey)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _barcode = new Shapes.Basic.Barcode(0, 0, 0, 0);
                _barcode.SetSnapTargets(SnapTargets.Center);
                _barcodeHandle = new BarcodeHandle(new Ellipse(mouseX, mouseY, 10, 10), _barcode)
                {
                    CanBeSnapTarget = false,
                    FillColor = Colors.Transparent
                };

                foreach (var snapPolicy in canvas.GetSnapPolicies())
                {
                    snapPolicy.InitSnap(canvas, _barcodeHandle.GetSnapPoints(), new[] { _barcodeHandle });
                }

                canvas.Selection.Clear();

                canvas.AddFigure(_barcodeHandle);
            }

            var snapPoint = new Geo.Point(mouseX, mouseY);
            var isSnapped = false;

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                Geo.Point delta;
                isSnapped = snapPolicy.Snap(canvas, snapPoint, 0, 0, mouseX, mouseY, out snapPoint, out delta,
                    new[] { _barcodeHandle });

                if (isSnapped)
                    break;
            }

            if (isSnapped)
            {
                mouseX = snapPoint.X;
                mouseY = snapPoint.Y;
            }

            _barcodeHandle.ForceSetPositionCenter(mouseX, mouseY);


            _barcode.SetPostion(mouseX, mouseY);
        }

        public override void OnMouseLeftDown(Canvas canvas, float mouseX, float mouseY, bool isShiftKey,
            bool isCtrlKey)
        {
            _clickCount++;

            if (_clickCount == 1)
            {
                var dialog = new BarcodeValueDialog("12345");
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    // MessageBox.Show("You entered: " + dialog.UserInput);
                    _barcode.BarcodeValue = dialog.UserInput;
                }
                else
                {
                    MessageBox.Show("Input canceled.");
                    return;
                }

                var snapPoint = new Geo.Point(mouseX, mouseY);
                var isSnapped = false;

                foreach (var snapPolicy in canvas.GetSnapPolicies())
                {
                    Geo.Point delta;
                    isSnapped = snapPolicy.Snap(canvas, new Geo.Point(mouseX, mouseY), 0, 0, mouseX, mouseY, out snapPoint, out delta, new[] { _barcodeHandle });

                    if (isSnapped)
                        break;
                }

                if (isSnapped)
                {
                    mouseX = snapPoint.X;
                    mouseY = snapPoint.Y;
                }

                _barcode.SetPostion(mouseX, mouseY);

            }

            if (_clickCount == 2)
            {
                ExecuteOnDone(canvas);
            }

            if (!_isBarcodeAdded)
            {
                _barcode.ForceSetDimensions(new Geo.Rectangle(mouseX, mouseY, 200, 150));
                _barcode.AddHandlesCornerDirections(canvas, HandleSizes.Small, HandleShapeType.Round);
                _barcode.InstallEditPolicy(SelectionFeedbackPolicy);
                canvas.AddFigure(_barcode);
                _isBarcodeAdded = true;
            }
        }

        private void ExecuteOnDone(Canvas canvas)
        {
            InstallHandles();
            _barcode.Select();

            canvas.RemoveFigure(_barcodeHandle.HandleShape);//Todo: Check this.
            canvas.RemoveFigure(_barcodeHandle);

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                snapPolicy.EndSnapping(canvas);
            }

            OnDone(this);
        }

        private void InstallHandles()
        {

            _barcode.InstallLineHandle();

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

            canvas.RemoveFigure(_barcode);
            canvas.RemoveFigure(_barcodeHandle.HandleShape); //Todo: Check this
            canvas.RemoveFigure(_barcodeHandle);

            _isInitialized = false;
        }

        public override void Reroute(IReadOnlyList<Infini.Draw2D.Core.Geo.Point> points)
        {

        }
    }
}
