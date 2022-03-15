using System;
using System.Drawing;
using System.Linq;
using GraphicsCourse.Core;

namespace GraphicsCourse.Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            using var bitmap = new Bitmap(1366, 728);
            var (width, height) = (bitmap.Width - 1, bitmap.Height - 1);
            var scaler = new IndependentFunctionGraphScaler(width, height);
            
            var segment = new Segment<double>(-0.1, 0.2);
            Func<double, double> func = x => x * Math.Sin(x * x);
            
            var range = scaler.GetRange(func, segment);
            var points = scaler.Scale(func, segment);
            
            using var graphics = Graphics.FromImage(bitmap);
            using var pen = new Pen(Color.Black);
            using var redPen = new Pen(Color.Red);
            graphics.Clear(Color.White);

            if (segment.Left * segment.Right <= 0)
            {
                var k = width / (segment.Right - segment.Left);
                var l = (int)Math.Floor(-segment.Left * k);
                graphics.DrawLine(redPen, new Point(l, 0), new Point(l, bitmap.Height));
            }

            if (range.Left * range.Right <= 0)
            {
                var k = height / (range.Right - range.Left);
                var l = height - (int)Math.Floor(-range.Left * k);
                graphics.DrawLine(redPen, new Point(0, l), new Point(width, l));
            }
            
            graphics.DrawLines(pen, points.ToArray());
            
            bitmap.Save("image.bmp");
        }
    }
}