using System.Drawing;

namespace GraphicsCourse.Task2
{
    public class Drawer
    {
        private readonly Size _size;

        public Drawer(Size size)
        {
            _size = size;
        }

        public void Draw(string filename, ParabolaParameters parameters)
        {
            using var bitmap = new Bitmap(_size.Width, _size.Height);
            var drawer = new ParabolaDrawer(parameters, _size);
            using var g = Graphics.FromImage(bitmap);
            g.DrawRectangle(Pens.Green, 0, 0, _size.Width - 1, _size.Height - 1);
            drawer.Draw(bitmap);
            bitmap.Save(filename);
        }
    }
}