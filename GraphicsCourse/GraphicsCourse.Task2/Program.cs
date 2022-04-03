using System.Drawing;

namespace GraphicsCourse.Task2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var drawer = new Drawer(new Size(500, 500));
            drawer.Draw("result.bmp", new ParabolaParameters(0.25f, 10, 10, 50));
        }
    }
}