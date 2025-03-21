using Infini.Draw2D.Core.Geo;
using Infini.Draw2D.Core.Shapes.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Infini.Draw2D.Core.Handles
{
    internal class TextHandle : VectorFigure, IHandle
    {
        private readonly Shapes.Basic.BoxedText _boxedText;
        public VectorFigure HandleShape { get; set; }

        public Figure Owner { get; }

        public TextHandle(VectorFigure handleShape, Shapes.Basic.BoxedText boxedText)
        {
            _boxedText = boxedText;
            Owner = boxedText;
            HandleShape = handleShape;



            Width = HandleShape.Width;
            Height = handleShape.Height;

            Update();

            handleShape.IsDragable = false;
            handleShape.IsVisible = false;
            handleShape.IsSelectable = false;
            handleShape.CanBeSnapTarget = false;

            IsDragable = true;
            IsVisible = true;
            IsSelectable = true;
            CanBeSnapTarget = false;

            SetSnapTargets(SnapTargets.Center);
        }

        public override void OnDrag(Canvas canvas, float dxSum, float dySum, float dx, float dy, bool isShiftKey, bool isCtrlKey)
        {
            if (!IsDragable)
                return;

            var snapDelta = new Point(dx, dy);
            var isSnapped = false;

            foreach (var policy in canvas.GetSnapPolicies())
            {
                Point snapPoint;
                isSnapped = policy.Snap(canvas, HandleShape.Position, dx, dy, dxSum, dySum, out snapPoint, out snapDelta, new[] { this });

                if (isSnapped)
                    break;
            }

            if (isSnapped)
            {
                dx = snapDelta.X;
                dy = snapDelta.Y;
            }

            _boxedText.Translate(dx, dy);

            Update();


            Canvas?.NeedsRepaint(this);

        }


        public override void ForceSetPositionCenter(float x, float y)
        {
            base.ForceSetPositionCenter(x, y);
            HandleShape.ForceSetPositionOfCenter(x, y);
        }


        public void Show(Canvas canvas)
        {
            canvas.AddAdornerFigure(this);
            BringToFront();
        }

        public void Hide(Canvas canvas)
        {
            canvas.RemoveAdornerFigure(this);
        }

        public void Update()
        {
            ForceSetPositionOfCenter(_boxedText.X, _boxedText.Y);
            HandleShape.ForceSetPositionOfCenter(_boxedText.X, _boxedText.Y);
        }

        public override void Render(DrawingContext dc)
        {
            if (HandleShape != null)
            {
                HandleShape.Canvas = Canvas;
                HandleShape.Render(dc);
            }

        }
    }
}
