using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;

namespace GraphicsCourse.Task3;

public readonly struct Line
{
    private readonly Vector2 _a;
    private readonly Vector2 _b;

    public Line(Vector2 a, Vector2 b)
    {
        _a = a;
        _b = b;
    }

    public Vector2 Point => _a;

    public Vector2? Cross(Line line)
    {
        var (a1, b1, c1) = GetABC();
        var (a2, b2, c2) = line.GetABC();

        var dd = a1 * b2 - a2 * b1;
        if (dd != 0)
        {
            var dx = c2 * b1 - c1 * b2;
            var dy = a2 * c1 - a1 * c2;
            return new Vector2(dx / dd, dy / dd);
        }

        return null;
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private (float, float, float) GetABC()
    {
        // Ax + By + C = 0;
        var A = _a.Y - _b.Y;
        var B = _b.X - _a.X;
        var C = -(A * _a.X + B * _a.Y);
        return (A, B, C);
    }

    public Vector2? Cross(Segment segment)
    {
        var point = Cross(new Line(segment.A, segment.B));
        if (point.HasValue)
        {
            return segment.Contains(point.Value) ? point : null;
        }

        return null;
    }

    public float DistanceTo(Vector2 vector)
    {
        var (a, b, c) = GetABC();
        return (float)(Math.Abs(a * vector.X + b * vector.Y + c) / Math.Sqrt(a * a + b * b));
    }

    public Vector2 Direction()
    {
        return _b - _a;
    }
}