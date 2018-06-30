using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering
{
    public class VertexBuffer<TVertex> : IDisposable where TVertex : struct, IEquatable<TVertex>
    {
        private int handle;
        private TVertex[] vertices = new TVertex[0];

        private VertexBuffer()
        {
        }

        public void Initialize()
        {
            handle = GL.GenBuffer();
            Update(this);
        }

        public VertexBuffer(int capacity)
            : this()
        {
            Array.Resize(ref vertices, capacity);
        }

        public VertexBuffer(params TVertex[] vertices)
            : this()
        {
            SetVertices(vertices);
        }

        public bool IsInitialized => handle != 0;

        public PrimitiveType PrimitiveType { get; set; } = PrimitiveType.TriangleFan;
        public BufferUsageHint BufferUsage { get; set; } = BufferUsageHint.StreamDraw;

        public static void BindBuffer(VertexBuffer<TVertex> buffer)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer?.handle ?? 0);
        }

        private static void Update(VertexBuffer<TVertex> buffer)
        {
            if (0 == buffer.handle) return; // Buffer has not been initialized
            BindBuffer(buffer);
            var verts = buffer.vertices;
            GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf<Vertex2>() * verts.Length, verts,
                buffer.BufferUsage);
            BindBuffer(null);
        }

        public void Dispose()
        {
            if (0 == handle) return;
            GL.DeleteBuffer(handle);
        }

        //public void AddVertex(TVertex vertex)
        //{
        //    vertices.Add(vertex);
        //    Update();
        //}

        //public void AddVertices(IEnumerable<TVertex> verts)
        //{
        //    vertices.AddRange(verts);
        //    Update();
        //}

        //public void DeleteVertices(int index, int count = 1)
        //{
        //    vertices.RemoveRange(index, count);
        //    Update();
        //}

        //public void AddVertices(params TVertex[] verts)
        //{
        //    vertices.AddRange(verts);
        //    Update();
        //}

        public void SetVertices(params TVertex[] verts)
        {
            if (null == verts) throw new ArgumentNullException(nameof(verts));
            Array.Resize(ref vertices, verts.Length);
            Array.Copy(verts, vertices, vertices.Length);
        }

        public void ReplaceVertices(int startIndex, params TVertex[] verts)
        {
            var end = startIndex + verts.Length;
            if (end >= Length)
                Array.Resize(ref vertices, end);
            Array.Copy(verts, 0, vertices, startIndex, verts.Length);
        }

        public TVertex this[int index]
        {
            get => vertices[index];
            set
            {
                if (vertices[index].Equals(value)) return;
                vertices[index] = value;
                Update(this);
            }
        }

        public int Length => vertices.Length;
    }
}