using System.Numerics;
using Rationals;

namespace GraphicsCourse.Task3;

public static class VectorHelper
{
    public static Rational Cross(VectorRat a, VectorRat b)
    {
        return a.X * b.Y - a.Y * b.X;
    }

    public static VectorRat GetBisector(VectorRat a, VectorRat b)
    {
        var aLength = a.Length();
        var bLength = b.Length();
        return a * bLength + b * aLength;
    }
}