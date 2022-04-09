using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using FluentAssertions;
using NUnit.Framework;
using Rationals;

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
    public void Triangle1()
    {
        Draw(_bmp, "Triangle1.bmp", new[]
        {
            new Point(0, 0),
            new Point(500, 0),
            new Point(0, 500)
        }.Select(Shift(new Size(100, 100))));
    }
    
    [Test]
    public void Triangle2()
    {
        const int a = 500;
        Draw(_bmp, "Triangle2.bmp", new[]
        {
            new Point(0, 0),
            new Point(a / 2, (int)(a * MathF.Sqrt(3) / 2)),
            new Point(a, 0)
        }.Select(Shift(new Size(100, 100))));
    }
    
    [Test]
    public void Square1()
    {
        const int a = 500;
        Draw(_bmp, "Square1.bmp", new[]
        {
            new Point(0, 0),
            new Point(0, a),
            new Point(a, a),
            new Point(a, 0),
        }.Select(Shift(new Size(100, 100))));
    }
    
    [Test]
    public void Polygon1()
    {
        Draw(_bmp, "Polygon1.bmp", new[]
        {
            new Point(0, 0),
            new Point(0, 2),
            new Point(2, 4),
            new Point(6, 5),
            new Point(7, 2),
            new Point(5, -2),
        }
            .Select(Multiply(100))
            .Select(Shift(new Size(250, 250))));
    }
    
    [Test]
    public void Polygon2()
    {
        Draw(_bmp, "Polygon2.bmp", new[]
            {
                new Point(0, 0),
                new Point(2, 4),
                new Point(6, 4),
                new Point(8, 0)
            }
            .Select(Multiply(100))
            .Select(Shift(new Size(100, 100))));
    }

    private static Func<Point, Point> Multiply(int multiplier)
    {
        return point => new Point(point.X * multiplier, point.Y * multiplier);
    }

    private static Func<Point, Point> Shift(Size offset)
    {
        return point => point + offset;
    }

    public void Draw(Bitmap bitmap, string filename, IEnumerable<Point> polygon)
    {
        var drawer = new Drawer();
        drawer.Draw(bitmap, polygon.ToArray());
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
    public void Cross_ShouldReturnPoint(Line a, Line b, VectorRat? c)
    {
        var result = a.Cross(b);

        result.Should().Be(c);
    }

    [TestCaseSource(nameof(CrossSegmentTestCases))]
    public void CrossSegment_ShouldReturnPoint(Line a, Segment b, VectorRat? c)
    {
        var result = a.Cross(b);

        result.Should().Be(c);
    }

    public static IEnumerable<TestCaseData> CrossTestCases()
    {
        yield return new TestCaseData(
            new Line(new VectorRat(0, 0), new VectorRat(1, 1)),
            new Line(new VectorRat(0, 0), new VectorRat(1, -1)),
            new VectorRat(0, 0));
        
        yield return new TestCaseData(
            new Line(new VectorRat(0, 0), new VectorRat(1, 1)),
            new Line(new VectorRat(1, 1), new VectorRat(2, 2)),
            null);
    }

    public static IEnumerable<TestCaseData> CrossSegmentTestCases()
    {
        yield return new TestCaseData(
            new Line(new VectorRat(300, 250), new VectorRat(-200, 850)),
            new Segment(new VectorRat(0, 200), new VectorRat(200, 400)),
            new VectorRat(Rational.Approximate(186.36363f), Rational.Approximate(386.36365f)));
    }
}