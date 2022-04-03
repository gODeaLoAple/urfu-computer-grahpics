using System;

namespace GraphicsCourse.Task2
{
    public struct ParabolaParameters
    {
        // x = A * t^2 + B
        // y = C * t   + D
        public ParabolaParameters(float a, float b, float c, float d)
        {
            if (a == 0)
                throw new ArgumentException($"{nameof(a)} expected to be a non-zero number.");
            if (c == 0)
                throw new ArgumentException($"{nameof(c)} expected to be a non-zero number.");
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public readonly float A;
        public readonly float B;
        public readonly float C;
        public readonly float D;
    }
}