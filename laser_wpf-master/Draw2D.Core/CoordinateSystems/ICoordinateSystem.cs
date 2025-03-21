﻿using Infini.Draw2D.Core.Geo;

namespace Infini.Draw2D.Core
{
    public interface ICoordinateSystem
    {
        Point ToWorldSpace(double screenPointX, double screenPointY);
        double[] ToScreenSpace(Point worldPoint);
        ICanvas Canvas { get; set; }
        float OffsetX { get;  }
        float OffsetY { get; }
       
    }
}