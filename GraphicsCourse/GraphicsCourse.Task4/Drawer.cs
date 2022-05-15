using System.Drawing;

namespace GraphicsCourse.Task4;

public interface IScreenProjector
{
    PointF ToScreen(float x, float y, float z);
}

public class IsometricProjector : IScreenProjector
{
    public PointF ToScreen(float x, float y, float z)
    {
        return new PointF((y - x) * MathF.Sqrt(3) / 2, (x + y) / 2 - z);
    }
}

public class Projector : IScreenProjector
{
    public PointF ToScreen(float x, float y, float z)
    {
        var k = 2 * MathF.Sqrt(2);
        return new PointF(-y / k + x, y / k - z);
    }
}
public class CoordinateStretcher
{
    private readonly Size _size;
    private readonly float _sX;
    private readonly float _sY;
    private readonly float _kX;
    private readonly float _kY;

    public CoordinateStretcher(Size size, Segment<float> rangeX, Segment<float> rangeY)
    {
        _size = size;
        _sX = rangeX.Left;
        _sY = rangeY.Left;
        (_kX, _kY) = (size.Width / rangeX.Length(), size.Height / rangeY.Length());
    }

    public Point Stretch(PointF coord) 
        => new(
            ToBound(StretchSingle(coord.X, _sX, _kX), 0, _size.Width - 1),
            ToBound(StretchSingle(coord.Y, _sY, _kY), 0, _size.Height - 1));

    private static int StretchSingle(float coord, float offset, float resize) => (int)((coord - offset) * resize);

    private static int ToBound(int value, int min, int max) => value < min ? min : value > max ? max : value;
}

public class Drawer
{
    private int Nx => _bitmap.Width;
    private readonly Bitmap _bitmap;
    private readonly float[] _top;
    private readonly float[] _bottom;
    private readonly Func<float, float, float> _f;
    private readonly Segment<float> _rangeY;
    private readonly Segment<float> _rangeX;
    private readonly int _stepsX;
    private readonly int _stepsY;
    private readonly CoordinateStretcher _stretcher;
    private readonly IScreenProjector _projector;

    public Drawer(Bitmap bitmap, DrawerParams parameters)
    {
        _bitmap = bitmap;
        _top = new float[_bitmap.Width];
        _bottom = new float[_bitmap.Width];
        (_rangeX, _rangeY, _f) = parameters;
        _stepsX = 50;
        _stepsY = 2 * _bitmap.Width;
        _projector = new IsometricProjector();
        _stretcher = CreateStretcher();
    }

    public void Draw()
    {
        InitHorizon();
        using var graphics = Graphics.FromImage(_bitmap);
        var (stepX, stepY) = CalculateSteps();
        for (var x = _rangeX.Right; x > _rangeX.Left; x -= stepX)
        {
            for (var y = _rangeY.Right; y > _rangeY.Left; y -= stepY)
            {
                var current = Stretch(x, y, _f(x, y));
                var (xx, yy) = (current.X, current.Y);

                if (IsVisible(current))
                {
                    var color = yy > _bottom[xx] ? Color.Green : Color.Blue;
                    _bitmap.SetPixel(xx, yy, color);
                    UpdateHorizon(xx, yy);
                }
            }
        }
        
        InitHorizon();
        for (var x = _rangeX.Right; x > _rangeX.Left; x -= stepX)
        {
            for (var y = _rangeY.Right; y > _rangeY.Left; y -= stepY)
            {
                var current = Stretch(y, x, _f(y, x));
                var (xx, yy) = (current.X, current.Y);

                if (IsVisible(current))
                {
                    var color = yy > _bottom[xx] ? Color.Green : Color.Blue;
                    _bitmap.SetPixel(xx, yy, color);
                    UpdateHorizon(xx, yy);
                }
            }
        }
    }
    

    private bool IsVisible(int xx, int yy) => yy > _bottom[xx] || yy < _top[xx];
    
    private bool IsVisible(Point point) => IsVisible(point.X, point.Y);

    private void UpdateHorizon(int x, int y)
    {
        _bottom[x] = Math.Max(_bottom[x], y);
        _top[x] = Math.Min(_top[x], y);
    }

    private void UpdateHorizon(Point point) => UpdateHorizon(point.X, point.Y);

    private void InitHorizon()
    {
        for (var i = 0; i < Nx; i++)
        {
            _top[i] = _bitmap.Height;
            _bottom[i] = 0;
        }
    }

    private Point Stretch(float x, float y, float z)
    {
        var projection = _projector.ToScreen(x, y, z);
        return _stretcher.Stretch(projection);
    }

    private CoordinateStretcher CreateStretcher()
    {
        var (sX, sY) = GetScreenRanges();
        var size = new Size(_bitmap.Width - 1, _bitmap.Height - 1);
        return new CoordinateStretcher(size, sX, sY);
    }

    private (Segment<float>, Segment<float>) GetScreenRanges()
    {
        var (stepX, stepY) = CalculateSteps();
        var (minX, maxX) = (float.PositiveInfinity, float.NegativeInfinity);
        var (minY, maxY) = (float.PositiveInfinity, float.NegativeInfinity);
        for (var x = _rangeX.Left; x <= _rangeX.Right; x += stepX)
        for (var y = _rangeY.Left; y <= _rangeY.Right; y += stepY)
        {
            var point = _projector.ToScreen(x, y, _f(x, y));
            var (xx, yy) = (point.X, point.Y);
            
            minX = MathF.Min(minX, xx);
            maxX = MathF.Max(maxX, xx);

            minY = MathF.Min(minY, yy);
            maxY = MathF.Max(maxY, yy);
        }

        return (new Segment<float>(minX, maxX), new Segment<float>(minY, maxY));
    }

    private (float, float) CalculateSteps()
    {
        var stepX = _rangeX.Length() / _stepsX;
        var stepY = _rangeY.Length() / _stepsY;
        return (stepX, stepY);
    }

}