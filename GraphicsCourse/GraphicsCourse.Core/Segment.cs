using System.Drawing;

namespace GraphicsCourse.Core
{
    public interface IParametricFunction
    {
        Point GetPoint(int t);
    }
    
    public readonly struct Segment<T>
    {
        public void Deconstruct(out T left, out T right)
        {
            left = Left;
            right = Right;
        }

        public readonly T Left;
        public readonly T Right;

        public Segment(T left, T right)
        {
            Left = left;
            Right = right;
        }
    }
}