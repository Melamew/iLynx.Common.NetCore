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
        private bool dirty = true;

        public VertexBuffer()
        {
            handle = GL.GenBuffer();
        }

        public VertexBuffer(IEnumerable<Vertex> vertices)
            : this()
        {
            this.vertices = new List<Vertex>(vertices);
        }

        public PrimitiveType PrimitiveType { get; set; } = PrimitiveType.TriangleFan;
        public BufferUsageHint BufferUsage { get; set; } = BufferUsageHint.StreamDraw;

        protected virtual void BindBuffer(int bufferHandle)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
        }

        protected virtual void Copy()
        {
            if (!dirty) return;
            var verts = vertices.ToArray();
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length, verts,
                BufferUsageHint.StreamDraw);
            dirty = false;
        }

        public void Draw()
        {
            BindBuffer(handle);
            Copy();
            GL.DrawArrays(PrimitiveType, 0, vertices.Count);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(handle);
        }

        public void AddVertex(Vertex vertex)
        {
            vertices.Add(vertex);
            dirty = true;
        }

        public void AddVertices(IEnumerable<Vertex> verts)
        {
            vertices.AddRange(verts);
            dirty = true;
        }

        public void RemoveAt(int index)
        {
            vertices.RemoveAt(index);
            dirty = true;
        }

        public void AddVertices(params Vertex[] verts)
        {
            vertices.AddRange(verts);
            dirty = true;
        }

        public Vertex this[int index]
        {
            get => vertices[index];
            set
            {
                if (vertices[index] == value) return;
                vertices[index] = value;
                dirty = true;
            }
        }

        public int Length => vertices.Count;
    }
}
