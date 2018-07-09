using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenTK;
using SharpFont;
using SixLabors.ImageSharp.PixelFormats;
using static System.MathF;

namespace iLynx.Graphics
{
    /// <summary>
    /// Represents a color consisting of 4 32-bit channels (Red, Green, Blue, Alpha).
    /// </summary>
    /// <inheritdoc cref="IComparable{Color32}"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct Color32 : IComparable<Color32>, IPixel<Color32>
    {
        public float R;
        public float G;
        public float B;
        public float A;

        /// <summary>
        /// Initializes a new instance of <see cref="Color32"/> with the specified values
        /// </summary>
        /// <param name="r">The red (R) component</param>
        /// <param name="g">The green (G) component</param>
        /// <param name="b">The blue (B) component</param>
        /// <param name="a">The alpha (A) component</param>
        public static Color32 FromRgba(float r, float g, float b, float a)
        {
            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Color32"/> with the specified values divided by <see cref="byte.MaxValue"/>
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Color32 FromRgba(byte r, byte g, byte b, byte a)
        {
            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Color32"/> with the specified values
        /// </summary>
        /// <param name="r">The red (R) component</param>
        /// <param name="g">The green (G) component</param>
        /// <param name="b">The blue (B) component</param>
        /// <param name="a">The alpha (A) component</param>
        public Color32(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color32(byte r, byte g, byte b, byte a)
        {
            this = new Color32(r / (float)byte.MaxValue, g / (float)byte.MaxValue, b / (float)byte.MaxValue, a / (float)byte.MaxValue);
        }

        /// <summary>
        /// Normalizes this <see cref="Color32"/> to a range within 0 to 1
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            Maximum(ref this, out var max);
            R /= max;
            G /= max;
            B /= max;
            A /= max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color32 Normalized()
        {
            var col = this;
            col.Normalize();
            return col;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Maximum(ref Color32 color, out float result)
        {
            Absolute(ref color, out color);
            var max = Max(color.R, color.G);
            max = Max(max, color.B);
            result = Max(max, color.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Absolute(ref Color32 color, out Color32 result)
        {
            result.R = Abs(color.R);
            result.G = Abs(color.G);
            result.B = Abs(color.B);
            result.A = Abs(color.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Color32(Rgba64 v)
        {
            var vector = v.ToScaledVector4();
            return new Color32(vector.X, vector.Y, vector.Z, vector.W);
        }

        /// <summary>
        /// Returns a new <see cref="Color32"/> instance converted from the specified <see cref="Color"/> (255f / component).
        /// </summary>
        /// <param name="color"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color32(Color color)
        {
            return FromRgba(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Returns a 4 component vector with the components of this color (The format is R,G,B,A)
        /// </summary>
        /// <param name="color"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector4(Color32 color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Divides the components of the specified  color by the specified value
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color32 operator /(Color32 left, float right)
        {
            return new Color32(left.R / right, left.G / right, left.B / right, left.A / right);
        }

        /// <summary>
        /// Returns a Color32 with value / right.RGBA values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color32 operator /(float left, Color32 right)
        {
            return new Color32(left / right.R, left / right.G, left / right.B, left / right.A);
        }

        /// <summary>
        /// Multiplies the specified color by the specified value
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color32 operator *(Color32 left, float right)
        {
            return new Color32(left.R * right, left.G * right, left.B * right, left.A * right);
        }

        /// <summary>
        /// Compares two colors and returns a value indicating wether or not they are equal
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Color32 left, Color32 right)
        {
            return Equals(right.R, left.R) && Equals(right.G, left.G) && Equals(right.B, left.B) && Equals(right.A, left.A);
        }

        /// <summary>
        /// Compares two colors and returns a value indicating wether or not they are NOT equal
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Color32 left, Color32 right)
        {
            return !Equals(right.R, left.R) && !Equals(right.G, left.G) && !Equals(right.B, left.B) && !Equals(right.A, left.A);
        }

        public override string ToString()
        {
            return $"<{R},{G},{B},{A}>";
        }

        /// <summary>
        /// Compares the values of this color to the values of the specified <see cref="Color32"/>
        /// Comparison occurs in the following order: R, G, B, A.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Color32 other)
        {
            if (R < other.R)
                return -1;
            if (R > other.R)
                return 1;
            if (G < other.G)
                return -1;
            if (G > other.G)
                return 1;
            if (B < other.B)
                return -1;
            if (B > other.B)
                return 1;
            if (A < other.A)
                return -1;
            return A > other.A ? 1 : 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Color32 other)
        {
            return Equals(R, other.R) && Equals(G, other.G) && Equals(B, other.B) && Equals(A, other.A);
        }

        /// <inheritdoc />
        public PixelOperations<Color32> CreatePixelOperations()
        {
            return new PixelOperations<Color32>();
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Color32 color32 && Equals(color32);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                return hashCode;
            }
        }

        public static Color32 Lime => new Color32(0f, 1f, 0f, 1f);
        public static Color32 Transparent => new Color32(0f, 0f, 0f, 0f);
        public static Color32 White => new Color32(1f, 1f, 1f, 1f);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromVector4(System.Numerics.Vector4 vector)
        {
            R = vector.X;
            G = vector.Y;
            B = vector.Z;
            A = vector.W;
            Normalize();
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromScaledVector4(System.Numerics.Vector4 vector)
        {
            R = vector.X;
            G = vector.Y;
            B = vector.Z;
            A = vector.W;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public System.Numerics.Vector4 ToScaledVector4()
        {
            return new System.Numerics.Vector4(R, G, B, A);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public System.Numerics.Vector4 ToVector4()
        {
            return ToScaledVector4() * byte.MaxValue;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromRgba32(Rgba32 source)
        {
            this = FromRgba(source.R, source.G, source.B, source.A);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromArgb32(Argb32 source)
        {
            this = FromRgba(source.R, source.G, source.B, source.A);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromBgra32(Bgra32 source)
        {
            this = FromRgba(source.R, source.G, source.B, source.A);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToRgb24(ref Rgb24 dest)
        {
            dest.R = (byte)(byte.MaxValue * R);
            dest.G = (byte)(byte.MaxValue * G);
            dest.B = (byte)(byte.MaxValue * B);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToRgba32(ref Rgba32 dest)
        {
            dest.R = (byte)(byte.MaxValue * R);
            dest.G = (byte)(byte.MaxValue * G);
            dest.B = (byte)(byte.MaxValue * B);
            dest.A = (byte)(byte.MaxValue * A);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToArgb32(ref Argb32 dest)
        {
            dest.R = (byte)(byte.MaxValue * R);
            dest.G = (byte)(byte.MaxValue * G);
            dest.B = (byte)(byte.MaxValue * B);
            dest.A = (byte)(byte.MaxValue * A);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgr24(ref Bgr24 dest)
        {
            dest.R = (byte)(byte.MaxValue * R);
            dest.G = (byte)(byte.MaxValue * G);
            dest.B = (byte)(byte.MaxValue * B);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgra32(ref Bgra32 dest)
        {
            dest.R = (byte)(byte.MaxValue * R);
            dest.G = (byte)(byte.MaxValue * G);
            dest.B = (byte)(byte.MaxValue * B);
            dest.A = (byte)(byte.MaxValue * A);
        }
    }
}