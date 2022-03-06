using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicsCourse.Core
{
    public class IndependentFunctionGraphScaler : IFunctionGraphScaler
    {
        private readonly int _width;
        private readonly int _height;

        public IndependentFunctionGraphScaler(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public IEnumerable<Point> Scale(Func<double, double> f, Segment<double> segment)
        {
            var (a, b) = segment;
            var range = GetRange(f, segment);

            for (var xx = 0; xx < _width; xx++)
            {
                var x = a + xx * (b - a) / _width;
                var y = f(x);
                var yy = ToScreen(y, _height, range);

                yield return new Point(xx, _height - yy);
            }
        }

        public static int ToScreen(double value, int size, Segment<double> range)
        {
            var (left, right) = range;
            return (int)Math.Round((value - left) * size / (right - left));
        }

        public Segment<double> GetRange(Func<double, double> f, Segment<double> range)
        {
            var (minY, maxY) = (double.MaxValue, double.MinValue);
            for (var xx = 0; xx < _width; xx++)
            {
                var x = range.Left + xx * (range.Right - range.Left) / _width;
                var value = f(x);
                minY = Math.Min(minY, value);
                maxY = Math.Max(maxY, value);
            }

            return new Segment<double>(minY, maxY);
        }
    }
}