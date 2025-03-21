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
    public class EllipseTool : ToolBase
    {
        private Shapes.Basic.Ellipse _ellipse;

        private bool _isEllipseAdded;

        private int _clickCount;
        private bool _isInitialized;
        private EllipseHandle _ellipseHandle;

        public SelectionFeedbackPolicy SelectionFeedbackPolicy { get; set; } = new SelectionFeedbackPolicy();

        public override void OnMouseMove(Canvas canvas, float mouseX, float mouseY, bool isShiftKey, bool isCtrlKey)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _ellipse = new Shapes.Basic.Ellipse(0, 0, 0, 0) { FillColor = Colors.Transparent };
                _ellipse.SetSnapTargets(SnapTargets.Center);
                _ellipseHandle = new EllipseHandle(new Ellipse(mouseX, mouseY, 10, 10), _ellipse)
                {
                    CanBeSnapTarget = false,
                    FillColor = Colors.Transparent
                };

                foreach (var snapPolicy in canvas.GetSnapPolicies())
                {
                    snapPolicy.InitSnap(canvas, _ellipseHandle.GetSnapPoints(), new[] { _ellipseHandle });
                }

                canvas.Selection.Clear();

                canvas.AddFigure(_ellipseHandle);
            }

            var snapPoint = new Point(mouseX, mouseY);
            var isSnapped = false;

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                Point delta;
                isSnapped = snapPolicy.Snap(canvas, snapPoint, 0, 0, mouseX, mouseY, out snapPoint, out delta,
                    new[] { _ellipseHandle });

                if (isSnapped)
                    break;
            }

            if (isSnapped)
            {
                mouseX = snapPoint.X;
                mouseY = snapPoint.Y;
            }

            _ellipseHandle.ForceSetPositionCenter(mouseX, mouseY);


            _ellipse.SetPostion(mouseX, mouseY);
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
                    isSnapped = snapPolicy.Snap(canvas, new Point(mouseX, mouseY), 0, 0, mouseX, mouseY, out snapPoint, out delta, new[] { _ellipseHandle });

                    if (isSnapped)
                        break;
                }

                if (isSnapped)
                {
                    mouseX = snapPoint.X;
                    mouseY = snapPoint.Y;
                }

                _ellipse.SetPostion(mouseX, mouseY);
                
            }

            if (_clickCount == 2)
            {
                ExecuteOnDone(canvas);
            }

            if (!_isEllipseAdded)
            {
                _ellipse.ForceSetDimensions(new Geo.Rectangle(mouseX, mouseY, 200, 150));
                _ellipse.AddHandlesCornerDirections(canvas, HandleSizes.Small, HandleShapeType.Round);
                _ellipse.InstallEditPolicy(SelectionFeedbackPolicy);
                canvas.AddFigure(_ellipse);
                _isEllipseAdded = true;
            }
        }

        private void ExecuteOnDone(Canvas canvas)
        {
            InstallHandles();
            _ellipse.Select();

            canvas.RemoveFigure(_ellipseHandle.HandleShape);//Todo: Check this.
            canvas.RemoveFigure(_ellipseHandle);

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                snapPolicy.EndSnapping(canvas);
            }

            OnDone(this);
        }

        private void InstallHandles()
        {

            _ellipse.InstallLineHandle();

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

            canvas.RemoveFigure(_ellipse);
            canvas.RemoveFigure(_ellipseHandle.HandleShape); //Todo: Check this
            canvas.RemoveFigure(_ellipseHandle);

            _isInitialized = false;
        }

        public override void Reroute(IReadOnlyList<Point> points)
        {

        }
    }
}
