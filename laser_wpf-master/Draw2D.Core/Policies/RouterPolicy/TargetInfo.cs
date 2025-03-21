using System.Security.AccessControl;
using Infini.Draw2D.Core.Geo;
using Infini.Draw2D.Core.Utlils;

namespace Infini.Draw2D.Core.Policies.RouterPolicy
{
    internal class TargetInfo
    {
        public Point TargetVertex { get; set; }
        public Point SnapPoint { get; set; }
        public Point SourcePoint { get; set; }
        public Point NormalizedDirection => (TargetVertex - SnapPoint).Normalized();

        public bool IsInXDirection => NormalizedDirection.X != 0;

        public override string ToString()
        {
            return $"SourcePoint: {SourcePoint}, TargetVertex: {TargetVertex}, SnapPoint: {SnapPoint}";
        }
    }
}