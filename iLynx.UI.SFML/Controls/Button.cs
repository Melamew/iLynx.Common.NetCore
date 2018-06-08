using System;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public class Button : SfmlControlBase// : IButton
    {
        private Geometry geometry = new RectangleGeometry(new Vector2f(480f, 480f), new Vector2f(720f, 240f), Color.Green);
        public uint Width { get; set; }
        public uint Height { get; set; }
        public Color Background { get; set; }
        public Color Foreground { get; set; }

        protected override Geometry GetGeometry()
        {
            return geometry;
        }
    }

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
            //Shader = Shader.
        }

        private void GenerateVertices(Vertex v1, Vertex v3)
        {
            var v2 = new Vertex(new Vector2f(v3.Position.X, v1.Position.Y), v1.Color);
            var v4 = new Vertex(new Vector2f(v1.Position.X, v3.Position.Y), v3.Color);
            AddVertex(
                v1,
                v2,
                v3,
                v4,
                v1
                );
        }
    }
}
