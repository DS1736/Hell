using Infini.Draw2D.Core.Geo;

namespace Infini.Draw2D.Core.Policies.FigurePolicy
{
    public interface IRegionAwarePolicy
    {
        Rectangle Region { get; set; }
    }
}