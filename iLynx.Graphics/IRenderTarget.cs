using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics
{
    public interface IRenderTarget
    {
        /// <summary>
        /// Draws the specified <see cref="VertexBuffer"/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="primitiveType"></param>
        void Draw(VertexBuffer buffer, PrimitiveType primitiveType);

        /// <summary>
        /// Draws the specified vertices
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="primitiveType"></param>
        void Draw(Vertex[] vertices, PrimitiveType primitiveType);

        /// <summary>
        /// Draws the specified geometry.
        /// </summary>
        /// <param name="geometry"></param>
        void Draw(Geometry geometry);
    }
}
