using System.Drawing;
using System.Numerics;

namespace GraphicsCourse.Task3;

public readonly struct Segment
{
    public readonly Vector2 A;
    public readonly Vector2 B;

    public Segment(Vector2 a, Vector2 b)
    {
        A = a;
        B = b;
    }
    
    public Segment(Point a, Point b) : this(new Vector2(a.X, a.Y), new Vector2(b.X, b.Y))
    {
    }

    public bool Contains(Vector2 point)
    {
        var d1 = new Vector2(B.X - A.X, B.Y - A.Y);
        var d2 = new Vector2(point.X - A.X, point.Y - A.Y);
        var abs = Math.Abs(VectorHelper.Cross(d1, d2));
        return abs <= 1e-2
            && Vector2.Dot(new Vector2(-d2.X, -d2.Y), new Vector2(B.X - point.X, B.Y - point.Y)) <= 0;
    }
}