using System.Drawing;

namespace GraphicsCourse.Task2
{
    public static class BitmapExtensions
    {
        public static void SetPixelsAround(this Bitmap bitmap, int x, int y, Color color)
        {
            bitmap.SetPixel(x, y, color);
            if (x + 1 < bitmap.Width)
                bitmap.SetPixel(x + 1, y, color);
            if (x - 1 > 0)
                bitmap.SetPixel(x - 1, y, color);
            if (y + 1 < bitmap.Height)
                bitmap.SetPixel(x, y + 1, color);
            if (y - 1 > 0)
                bitmap.SetPixel(x, y - 1, color);
        }
    }
}