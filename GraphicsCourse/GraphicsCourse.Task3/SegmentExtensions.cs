namespace GraphicsCourse.Task3;

public static class SegmentExtensions
{
    public static Line AsLine(this Segment segment) => new Line(segment.A, segment.B);
}