using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Rendering
{
    public interface IRenderTarget
    {
        //void Draw(Vertex[] vertices, PrimitiveType primitiveType);
        void Draw(VertexBuffer buffer, PrimitiveType primitiveType);

        void Draw(Geometry geometry);
    }

    public class VertexBuffer : IDisposable
    {
        private readonly int handle;
        private readonly List<Vertex> vertices = new List<Vertex>();
        private PrimitiveType primitiveType;

        public VertexBuffer()
        {
            handle = GL.GenBuffer();
        }

        public VertexBuffer(Vertex[] vertices)
            : this()
        {
            this.vertices = new List<Vertex>(vertices);
        }

        public PrimitiveType PrimitiveType
        {
            get { return primitiveType; }
            set { primitiveType = value; }
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
        }

        public void CopyToDevice()
        {
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) vertices.Count, vertices.ToArray(),
                BufferUsageHint.StreamDraw);
        }

        public void Draw()
        {
            GL.DrawArrays(primitiveType, 0, vertices.Count);
        }

        public void Dispose()
        {
        }
    }
}
