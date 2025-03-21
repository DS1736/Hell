using System.Collections.Generic;

namespace Infini.Draw2D.Core
{
    public class ZorderComparer : IComparer<Figure>
    {
        public int Compare(Figure x, Figure y)
        {
            return x.ZOrder.CompareTo(y.ZOrder);
        }
    }
}