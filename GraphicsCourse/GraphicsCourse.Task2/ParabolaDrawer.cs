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
            if (parameters.A == 0) throw new ArgumentException($"{nameof(parameters.A)} should not be zero");
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
            var doubleStartY = 2 * startY;
            var startX = (int)_b;
            var doubleStartX = 2 * startX;
            var state = new ParabolaDrawerState(startX, startY, 0f);
            switch (_a)
            {
                case > 0:
                {
                    while (state.X < _borders.Width)
                    {
                        DrawSafety(bitmap, state.X, state.Y);
                        DrawSafety(bitmap, state.X, doubleStartY - state.Y);
                        state = MakeStep(state, p);
                    }

                    break;
                }
                case < 0:
                {
                    var u = doubleStartX - state.X;
                    while (u >= 0)
                    {
                        u = doubleStartX - state.X;
                        DrawSafety(bitmap, u, doubleStartY - state.Y);
                        DrawSafety(bitmap, u, state.Y);
                        state = MakeStep(state, p);
                    }

                    break;
                }
            }
        }

        private ParabolaDrawerState MakeStep(ParabolaDrawerState state, float p)
        {
            var x = state.X;
            var y = state.Y;
            var delta = state.Delta;
            var incX = p;
            var incY = -(2 * (y - _d) + 1);
            var deltaC = delta + incX + incY;
            switch (deltaC)
            {
                case > 0:
                {
                    var deltaD = delta + incY;
                    if (Math.Abs(deltaD) - Math.Abs(deltaC) < 0)
                    {
                        return new ParabolaDrawerState(x, y + 1, deltaD);
                    }

                    break;
                }
                case < 0:
                {
                    var deltaB = delta + incX;
                    if (Math.Abs(deltaB) - Math.Abs(deltaC) < 0)
                    {
                        return new ParabolaDrawerState(x + 1, y, deltaB);
                    }

                    break;
                }
            }
                
            return new ParabolaDrawerState(x + 1, y + 1, deltaC);
        }

        private void DrawSafety(Bitmap bitmap, int x, int y)
        {
            if (_borders.Contains(x, y))
            {
                bitmap.SetPixel(x, y, Color.Black);
            }
        }
    }


    public readonly struct ParabolaDrawerState
    {
        public readonly int X;
        public readonly int Y;
        public readonly float Delta;

        public ParabolaDrawerState(int x, int y, float delta)
        {
            X = x;
            Y = y;
            Delta = delta;
        }
    }
}