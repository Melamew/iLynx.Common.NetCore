#region LICENSE
/*
 * Copyright 2018 Melanie Hjorth
 *
 * Redistribution and use in source and binary forms,
 * with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote
 * products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using iLynx.Graphics.Geometry;
using iLynx.Graphics.Rendering.Geometry;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering
{
    public class VertexBuffer<TVertex> : IDisposable where TVertex : struct, IEquatable<TVertex>
    {
        private int handle;
        private TVertex[] vertices = new TVertex[0];
        private bool dirty = true;

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
            if (!buffer.dirty) return;
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

        public void SetVertices(params TVertex[] verts)
        {
            if (null == verts) throw new ArgumentNullException(nameof(verts));
            Array.Resize(ref vertices, verts.Length);
            Array.Copy(verts, vertices, vertices.Length);
            dirty = true;
        }

        public void ReplaceVertices(int startIndex, params TVertex[] verts)
        {
            var end = startIndex + verts.Length;
            if (end >= Length)
                Array.Resize(ref vertices, end);
            Array.Copy(verts, 0, vertices, startIndex, verts.Length);
            dirty = true;
        }

        public void AddVertices(params TVertex[] verts)
        {
            ReplaceVertices(Length - 1, verts);
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