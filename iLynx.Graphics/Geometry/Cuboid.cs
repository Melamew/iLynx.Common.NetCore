using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Geometry
{
    public class Cuboid : GeometryBase
    {
        private readonly Vector3 dimensions;
        private readonly Vertex[] vertices = new Vertex[8];

        public Cuboid(Color fillColor, Vector3 dimensions, bool showOrigin = false) : base(fillColor, true, 8, showOrigin)
        {
            this.dimensions = dimensions;
            Update();
        }

        public Cuboid(Color fillColor, float width, float height, float depth, bool showOrigin = false) : this(
            fillColor, new Vector3(width, height, depth), showOrigin)
        { }

        protected override PrimitiveType PrimitiveType => PrimitiveType.Triangles;

        private static readonly uint[] Indices =
        {
            // Front Face
            0, 1, 3,
            3, 2, 0,
            // Left Face
            0, 2, 6,
            6, 4, 0,
            // Bottom Face
            4, 5, 1,
            1, 0, 4,
            // Back Face
            4, 6, 7,
            7, 5, 4,
            // Top Face
            7, 6, 2,
            2, 3, 7,
            // Right Face
            7, 3, 1,
            1, 5, 7
        };
        protected override Vertex[] GetVertices()
        {
            vertices[0] = new Vertex(new Vector3(0f, 0f, 0f), Color.Red); // Front Bottom Left
            vertices[1] = new Vertex(new Vector3(dimensions.X, 0f, 0f), Color.Lime); // Front Bottom Right
            vertices[2] = new Vertex(new Vector3(0f, dimensions.Y, 0f), Color.Blue); // Front Top Left
            vertices[3] = new Vertex(new Vector3(dimensions.X, dimensions.Y, 0f), Color.Cyan); // Front Top Right

            vertices[4] = new Vertex(new Vector3(0f, 0f, dimensions.Z), Color.Magenta); // Back Bottom Left
            vertices[5] = new Vertex(new Vector3(dimensions.X, 0f, dimensions.Z), Color.Yellow); // Back Bottom Right
            vertices[6] = new Vertex(new Vector3(0f, dimensions.Y, dimensions.Z), Color.Turquoise); // Back Top Left
            vertices[7] = new Vertex(new Vector3(dimensions.X, dimensions.Y, dimensions.Z), Color.HotPink); // Back Top Right
            return vertices;
        }

        protected override uint[] GetIndices()
        {
            return Indices;
        }
    }
}
