using System;
using System.Drawing;
using System.IO;
using NUnit.Framework;

namespace GraphicsCourse.Task2.UnitTests;

public class Tests
{
    private Drawer _drawer = null!;

    [SetUp]
    public void Setup()
    {
        _drawer = new Drawer(new Size(800, 800));
    }
    
    

    [TestCase(1, 0, 1, 0)]
    [TestCase(1, 0, 1, 100)]
    [TestCase(1, 0, 1, 200)]
    [TestCase(1, 0, 1, 300)]
    [TestCase(1, 0, 1, 400)]
    [TestCase(1, 0, 1, 500)]
    [TestCase(1, 0, 1, 600)]
    [TestCase(1, 0, 1, 700)]
    [TestCase(1, 0, 1, 800)]
    public void MoveVertical(float a, float b, float c, float d)
    {
       Draw(a,b,c,d);
    }
    
    [TestCase(1, 0, 1, -800)]
    [TestCase(1, 0, 1, -700)]
    [TestCase(1, 0, 1, -600)]
    [TestCase(1, 0, 1, -500)]
    [TestCase(1, 0, 1, -400)]
    [TestCase(1, 0, 1, -300)]
    [TestCase(1, 0, 1, -200)]
    [TestCase(1, 0, 1, -100)]
    public void MoveVerticalNegative(float a, float b, float c, float d)
    {
        Draw(a,b,c,d);
    }
    
    [TestCase(1, 0, 1, 400)]
    [TestCase(1, 100, 1, 400)]
    [TestCase(1, 200, 1, 400)]
    [TestCase(1, 300, 1, 400)]
    [TestCase(1, 400, 1, 400)]
    [TestCase(1, 500, 1, 400)]
    [TestCase(1, 600, 1, 400)]
    [TestCase(1, 700, 1, 400)]
    [TestCase(1, 800, 1, 400)]
    public void MoveHorizontal(float a, float b, float c, float d)
    {
        Draw(a,b,c,d);
    }
    
    [TestCase(1, -800, 1, 400)]
    [TestCase(1, -700, 1, 400)]
    [TestCase(1, -600, 1, 400)]
    [TestCase(1, -500, 1, 400)]
    [TestCase(1, -400, 1, 400)]
    [TestCase(1, -300, 1, 400)]
    [TestCase(1, -200, 1, 400)]
    [TestCase(1, -100, 1, 400)]
    [TestCase(1, 0, 1, 400)]
    public void MoveHorizontalNegative(float a, float b, float c, float d)
    {
        Draw(a,b,c,d);
    }
    
    [TestCase(-0.01f, 799, 1, 400)]
    [TestCase(-0.01f, 700, 1, 400)]
    [TestCase(-0.01f, 600, 1, 400)]
    [TestCase(-0.01f, 500, 1, 400)]
    [TestCase(-0.01f, 400, 1, 400)]
    [TestCase(-0.01f, 300, 1, 400)]
    [TestCase(-0.01f, 200, 1, 400)]
    [TestCase(-0.01f, 100, 1, 400)]
    [TestCase(-0.01f, 0, 1, 400)]
    public void MoveHorizontalNegativeA(float a, float b, float c, float d)
    {
        Draw(a,b,c,d);
    }

    
    [TestCase(1, 0, 1, 0)]
    [TestCase(1, 100, 1, 100)]
    [TestCase(1, 200, 1, 200)]
    [TestCase(1, 300, 1, 300)]
    [TestCase(1, 400, 1, 400)]
    [TestCase(1, 500, 1, 500)]
    [TestCase(1, 600, 1, 600)]
    [TestCase(1, 700, 1, 700)]
    [TestCase(1, 800, 1, 800)]
    public void MoveDiagonal(float a, float b, float c, float d)
    {
        Draw(a,b,c,d);
    }
    
    [TestCase(-100, 400, 1, 400)]
    [TestCase(-10, 400, 1, 400)]
    [TestCase(-1, 400, 1, 400)]
    [TestCase(-0.1f, 400, 1, 400)]
    [TestCase(-0.01f, 400, 1, 400)]
    [TestCase(-0.001f, 400, 1, 400)]
    [TestCase(0.001f, 400, 1, 400)]
    [TestCase(0.01f, 400, 1, 400)]
    [TestCase(0.1f, 400, 1, 400)]
    [TestCase(1, 400, 1, 400)]
    [TestCase(10, 400, 1, 400)]
    [TestCase(100, 400, 1, 400)]
    public void ChangeA(float a, float b, float c, float d)
    {
        Draw(a, b, c, d, context => a + ".bmp");
    }
    
    [TestCase(-100, 400, 1, 400)]
    [TestCase(-10, 400, 1, 400)]
    [TestCase(-1, 400, 1, 400)]
    [TestCase(-0.1f, 400, 1, 400)]
    [TestCase(-0.01f, 400, 1, 400)]
    [TestCase(-0.001f, 400, 1, 400)]
    [TestCase(0.001f, 400, 1, 400)]
    [TestCase(0.01f, 400, 1, 400)]
    [TestCase(0.1f, 400, 1, 400)]
    [TestCase(1, 400, 1, 400)]
    [TestCase(10, 400, 1, 400)]
    [TestCase(100, 400, 1, 400)]
    public void ChangeC(float c, float b, float a, float d)
    {
        Draw(a, b, c, d, context => c + ".bmp");
    }
    
    private void Draw(float a, float b, float c, float d, Func<TestContext.TestAdapter, string> imageNameFactory) 
    {
        var context = TestContext.CurrentContext.Test;
        var filename = imageNameFactory(context);
        var directory = Path.Join( "TestImages", (context.MethodName ?? throw new Exception()));
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        _drawer.Draw(Path.Join(directory, filename), new ParabolaParameters(a, b, c, d));
    }

    private void Draw(float a, float b, float c, float d)
    {
        Draw(a, b, c, d, adapter => adapter.MethodName + string.Join("_", TestContext.CurrentContext.Test.Arguments) + ".bmp");
    }
}