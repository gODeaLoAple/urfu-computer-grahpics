using System.Drawing;
using System.Numerics;

namespace GraphicsCourse.Task3;

public static class PointExtensions
{
    public static VectorRat ToVectorRat(this Point point)
    {
        return new VectorRat(point.X, point.Y);
    }
}