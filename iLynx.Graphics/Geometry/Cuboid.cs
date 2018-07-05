using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Geometry
{
    public class Cuboid : GeometryBase
    {
        private readonly Vector3 dimensions;
        private static readonly uint[] Indices = { 0, 1, 2, 3,  };
        private readonly Vertex[] vertices = new Vertex[8];

        public Cuboid(Color fillColor, Vector3 dimensions, bool showOrigin = false) : base(fillColor, true, 8, showOrigin)
        {
            this.dimensions = dimensions;
            Update();
        }

        public Cuboid(Color fillColor, float width, float height, float depth, bool showOrigin = false) : this(
            fillColor, new Vector3(width, height, depth), showOrigin)
        { }

        protected override PrimitiveType PrimitiveType => PrimitiveType.TriangleStrip;

        protected override Vertex[] GetVertices()
        {
            vertices[0] = new Vertex(new Vector3(0f, 0f, 0f), Color.Red);
            vertices[1] = new Vertex(new Vector3(0f, dimensions.Y, 0f), Color.Green);
            vertices[2] = new Vertex(new Vector3(dimensions.X, dimensions.Y, 0f), Color.Cyan);
            vertices[3] = new Vertex(new Vector3(dimensions.X, 0f, 0f), Color.Blue);

            vertices[4] = new Vertex(new Vector3(0f, 0f, dimensions.Z), Color.Magenta);
            vertices[5] = new Vertex(new Vector3(0f, dimensions.Y, dimensions.Z), Color.Yellow);
            vertices[6] = new Vertex(new Vector3(dimensions.X, dimensions.Y, dimensions.Z), Color.HotPink);
            vertices[7] = new Vertex(new Vector3(dimensions.X, 0f, dimensions.Z), Color.Honeydew);
            return vertices;
        }

        protected override uint[] GetIndices()
        {
            return Indices;
        }
    }
}
