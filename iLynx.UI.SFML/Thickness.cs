using System;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML
{
    public struct Thickness : IEquatable<Thickness>
    {
        public float Top, Left, Bottom, Right;

        public static readonly Thickness NaN = new Thickness(float.NaN);

        public static bool IsNaN(Thickness thickness)
        {
            return float.IsNaN(thickness.Left) &&
                   float.IsNaN(thickness.Top) &&
                   float.IsNaN(thickness.Right) &&
                   float.IsNaN(thickness.Bottom);
        }

        public bool Equals(Thickness other)
        {
            return MathF.Abs(Top - other.Top) <= float.Epsilon &&
                   MathF.Abs(Left - other.Left) <= float.Epsilon &&
                   MathF.Abs(Bottom - other.Bottom) <= float.Epsilon &&
                   MathF.Abs(Right - other.Right) <= float.Epsilon;
        }

        public float Vertical => Top + Bottom;
        public float Horizontal => Left + Right;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Thickness thickness && Equals(thickness);
        }

        public Thickness(float left, float top, float right, float bottom)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public Thickness(float uniformWidth)
            : this(uniformWidth, uniformWidth, uniformWidth, uniformWidth)
        {

        }

        public Thickness(float horizontal, float vertical)
            : this(horizontal, vertical, horizontal, vertical) { }

        public Thickness(Vector2f dimensions)
            : this(dimensions.X, dimensions.Y) { }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyMemberInGetHashCode
                var hashCode = Top.GetHashCode();
                hashCode = (hashCode * 397) ^ Left.GetHashCode();
                hashCode = (hashCode * 397) ^ Bottom.GetHashCode();
                hashCode = (hashCode * 397) ^ Right.GetHashCode();
                return hashCode;
                // ReSharper restore NonReadonlyMemberInGetHashCode
            }
        }

        public static bool operator ==(Thickness left, Thickness right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Thickness left, Thickness right)
        {
            return !left.Equals(right);
        }

        public static FloatRect operator +(FloatRect left, Thickness right)
        {
            return new FloatRect(
                left.Left - right.Left,
                left.Top - right.Top,
                left.Width + right.Right + right.Left,
                left.Height + right.Bottom + right.Top
                );
        }
             
    }
}