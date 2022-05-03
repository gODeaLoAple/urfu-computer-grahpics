using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace GraphicsCourse.Task3.UnitTests;

public class IsInPolygonTest
{
    [Test]
    public void TestSquare()
    {
        var points = new[]
        {
            new Point(0, 0),
            new Point(0, 10),
            new Point(10, 10),
            new Point(10, 0),
        };
        Drawer.IsInPolygon(points, new Point(5, 5)).Should().BeTrue();
        Drawer.IsInPolygon(points, new Point(5, 15)).Should().BeFalse();
    }
}