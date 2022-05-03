namespace GraphicsCourse.Task4;

public record Segment<T>(T Left, T Right);

public static class SegmentExtensions
{
    public static float Length(this Segment<float> segment) => segment.Right - segment.Left;
}