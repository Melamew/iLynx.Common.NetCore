using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Rendering
{
    public class VertexBuffer : IDisposable
    {
        private readonly int handle;
        private readonly List<Vertex> vertices = new List<Vertex>();

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

        public static void Bind(VertexBuffer buffer)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.handle);
        }

        private void Update()
        {
            Bind(this);
            var verts = vertices.ToArray();
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length, verts,
                BufferUsageHint.StreamDraw);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(handle);
        }

        public void AddVertex(Vertex vertex)
        {
            vertices.Add(vertex);
            Update();
        }

        public void AddVertices(IEnumerable<Vertex> verts)
        {
            vertices.AddRange(verts);
            Update();
        }

        public void DeleteVertices(int index, int count = 1)
        {
            vertices.RemoveRange(index, count);
            Update();
        }

        public void AddVertices(params Vertex[] verts)
        {
            vertices.AddRange(verts);
            Update();
        }

        public void SetVertices(params Vertex[] verts)
        {
            vertices.Clear();
            vertices.AddRange(verts);
        }

        public Vertex this[int index]
        {
            get => vertices[index];
            set
            {
                if (vertices[index] == value) return;
                vertices[index] = value;
                Update();
            }
        }

        public int Length => vertices.Count;
    }
}