namespace GraphicsCourse.Task3;

public class Polygon
{
    private readonly VectorRat[] _points;
    private readonly Segment[] _sides; 

    public Polygon(VectorRat[] points)
    {
        if (points.Length < 3)
            throw new ArgumentException("Points must be at leas 3 count");
        _points = points;
        _sides = new Segment[_points.Length];
        for (var i = 0; i < _points.Length; i++)
        {
            var a = _points[i];
            var b = _points[(i + 1) % _points.Length];
            _sides[i] = new Segment(a, b);
        }
    }

    public IList<VectorRat> Points => _points;

    public IList<Segment> Sides => _sides;
}