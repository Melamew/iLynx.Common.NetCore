using System;
using OpenTK;

namespace iLynx.UI.OpenGL.Rendering
{
    public struct Vertex : IEquatable<Vertex>
    {
        public Vector2 Position;
        public Color Color;
        public Vector2 TexCoord;
        public Vertex(Vector2 position, Color color, Vector2 texCoord)
        {
            Position = position;
            Color = color;
            TexCoord = texCoord;
        }

        public Vertex(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
            TexCoord = new Vector2();
        }

        public Vertex(Vector2 position)
        {
            Position = position;
            Color = Color.Transparent;
            TexCoord = new Vector2();
        }

        public static bool operator ==(Vertex left, Vertex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vertex left, Vertex right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Vertex other)
        {
            return Position.Equals(other.Position) && Color.Equals(other.Color) && TexCoord.Equals(other.TexCoord);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vertex vertex && Equals(vertex);
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