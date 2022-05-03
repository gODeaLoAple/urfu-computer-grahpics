using System.Drawing;
using Rationals;

namespace GraphicsCourse.Task3;

public class Drawer
{
    private const bool DoShowAll = false;
    public void Draw(Bitmap bmp, Point[] points)
    {
        var polygon = new Polygon(points.Select(x => x.ToVectorRat()).ToArray());
        var solution = Solve(polygon).OrderBy(x => x.GetValue(points));
        var best = DoShowAll ? solution : solution.Take(1);
        using var g = Graphics.FromImage(bmp);
        g.DrawPolygon(Pens.Black, points);
        foreach (var r in best)
        {
            Draw(g, r, points);
        }
        bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
    }

    public static bool IsInPolygon(IReadOnlyList<Point> polygon, Point point)
    {
        var count = polygon.Count;
        var sign = 0;
        for (var i = 0; i < count; i++)
        {
            var j = (i + 1) % count;
            var a = new Point(point.X - polygon[i].X, point.Y - polygon[i].Y);
            var b = new Point(point.X - polygon[j].X, point.Y - polygon[j].Y);
            var sin = a.X * b.Y - b.X * a.Y;  
            var cos = a.X * b.X + a.Y * b.Y;
            var angle = Math.Atan2(sin, cos) * (180 / Math.PI);
            var currentSign = angle > 0 ? 1 : -1;
            if (sign == 0)
            {
                sign = currentSign;
            }
            else if (sign != currentSign)
            {
                return false;
            }
        }
        return true;
    }
    
    private static void Draw(Graphics g, Result result, Point[] points)
    {
        var center = result.Center;
        var r = result.GetRadius(points);
        g.DrawArc(Pens.Green, center.X - r, center.Y - r, 2 * r, 2 * r, 0, 360);
        g.FillEllipse(Brushes.Blue, center.X - 1, center.Y - 1, 2, 2);
    }

    private static IEnumerable<Result> Solve(Polygon polygon)
    {
        var sides = polygon.Sides;
        var points = polygon.Points;
        foreach (var (p1, p2) in GeneratePoints(polygon))
        foreach (var (s1, s2) in GenerateSides(polygon))
        {
            var normal = GetNormal(p1, p2);
            var bisector = GetBisector(s1, s2);
           
            var t = bisector.Cross(normal);
            if (!t.HasValue)
                continue;
            var circleCenter = t.Value;

            var a = sides.Select(x => circleCenter.DistanceTo(x)).Min();
            var b = points.Select(x => circleCenter.DistanceTo(x)).Max();

            yield return new Result(new Point((int)circleCenter.X, (int)circleCenter.Y), a, b);
        }
    }

    private static Line GetNormal(VectorRat p1, VectorRat p2)
    {
        var middle = new VectorRat(p1.X + p2.X, p1.Y + p2.Y) / 2;
        return new Line(middle, middle + new VectorRat(p1.Y -p2.Y, p2.X - p1.X));
    }

    private static Line GetBisector(Segment s1, Segment s2)
    {
        var l1 = s1.AsLine();
        var l2 = s2.AsLine();
        var intersectionsPoint = l1.Cross(l2);
        if (intersectionsPoint.HasValue)
        {
            var p = intersectionsPoint.Value;
            if (l1.Point != p)
                l1 = new Line(p, l1.Point);
            if (l2.Point != p)
                l2 = new Line(p, l2.Point);
            var d1 = l1.Direction();
            var d2 = l2.Direction();
            var bisector = VectorHelper.GetBisector(d1, d2);
            return new Line(p, p + bisector);
        }
        else
        {
            var bisector = l1.Direction();
            var p = (l1.Point + l2.Point) / 2;
            return new Line(p, p + bisector);
        }
    }

    private static IEnumerable<(VectorRat, VectorRat)> GeneratePoints(Polygon polygon)
    {
        var points = polygon.Points;
        var n = points.Count;
        for (var i = 0; i < n; i++)
        for (var j = i + 1; j < n; j++)
            yield return (points[i], points[j]);
    }

    private static IEnumerable<(Segment, Segment)> GenerateSides(Polygon polygon)
    {
        var lines = polygon.Sides;
        var n = lines.Count;
        for (var i = 0; i < n; i++)
        for (var j = i + 1; j < n; j++)
            yield return (lines[i], lines[j]);
    }

    private readonly record struct Result(Point Center, Rational A, Rational B)
    {
        public float GetRadius(IReadOnlyList<Point> polygon) =>
            (float)(IsInPolygon(polygon, Center) ? (A + B)/ 2 : (A - B) / 2);
        public float GetValue(IReadOnlyList<Point> polygon) =>
            (float)(IsInPolygon(polygon, Center) ? (B - A) / 2 : (A + B) / 2);
    }
}