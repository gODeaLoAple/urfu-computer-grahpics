using System.Drawing;
using System.Numerics;

namespace GraphicsCourse.Task3;

public class Drawer
{
    public void Draw(Bitmap bmp, Point[] polygon)
    {
        var circles = Solve(polygon);

        using var g = Graphics.FromImage(bmp);
        g.DrawPolygon(Pens.Black, polygon);
        foreach (var circle in circles)
        {
            var (center, r) = circle;
            g.DrawArc(Pens.Green, center.X - r, center.Y - r, 2 * r, 2 * r, 0, 360);
            g.FillEllipse(Brushes.Blue, center.X - 5, center.Y - 5, 10, 10);
        }
    }

    private static IEnumerable<(Point, float)> Solve(IEnumerable<Point> input)
    {
        var polygon = new Polygon(input.Select(x => x.ToVector2()).ToArray());

        var bestD = float.MaxValue;

        var points = polygon.Points;
        var count = points.Count;
        for (var i = 0; i < count; i++)
        {
            for (var j = i + 1; j < count; j++)
            {
                var p1 = points[i];
                var p2 = points[j];

                var middle = new Vector2((p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);
                var normal = new Line(middle, middle + new Vector2(-(p2.Y - p1.Y), p2.X - p1.X));

                var intersections = polygon.Sides
                    .Select(x => (Segment: x, Point: normal.Cross(x)))
                    .Where(x => x.Point.HasValue)
                    .ToArray();

                var firstLine = intersections[0].Segment.AsLine();
                var secondLine = intersections[1].Segment.AsLine();
                var intersectionsPoint = firstLine.Cross(secondLine);
                Vector2 circleCenter;
                if (intersectionsPoint.HasValue)
                {
                    var p = intersectionsPoint.Value;
                    var bisector = VectorHelper.GetBisector(firstLine.Direction() * -1, secondLine.Direction());
                    var line = new Line(p, p + bisector);
                    circleCenter = line.Cross(normal)!.Value;
                }
                else
                {
                    var bisector = firstLine.Direction();
                    var p = (firstLine.Point + secondLine.Point) * 0.5f;
                    var line = new Line(p, p + bisector);
                    circleCenter = line.Cross(normal)!.Value;
                }

                var a = polygon.Points.Select(x => circleCenter - x).Max(x => x.Length());
                var b = polygon.Sides.Select(x => circleCenter.DistanceTo(x)).Min();

                var d = (a - b) / 2;
                if (d < bestD)
                {
                    bestD = d;
                }

                yield return (new Point((int)circleCenter.X, (int)circleCenter.Y), (a + b) / 2);
            }
        }
    }
}