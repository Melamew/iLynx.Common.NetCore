using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace iLynx.Graphics.Rendering
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex2 : IEquatable<Vertex2>
    {
        public readonly Vector2 Position;
        public readonly Color Color;
        public readonly Vector2 TexCoord;
        public Vertex2(Vector2 position, Color color, Vector2 texCoord)
        {
            Position = position;
            Color = color;
            TexCoord = texCoord;
        }

        public Vertex2(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
            TexCoord = new Vector2();
        }

        public Vertex2(Vector2 position)
        {
            Position = position;
            Color = Color.Transparent;
            TexCoord = new Vector2();
        }

        public static bool operator ==(Vertex2 left, Vertex2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vertex2 left, Vertex2 right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Vertex2 other)
        {
            return Position == other.Position && Color == other.Color && TexCoord == other.TexCoord;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Vertex2 vertex && Equals(vertex);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                hashCode = (hashCode * 397) ^ TexCoord.GetHashCode();
                return hashCode;
            }
        }
    }
}