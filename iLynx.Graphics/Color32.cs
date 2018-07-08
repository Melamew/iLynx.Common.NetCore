using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using ImageMagick;
using OpenTK;

namespace iLynx.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct Color32 : IComparable<Color32>
    {
        public bool Equals(Color32 other)
        {
            return Equals(R, other.R) && Equals(G, other.G) && Equals(B, other.B) && Equals(A, other.A);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Color32 color32 && Equals(color32);
        }

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

        public readonly float R;
        public readonly float G;
        public readonly float B;
        public readonly float A;

        public static readonly Color32 Transparent = FromRgba(0f, 0f, 0f, 0f);

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

        /// <summary>
        /// Normalizes this <see cref="Color32"/> to a range within -1 to +1
        /// <see cref="Normalize(Color32)"/>
        /// <seealso cref="NormalizeRGB()"/>
        /// <seealso cref="NormalizeRGB(Color32)"/>
        /// </summary>
        /// <returns></returns>
        public Color32 Normalize()
        {
            return Normalize(this);
        }

        /// <summary>
        /// Normalizes the specified <see cref="Color32"/> to a range within -1 to +1
        /// <seealso cref="NormalizeRGB()"/>
        /// <seealso cref="NormalizeRGB(Color32)"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color32 Normalize(Color32 color)
        {
            var cA = Absolute(color);
            var max = Max(cA);
            return color / max;
        }

        /// <summary>
        /// Normalizes the Red Green and Blue components of this <see cref="Color32"/> (NOT including the Alpha component).
        /// <see cref="NormalizeRGB(Color32)"/>
        /// <seealso cref="Normalize()"/>
        /// <seealso cref="Normalize(Color32)"/>
        /// </summary>
        /// <returns></returns>
        public Color32 NormalizeRGB()
        {
            return NormalizeRGB(this);
        }
        /// <summary>
        /// Normalizes the Red Green and Blue components of the specified color (NOT including the Alpha component).
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color32 NormalizeRGB(Color32 color)
        {
            var maxRGB = MaxRGB(color);
            return new Color32(color.R / maxRGB, color.G / maxRGB, color.B / maxRGB, color.A);
        }

        /// <summary>
        /// Returns the maximum value of the "color" (RGB) components of the specified <see cref="Color32"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static float MaxRGB(Color32 color)
        {
            var max = MathF.Max(color.R, color.G);
            return MathF.Max(max, color.B);
        }

        /// <summary>
        /// Returns the maximum value of the R, G, B and A components of the specified color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static float Max(Color32 color)
        {
            var max = MathF.Max(color.R, color.G);
            max = MathF.Max(max, color.B);
            return MathF.Max(max, color.A);
        }

        /// <summary>
        /// Returns the absolute values of the specified color
        /// </summary>
        /// <param name="color"></param>
        /// <returns>An instance of <see cref="Color32"/> with all its values equal to the absolute of the specified color</returns>
        public static Color32 Absolute(Color32 color)
        {
            return new Color32(MathF.Abs(color.R), MathF.Abs(color.G), MathF.Abs(color.B), MathF.Abs(color.A));
        }

        /// <summary>
        /// Returns a new <see cref="Color32"/> instance with the values of the specified <see cref="MagickColor"/>
        /// </summary>
        /// <param name="color"></param>
        public static implicit operator Color32(MagickColor color)
        {
            return new Color32(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Returns a new <see cref="Color32"/> instance converted from the specified <see cref="Color"/> (255f / component).
        /// </summary>
        /// <param name="color"></param>
        public static implicit operator Color32(Color color)
        {
            return new Color32(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        /// <summary>
        /// Returns a 4 component vector with the components of this color (The format is R,G,B,A)
        /// </summary>
        /// <param name="color"></param>
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
        public static bool operator !=(Color32 left, Color32 right)
        {
            return !Equals(right.R, left.R) && !Equals(right.G, left.G) && !Equals(right.B, left.B) && !Equals(right.A, left.A);
        }

        public override string ToString()
        {
            return $"rgba({R:F2},{G:F2},{B:F2},{A:F2})";
        }

        /// <summary>
        /// Compares the values of this color to the values of the specified <see cref="Color32"/>
        /// Comparison occurs in the following order: R, G, B, A.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
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

        public static Color32 Lime => new Color32(0f, 1f, 0f, 1f);
    }
}