using System.Drawing;
using System.Numerics;

namespace GraphicsCourse.Task3;

public readonly struct Segment
{
    public readonly VectorRat A;
    public readonly VectorRat B;

    public Segment(VectorRat a, VectorRat b)
    {
        A = a;
        B = b;
    }
    
    public Segment(Point a, Point b) : this(new VectorRat(a.X, a.Y), new VectorRat(b.X, b.Y))
    {
    }

    public bool Contains(VectorRat point)
    {
        var d1 = new VectorRat(B.X - A.X, B.Y - A.Y);
        var d2 = new VectorRat(point.X - A.X, point.Y - A.Y);
        return VectorHelper.Cross(d1, d2) == 0
            && VectorRat.Dot(new VectorRat(-d2.X, -d2.Y), new VectorRat(B.X - point.X, B.Y - point.Y)) <= 0;
    }
}