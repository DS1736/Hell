using System.Collections.Generic;
using Infini.Draw2D.Core.Geo;
using Infini.Draw2D.Core.Handles;
using Infini.Draw2D.Core.Layout.Connection;
using Infini.Draw2D.Core.Shapes.Basic;

namespace Infini.Draw2D.Core.Policies.RouterPolicy
{
    public class GraphTool : ToolBase
    {
        private LineHandle _lineHandle;
        private bool _isInitialized;
        private Graph _graph;

        public GraphTool()
        {
        }

        public override void OnMouseMove(Canvas canvas, float mouseX, float mouseY, bool isShiftKey, bool isCtrlKey)
        {
            if (!_isInitialized)
            {
                //_lineHandle = new LineHandle(new Ellipse(mouseX, mouseY, 10, 10), 0, _graph);
            }
        }

        public override void OnMouseLeftDown(Canvas canvas, float mouseX, float mouseY, bool isShiftKey, bool isCtrlKey)
        {

        }

        public override void Reroute(IReadOnlyList<Point> points)
        {

        }
    }
}