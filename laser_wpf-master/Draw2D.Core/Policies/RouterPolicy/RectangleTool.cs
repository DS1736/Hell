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

namespace Infini.Draw2D.Core.Policies.RouterPolicy
{
    public class RectangleTool : ToolBase
    {
        private Shapes.Basic.Rectangle _rectangle;

        private bool _isRectangleAdded;

        private int _clickCount;
        private bool _isInitialized;
        private RectangleHandle _rectangleHandle;

        public SelectionFeedbackPolicy SelectionFeedbackPolicy { get; set; } = new SelectionFeedbackPolicy();

        public override void OnMouseMove(Canvas canvas, float mouseX, float mouseY, bool isShiftKey, bool isCtrlKey)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _rectangle = new Shapes.Basic.Rectangle(0, 0, 0, 0) { FillColor = Colors.Transparent };
                _rectangle.SetSnapTargets(SnapTargets.Center);
                _rectangleHandle = new RectangleHandle(new Ellipse(mouseX, mouseY, 10, 10), _rectangle)
                {
                    CanBeSnapTarget = false,
                    FillColor = Colors.Transparent
                };

                foreach (var snapPolicy in canvas.GetSnapPolicies())
                {
                    snapPolicy.InitSnap(canvas, _rectangleHandle.GetSnapPoints(), new[] { _rectangleHandle });
                }

                canvas.Selection.Clear();

                canvas.AddFigure(_rectangleHandle);
            }

            var snapPoint = new Point(mouseX, mouseY);
            var isSnapped = false;

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                Point delta;
                isSnapped = snapPolicy.Snap(canvas, snapPoint, 0, 0, mouseX, mouseY, out snapPoint, out delta,
                    new[] { _rectangleHandle });

                if (isSnapped)
                    break;
            }

            if (isSnapped)
            {
                mouseX = snapPoint.X;
                mouseY = snapPoint.Y;
            }

            _rectangleHandle.ForceSetPositionCenter(mouseX, mouseY);


            _rectangle.SetPostion(mouseX, mouseY);
        }

        public override void OnMouseLeftDown(Canvas canvas, float mouseX, float mouseY, bool isShiftKey,
            bool isCtrlKey)
        {
            _clickCount++;

            if (_clickCount == 1)
            {
                var snapPoint = new Point(mouseX, mouseY);
                var isSnapped = false;

                foreach (var snapPolicy in canvas.GetSnapPolicies())
                {
                    Point delta;
                    isSnapped = snapPolicy.Snap(canvas, new Point(mouseX, mouseY), 0, 0, mouseX, mouseY, out snapPoint, out delta, new[] { _rectangleHandle });

                    if (isSnapped)
                        break;
                }

                if (isSnapped)
                {
                    mouseX = snapPoint.X;
                    mouseY = snapPoint.Y;
                }

                _rectangle.SetPostion(mouseX, mouseY);
                
            }

            if (_clickCount == 2)
            {
                ExecuteOnDone(canvas);
            }

            if (!_isRectangleAdded)
            {
                _rectangle.ForceSetDimensions(new Geo.Rectangle(mouseX, mouseY, 200, 150));
                _rectangle.AddHandlesCornerDirections(canvas, HandleSizes.Small, HandleShapeType.Round);
                _rectangle.InstallEditPolicy(SelectionFeedbackPolicy);
                canvas.AddFigure(_rectangle);
                _isRectangleAdded = true;
            }
        }

        private void ExecuteOnDone(Canvas canvas)
        {
            InstallHandles();
            _rectangle.Select();

            canvas.RemoveFigure(_rectangleHandle.HandleShape);//Todo: Check this.
            canvas.RemoveFigure(_rectangleHandle);

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                snapPolicy.EndSnapping(canvas);
            }

            OnDone(this);
        }

        private void InstallHandles()
        {

            _rectangle.InstallLineHandle();

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

            canvas.RemoveFigure(_rectangle);
            canvas.RemoveFigure(_rectangleHandle.HandleShape); //Todo: Check this
            canvas.RemoveFigure(_rectangleHandle);

            _isInitialized = false;
        }

        public override void Reroute(IReadOnlyList<Point> points)
        {

        }
    }
}
