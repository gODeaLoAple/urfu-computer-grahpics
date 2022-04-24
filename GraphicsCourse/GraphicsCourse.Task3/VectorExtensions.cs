using System.Drawing;
using Rationals;

namespace GraphicsCourse.Task3;

public static class VectorExtensions
{
    public static Rational DistanceTo(this VectorRat vector, Segment segment)
    {
        return Rational.Approximate(new Line(segment.A, segment.B).DistanceTo(vector));
    }

    public static Rational DistanceTo(this VectorRat vectorRat, VectorRat other)
    {
        return (vectorRat - other).Length();
    }

    public static Point AsPoint(this VectorRat vector)
    {
        return new Point((int)vector.X.WholePart, (int)vector.Y.WholePart);
    }
}