namespace GraphicsCourse.Task2
{
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