using System;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml
{
    public struct Thickness : IEquatable<Thickness>
    {
        public float Top, Left, Bottom, Right;

        public static readonly Thickness Zero = new Thickness(0f);

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

        /// <summary>
        /// Applies the righthand <see cref="Thickness"/> as a margin to the lefthand <see cref="FloatRect"/> such that the resulting <see cref="FloatRect"/> is the /outer/ space
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FloatRect operator +(FloatRect left, Thickness right)
        {
            return new FloatRect(
                left.Left - right.Left,
                left.Top - right.Top,
                left.Width + right.Horizontal,
                left.Height + right.Vertical
                );
        }

        /// <summary>
        /// Applies the righthand <see cref="Thickness"/> as a padding to the lefthand <see cref="FloatRect"/> such that the resulting <see cref="FloatRect"/> is the /internal/ space
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FloatRect operator -(FloatRect left, Thickness right)
        {
            return new FloatRect(
                left.Left + right.Left,
                left.Top + right.Top,
                left.Width - right.Horizontal,
                left.Height - right.Vertical
                );
        }

        public static Vector2f operator +(Vector2f left, Thickness right)
        {
            return new Vector2f(left.X + right.Horizontal, left.Y + right.Vertical);
        }

        public static Vector2f operator -(Vector2f left, Thickness right)
        {
            return new Vector2f(left.X - right.Horizontal, left.Y - right.Vertical);
        }

        public static implicit operator Thickness(float value)
        {
            return new Thickness(value);
        }

        public override string ToString()
        {
            return $"{Left}, {Top}, {Right}, {Bottom}";
        }
    }
}