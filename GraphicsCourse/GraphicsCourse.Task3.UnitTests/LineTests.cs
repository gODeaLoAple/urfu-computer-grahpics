using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using FluentAssertions;
using NUnit.Framework;

namespace GraphicsCourse.Task3.UnitTests;

[TestFixture]
public class ImagesTests
{
    private Bitmap _bmp;

    [SetUp]
    public void Setup()
    {
        _bmp = new Bitmap(1000, 1000);
    }

    [TearDown]
    public void TearDown()
    {
        _bmp.Dispose();
    }

    [Test]
    public void First()
    {
        Draw(_bmp, "First.bmp", new[]
        {
            new Point(0, 0),
            new Point(500, 0),
            new Point(0, 500)
        });
    }
    
    [Test]
    public void Second()
    {
        Draw(_bmp, "Second.bmp", new[]
        {
            new Point(0, 0),
            new Point(0, 2),
            new Point(2, 4),
            new Point(6, 5),
            new Point(7, 2),
            new Point(5, -2),
        }
            .Select(x => Multiply(x, 100))
            .Select(x =>  x + new Size(252, 252))
            .ToArray());
    }

    private static Point Multiply(Point point, int multiplier)
    {
        return new Point(point.X * multiplier, point.Y * multiplier);
    }

    public void Draw(Bitmap bitmap, string filename, Point[] polygon)
    {
        var drawer = new Drawer();
        drawer.Draw(bitmap, polygon);
        var directory = Path.Join(Directory.GetCurrentDirectory(), "Images");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        bitmap.Save(Path.Join(directory, filename));
    }
}

public class LineTests
{
    [TestCaseSource(nameof(CrossTestCases))]
    public void Cross_ShouldReturnPoint(Line a, Line b, Vector2? c)
    {
        var result = a.Cross(b);

        result.Should().Be(c);
    }

    [TestCaseSource(nameof(CrossSegmentTestCases))]
    public void CrossSegment_ShouldReturnPoint(Line a, Segment b, Vector2? c)
    {
        var result = a.Cross(b);

        result.Should().Be(c);
    }

    public static IEnumerable<TestCaseData> CrossTestCases()
    {
        yield return new TestCaseData(
            new Line(new Vector2(0, 0), new Vector2(1, 1)),
            new Line(new Vector2(0, 0), new Vector2(1, -1)),
            new Vector2(0, 0));
        
        yield return new TestCaseData(
            new Line(new Vector2(0, 0), new Vector2(1, 1)),
            new Line(new Vector2(1, 1), new Vector2(2, 2)),
            null);
    }

    public static IEnumerable<TestCaseData> CrossSegmentTestCases()
    {
        yield return new TestCaseData(
            new Line(new Vector2(300, 250), new Vector2(-200, 850)),
            new Segment(new Vector2(0, 200), new Vector2(200, 400)),
            new Vector2(186.36363f, 386.36365f));
    }
}