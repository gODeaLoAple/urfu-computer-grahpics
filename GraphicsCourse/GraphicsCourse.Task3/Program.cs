using System.Drawing;

namespace GraphicsCourse.Task3;

internal class Program
{
    public static void Main(string[] args)
    {
        var polygon = new[]
        {
            new Point(0, 0),
            new Point(0, 200),
            new Point(100, 500)
        }.Select(x => x + new Size(250, 250)).ToArray();

        using var bmp = new Bitmap(1000, 1000);

        var drawer = new Drawer();
        drawer.Draw(bmp, polygon);
        
        bmp.Save("result.bmp");
        
        
    }
}