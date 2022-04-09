using System.Numerics;

namespace GraphicsCourse.Task3;

public static class VectorExtensions
{
    public static float DistanceTo(this Vector2 vector, Segment segment)
    {
        return new Line(segment.A, segment.B).DistanceTo(vector);
    }
}