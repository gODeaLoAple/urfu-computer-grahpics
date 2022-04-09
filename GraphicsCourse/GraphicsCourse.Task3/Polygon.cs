using System.Drawing;
using System.Numerics;

namespace GraphicsCourse.Task3;

public class Polygon
{
    private readonly Vector2[] _points;
    private Segment[]? _sides; 

    public Polygon(Vector2[] points)
    {
        if (points.Length < 3)
            throw new ArgumentException("Points must be at leas 3 count");
        _points = points;
    }

    public IList<Vector2> Points => _points;

    public IEnumerable<Segment> Sides
    {
        get
        {
            if (_sides is null)
            {
                _sides = new Segment[_points.Length];
                for (var i = 0; i < _points.Length; i++)
                {
                    var a = _points[i];
                    var b = _points[(i + 1) % _points.Length];
                    _sides[i] = new Segment(a, b);
                }
            }

            return _sides;
        }
    }
}