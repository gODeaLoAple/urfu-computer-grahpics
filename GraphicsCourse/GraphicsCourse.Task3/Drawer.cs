using System.Drawing;
using System.Numerics;
using Rationals;

namespace GraphicsCourse.Task3;

public class Drawer
{
    public void Draw(Bitmap bmp, Point[] polygon)
    {
        var best = Solve(polygon).OrderByDescending(x => x.Value).First();
        using var g = Graphics.FromImage(bmp);
        g.TranslateTransform(bmp.Width, bmp.Height);
        g.RotateTransform(180);
        g.DrawPolygon(Pens.Black, polygon);
        Draw(g, best);
    }

    private void Draw(Graphics g, Result result)
    {
        var center = result.Center;
        var r = result.Radius;
        g.DrawArc(Pens.Green, center.X - r, center.Y - r, 2 * r, 2 * r, 0, 360);
        g.FillEllipse(Brushes.Blue, center.X - 1, center.Y - 1, 2, 2);
    }

    private static IEnumerable<Result> Solve(IEnumerable<Point> input)
    {
        var polygon = new Polygon(input.Select(x => x.ToVectorRat()).ToArray());

        var bestD = Rational.NaN;

        var points = polygon.Points;
        var count = points.Count;
        for (var i = 0; i < count; i++)
        {
            for (var j = i + 1; j < count; j++)
            {
                var p1 = points[i];
                var p2 = points[j];

                var middle = new VectorRat((p1.X + p2.X), (p1.Y + p2.Y)) / 2;
                var normal = new Line(middle, middle + new VectorRat(-(p2.Y - p1.Y), p2.X - p1.X));

                var intersections = new List<Segment>();
                var sides = polygon.Sides;
                VectorRat? previousIntersection = null;
                for (var k = 0; k < count; k++)
                {
                    var segment = sides[k];
                    var intersection = normal.Cross(segment);
                    if (intersection.HasValue)
                    {
                        if (!previousIntersection.HasValue || previousIntersection.Value != intersection.Value)
                        {
                            intersections.Add(segment);
                        }
                    }
                    previousIntersection = intersection;
                }

                var firstLine = intersections[0].AsLine();
                var secondLine = intersections[1].AsLine();
                var intersectionsPoint = firstLine.Cross(secondLine);
                VectorRat circleCenter;
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
                    var p = (firstLine.Point + secondLine.Point) / 2;
                    var line = new Line(p, p + bisector);
                    circleCenter = line.Cross(normal)!.Value;
                }

                var a = polygon.Points.Select(x => circleCenter.DistanceTo(x)).Max();
                var b = polygon.Sides.Select(x => circleCenter.DistanceTo(x)).Min();

                var d = (a - b) / 2;
                if (Rational.NaN == bestD || d < bestD)
                {
                    bestD = d;
                }

                yield return new Result(new Point((int)circleCenter.X, (int)circleCenter.Y), a, b);
            }
        }
    }
    
    private readonly record struct Result(Point Center, Rational A, Rational B)
    {
        public float Radius => (float)((A + B) / 2);
        public float Value => (float)((A - B) / 2);
    }
}