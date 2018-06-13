using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public class RectangleGeometry : Geometry
    {
        public RectangleGeometry(Vector2f p1, Vector2f p2, Color color = default(Color))
            : this(new Vertex(p1, color), new Vertex(p2, color)) { }

        public RectangleGeometry(float x1, float y1, float x2, float y2, Color color = default(Color))
            : this(new Vertex(new Vector2f(x1, y1), color), new Vertex(new Vector2f(x2, y2), color)) { }

        public RectangleGeometry(Vertex v1, Vertex v2)
        {
            GenerateVertices(v1, v2);
            PrimitiveType = PrimitiveType.TriangleStrip;
        }

        private void GenerateVertices(Vertex v1, Vertex v4)
        {
            var v2 = new Vertex(new Vector2f(v4.Position.X, v1.Position.Y), v1.Color);
            var v3 = new Vertex(new Vector2f(v1.Position.X, v4.Position.Y), v4.Color);
            AddVertex(
                v1,
                v2,
                v3,
                v4
            );
        }
    }
}