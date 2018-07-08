using System;
using System.Runtime.InteropServices;
using ImageMagick;
using OpenTK;

namespace iLynx.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
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


        public static bool operator ==(Color32 left, Color32 right)
        {
            return Equals(right.R, left.R) && Equals(right.G, left.G) && Equals(right.B, left.B) && Equals(right.A, left.A);
        }

        public static bool operator !=(Color32 left, Color32 right)
        {
            return !Equals(right.R, left.R) && !Equals(right.G, left.G) && !Equals(right.B, left.B) && !Equals(right.A, left.A);
        }

        public override string ToString()
        {
            return $"rgba({R:F2},{G:F2},{B:F2},{A:F2})";
        }

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
            //var rComparison = R.CompareTo(other.R);
            //if (rComparison != 0) return rComparison;
            //var gComparison = G.CompareTo(other.G);
            //if (gComparison != 0) return gComparison;
            //var bComparison = B.CompareTo(other.B);
            //if (bComparison != 0) return bComparison;
            //return A.CompareTo(other.A);
        }
    }
}