using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicsCourse.Core
{
    public interface IFunctionGraphScaler
    {
        IEnumerable<Point> Scale(Func<double, double> f, Segment<double> segment);
    }
}