using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using Rationals;

namespace GraphicsCourse.Task3;

public readonly struct Line
{
    private readonly VectorRat _a;
    private readonly VectorRat _b;

    public Line(VectorRat a, VectorRat b)
    {
        _a = a;
        _b = b;
    }

    public VectorRat Point => _a;

    public VectorRat? Cross(Line line)
    {
        var (a1, b1, c1) = GetCoefficients();
        var (a2, b2, c2) = line.GetCoefficients();

        var dd = a1 * b2 - a2 * b1;
        if (dd != 0)
        {
            var dx = c2 * b1 - c1 * b2;
            var dy = a2 * c1 - a1 * c2;
            return new VectorRat(dx / dd, dy / dd);
        }

        return null;
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private (Rational, Rational, Rational) GetCoefficients()
    {
        // Ax + By + C = 0;
        var A = _a.Y - _b.Y;
        var B = _b.X - _a.X;
        var C = -(A * _a.X + B * _a.Y);
        return (A, B, C);
    }

    public VectorRat? Cross(Segment segment)
    {
        var point = Cross(new Line(segment.A, segment.B));
        if (point.HasValue)
        {
            return segment.Contains(point.Value) ? point : null;
        }

        return null;
    }

    public float DistanceTo(VectorRat vector)
    {
        var (a, b, c) = GetCoefficients();
        var k1 = a * a + b * b;
        var k2 = a * vector.X + b * vector.Y + c;
        return (float)(Math.Abs((double)k2) / Math.Sqrt((double)k1));
    }

    public VectorRat Direction() => _b - _a;
}