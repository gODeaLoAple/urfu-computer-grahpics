using System.Numerics;

namespace GraphicsCourse.Task3;

public static class VectorHelper
{
    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.X * b.Y - a.Y * b.X;
    }

    public static Vector2 GetBisector(Vector2 a, Vector2 b)
    {
        var aLength = a.Length();
        var bLength = b.Length();
        return a * bLength + b * aLength;
    }
}