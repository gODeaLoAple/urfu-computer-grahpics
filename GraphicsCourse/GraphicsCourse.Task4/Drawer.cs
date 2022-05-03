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
    private readonly IScreenProjector _projector;
    private readonly float _sX;
    private readonly float _sY;
    private readonly float _kX;
    private readonly float _kY;

    public CoordinateStretcher(Size size, Segment<float> rangeX, Segment<float> rangeY)
    {
        _sX = rangeX.Left;
        _sY = rangeY.Left;
        (_kX, _kY) = (size.Width / rangeX.Length(), size.Height / rangeY.Length());
    }

    public Point Stretch(PointF coord) 
        => new(StretchSingle(coord.X, _sX, _kX), StretchSingle(coord.Y, _sY, _kY));

    private static int StretchSingle(float coord, float offset, float resize) => (int)(MathF.Max(coord - offset, 0) * resize);
}

public class Drawer
{
    private int Nx => _bitmap.Width;
    private int Ny => _bitmap.Height;
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
        _top = new float[Nx];
        _bottom = new float[Nx];
        (_rangeX, _rangeY, _f) = parameters;
        _stepsX = 100;
        _stepsY = 100;
        InitHorizon();
        _projector = new IsometricProjector();
        _stretcher = CreateStretcher();
    }

    public void Draw()
    {
        using var graphics = Graphics.FromImage(_bitmap);
        var (stepX, stepY) = CalculateSteps();
        for (var x = _rangeX.Right; x > _rangeX.Left; x -= stepX)
        {
            var previous = Stretch(x, _rangeY.Right, _f(x, _rangeY.Right));
            for (var y = _rangeY.Right; y > _rangeY.Left; y -= stepY)
            {
                var current = Stretch(x, y, _f(x, y));
                var (xx, yy) = (current.X, current.Y);

                if (yy < _bottom[xx] || yy > _top[xx])
                {
                    graphics.DrawLine(Pens.Black, current.X, current.Y, previous.X, previous.Y);
                }

                _bottom[xx] = Math.Min(_bottom[xx], yy);
                _top[xx] = Math.Max(_top[xx], yy);
                previous = current;
            }
        }
    }

    private void InitHorizon()
    {
        for (var i = 0; i < Nx; i++)
        {
            _top[i] = 0;
            _bottom[i] = _bitmap.Height;
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