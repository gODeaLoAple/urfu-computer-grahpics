using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace GraphicsCourse.Task2
{
    public class ParabolaDrawer
    {
        private readonly float _c;
        private readonly float _d;
        private readonly float _b;
        private readonly float _a;
        private readonly Rectangle _borders;

        public ParabolaDrawer(ParabolaParameters parameters, Size size)
        {
            _a = parameters.A;
            _b = parameters.B;
            _c = parameters.C;
            _d = parameters.D;
            _borders = new Rectangle(Point.Empty, size);
        }

        public void Draw(Bitmap bitmap)
        {
            var p = _c * _c / Math.Abs(_a);
            var startY = (int)_d;
            var y = startY;
            var doubleStartY = 2 * startY;
            var startX = (int)_b;
            var x = startX;
            var doubleStartX = 2 * startX;
            var delta = 0f;
            while (x < _borders.Width)
            {
                switch (_a)
                {
                    case > 0:
                        DrawSafety(bitmap, x, y);
                        DrawSafety(bitmap, x, doubleStartY - y);
                        break;
                    case < 0:
                        DrawSafety(bitmap, doubleStartX - x, doubleStartY - y);
                        DrawSafety(bitmap, doubleStartX - x, y);
                        break;
                }

                var incX = p;
                var incY = -(2 * (y - _d) + 1);
                var deltaC = delta + incX + incY;
                switch (deltaC)
                {
                    case > 0:
                    {
                        // D || C (y + 1)
                        var deltaD = delta + incY;
                        if (Math.Abs(deltaD) - Math.Abs(deltaC) < 0)
                        {
                            delta = deltaD;
                            y++;
                            continue;
                        }

                        break;
                    }
                    case < 0:
                    {
                        // B || C (x + 1)
                        var deltaB = delta + incX;
                        if (Math.Abs(deltaB) - Math.Abs(deltaC) < 0)
                        {
                            delta = deltaB;
                            x++;
                            continue;
                        }

                        break;
                    }
                }
                
                delta = deltaC;
                y++;
                x++;
            }
        }

        private void DrawSafety(Bitmap bitmap, int x, int y)
        {
            if (_borders.Contains(x, y))
            {
                bitmap.SetPixel(x, y, Color.Black);
            }
        }
    }
}