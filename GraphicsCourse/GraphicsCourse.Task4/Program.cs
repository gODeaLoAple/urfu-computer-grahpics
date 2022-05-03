using System.Drawing;

namespace GraphicsCourse.Task4;

internal class Program
{
    public static void Main(string[] args)
    {
        foreach (var (name, parameters) in Generate())
        {
            using var bmp = new Bitmap(1000, 1000);
            var drawer = new Drawer(bmp, parameters);
            drawer.Draw();
            bmp.Save(name);  
        }
    }

    private static IEnumerable<(string, DrawerParams)> Generate()
    {
        yield return ("a.bmp",
            new DrawerParams(
                new Segment<float>(-5, 5), 
                new Segment<float>(-5, 5),
                (x, y) => MathF.Sin(x) + MathF.Sin(y)));
        
        yield return ("b.bmp",
            new DrawerParams(
                new Segment<float>(0, 2), 
                new Segment<float>(0, 2),
                (x, y) => MathF.Cos(x) * MathF.Sin(x)));
        
        yield return ("c.bmp",
            new DrawerParams(
                new Segment<float>(10, 12), 
                new Segment<float>(12, 15),
                (x, y) => x * x * x / y));
        
        yield return ("d.bmp",
            new DrawerParams(
                new Segment<float>(0, 1), 
                new Segment<float>(-10, 10),
                (x, y) => x * MathF.Atan(y)));
        
        yield return ("e.bmp",
            new DrawerParams(
                new Segment<float>(0, 10), 
                new Segment<float>(0, 10),
                (x, y) => MathF.Sin(x + y)));
        
        yield return ("f.bmp",
            new DrawerParams(
                new Segment<float>(0, 10), 
                new Segment<float>(0, 10),
                (x, y) => MathF.Sqrt(x * x + y * y)));
        
        yield return ("g.bmp",
            new DrawerParams(
                new Segment<float>(0, 1), 
                new Segment<float>(0, 1),
                (x, y) => MathF.Sqrt(x * x + y * y)));
        
        yield return ("g.bmp",
            new DrawerParams(
                new Segment<float>(0, 10), 
                new Segment<float>(0, 10),
                (x, y) => MathF.Sin(MathF.Sqrt(x * x + y * y))));
    }
}