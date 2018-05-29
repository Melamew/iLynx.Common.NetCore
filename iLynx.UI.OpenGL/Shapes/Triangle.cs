using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shapes
{
    public class Triangle : IRenderable
    {
        public IGeometry Geometry { get; } = new TriangleGeometry();
    }

    public class TriangleGeometry : IGeometry
    {
        private readonly Vertex[] vertices = {
            new Vertex(new Vector3(-0.5f, 0.5f, 0.0f)),
            new Vertex(new Vector3(0.5f, 0.5f, 0.0f)),
            new Vertex(new Vector3(0f, 0.0f, 0.0f))
        };

        public IEnumerable<Vertex> Vertices => vertices;

        public void GenerateVertexArrays()
        {
            GL.GenBuffers(1, out int buffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf<Vertex>() * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        }

        public void DeleteVertexArrays()
        {
            throw new System.NotImplementedException();
        }
    }
}
