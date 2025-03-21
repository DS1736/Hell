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
using Infini.Draw2D.Core.Dialog;
using System.Windows;

namespace Infini.Draw2D.Core.Policies.RouterPolicy
{
    public class TextTool : ToolBase
    {
        private Shapes.Basic.BoxedText _text;

        private bool _isTextAdded;

        private int _clickCount;
        private bool _isInitialized;
        private TextHandle _textHandle;

        public SelectionFeedbackPolicy SelectionFeedbackPolicy { get; set; } = new SelectionFeedbackPolicy();

        public override void OnMouseMove(Canvas canvas, float mouseX, float mouseY, bool isShiftKey, bool isCtrlKey)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _text = new Shapes.Basic.BoxedText("");
                _text.SetSnapTargets(SnapTargets.Center);
                _text.FontSize = 20;
                _textHandle = new TextHandle(new Ellipse(mouseX, mouseY, 10, 10), _text)
                {
                    CanBeSnapTarget = false,
                    FillColor = Colors.Transparent
                };

                foreach (var snapPolicy in canvas.GetSnapPolicies())
                {
                    snapPolicy.InitSnap(canvas, _textHandle.GetSnapPoints(), new[] { _textHandle });
                }

                canvas.Selection.Clear();

                canvas.AddFigure(_textHandle);
            }

            var snapPoint = new Geo.Point(mouseX, mouseY);
            var isSnapped = false;

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                Geo.Point delta;
                isSnapped = snapPolicy.Snap(canvas, snapPoint, 0, 0, mouseX, mouseY, out snapPoint, out delta,
                    new[] { _textHandle });

                if (isSnapped)
                    break;
            }

            if (isSnapped)
            {
                mouseX = snapPoint.X;
                mouseY = snapPoint.Y;
            }

            _textHandle.ForceSetPositionCenter(mouseX, mouseY);


            _text.SetPostion(mouseX, mouseY);
        }

        public override void OnMouseLeftDown(Canvas canvas, float mouseX, float mouseY, bool isShiftKey,
            bool isCtrlKey)
        {
            _clickCount++;

            if (_clickCount == 1)
            {

                var dialog = new BoxedTextValueDialog("TEST");
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    // MessageBox.Show("You entered: " + dialog.UserInput);
                    _text.Message = dialog.UserInput;
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
                    isSnapped = snapPolicy.Snap(canvas, new Geo.Point(mouseX, mouseY), 0, 0, mouseX, mouseY, out snapPoint, out delta, new[] { _textHandle });

                    if (isSnapped)
                        break;
                }

                if (isSnapped)
                {
                    mouseX = snapPoint.X;
                    mouseY = snapPoint.Y;
                }

                _text.SetPostion(mouseX, mouseY);

            }

            if (_clickCount == 2)
            {
                ExecuteOnDone(canvas);
            }

            if (!_isTextAdded)
            {                
                _text.ForceSetDimensions(new Geo.Rectangle(mouseX, mouseY, 200, 100));
                _text.AddHandlesCornerDirections(canvas, HandleSizes.Small, HandleShapeType.Round);
                _text.InstallEditPolicy(SelectionFeedbackPolicy);
                canvas.AddFigure(_text);
                _isTextAdded = true;
            }
        }

        private void ExecuteOnDone(Canvas canvas)
        {
            InstallHandles();
            _text.Select();

            canvas.RemoveFigure(_textHandle.HandleShape);//Todo: Check this.
            canvas.RemoveFigure(_textHandle);

            foreach (var snapPolicy in canvas.GetSnapPolicies())
            {
                snapPolicy.EndSnapping(canvas);
            }

            OnDone(this);
        }

        private void InstallHandles()
        {

            _text.InstallLineHandle();

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

            canvas.RemoveFigure(_text);
            canvas.RemoveFigure(_textHandle.HandleShape); //Todo: Check this
            canvas.RemoveFigure(_textHandle);

            _isInitialized = false;
        }

        public override void Reroute(IReadOnlyList<Geo.Point> points)
        {

        }
    }
}
