﻿#region LICENSE
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
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering
{
    public class Buffer<TElement> : IDisposable where TElement : struct, IEquatable<TElement>
    {
        private int handle;
        private TElement[] vertices = new TElement[0];
        protected static readonly int ElementSize = Marshal.SizeOf<TElement>();

        private Buffer()
        {
        }

        public void Initialize()
        {
            if (0 != handle) return;
            handle = GL.GenBuffer();
            Update();
        }

        public Buffer(int capacity)
            : this()
        {
            Array.Resize(ref vertices, capacity);
        }

        public Buffer(params TElement[] vertices)
            : this()
        {
            this.vertices = vertices;
        }

        public bool IsInitialized => handle != 0;

        public BufferUsageHint BufferUsage { get; set; } = BufferUsageHint.StaticDraw;

        public BufferTarget Target { get; set; } = BufferTarget.ArrayBuffer;

        public void Bind()
        {
            if (0 == handle) throw new NotInitializedException();
            GL.BindBuffer(Target, handle);
        }

        private void Update()
        {
            if (0 == handle) return;
            Bind();
            var verts = vertices;
            GL.BufferData(Target, ElementSize * verts.Length, verts,
                BufferUsage);
            Unbind();
        }

        private void UpdateSubData(int offset, int length)
        {
            if (0 == handle) return;
            Bind();
            var verts = new TElement[length];
            Array.Copy(vertices, offset, verts, 0, length);
            GL.BufferSubData(Target, (IntPtr)(offset * ElementSize), length * ElementSize, verts);
            Unbind();
        }

        public void Unbind()
        {
            GL.BindBuffer(Target, 0);
        }

        public void Dispose()
        {
            if (0 == handle) return;
            GL.DeleteBuffer(handle);
        }

        public void SetVertices(params TElement[] verts)
        {
            if (null == verts) throw new ArgumentNullException(nameof(verts));
            Array.Resize(ref vertices, verts.Length);
            Array.Copy(verts, vertices, vertices.Length);
            Update();
        }

        public void SetVertices(int startIndex, params TElement[] verts)
        {
            var end = startIndex + verts.Length;
            if (end >= Length)
                Array.Resize(ref vertices, end);
            Array.Copy(verts, 0, vertices, startIndex, verts.Length);
            UpdateSubData(startIndex, verts.Length);
        }

        public void AddVertices(params TElement[] verts)
        {
            SetVertices(Length - 1, verts);
        }

        public TElement this[int index]
        {
            get => vertices[index];
            set
            {
                if (vertices[index].Equals(value)) return;
                vertices[index] = value;
                Update();
            }
        }

        public TElement[] this[int index, int count]
        {
            get => vertices.Skip(index).Take(count).ToArray();
            set => SetVertices(index, value.Take(count).ToArray());
        }

        public int Length => vertices.Length;
    }

    public class NotInitializedException : Exception
    {
    }
}