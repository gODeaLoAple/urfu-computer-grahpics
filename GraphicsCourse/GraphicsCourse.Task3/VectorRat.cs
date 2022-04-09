using System.Runtime.CompilerServices;
using Rationals;

namespace GraphicsCourse.Task3;

public readonly struct VectorRat : IEquatable<VectorRat>
{
    public bool Equals(VectorRat other) => X.Equals(other.X) && Y.Equals(other.Y);

    public override bool Equals(object? obj) => obj is VectorRat other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public readonly Rational X;
    public readonly Rational Y;

    public VectorRat(Rational x, Rational y)
    {
        X = x;
        Y = y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(VectorRat left, VectorRat right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(VectorRat left, VectorRat right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorRat operator +(VectorRat left, VectorRat right)
    {
        return new VectorRat(left.X + right.X, left.Y + right.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorRat operator -(VectorRat left, VectorRat right)
    {
        return new VectorRat(left.X - right.X, left.Y - right.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorRat operator /(VectorRat left, Rational value)
    {
        return new VectorRat(left.X / value, left.Y / value);
    }
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorRat operator *(VectorRat left, Rational value)
    {
        return new VectorRat(left.X * value, left.Y * value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rational Length()
    {
        var lengthSquared = LengthSquared();
        return Rational.Approximate(Math.Sqrt((double)lengthSquared));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rational LengthSquared()
    {
        return Dot(this, this);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rational Dot(VectorRat value1, VectorRat value2)
    {
        return (value1.X * value2.X) + (value1.Y * value2.Y);
    }
}