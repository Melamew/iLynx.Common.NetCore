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
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering
{
    public class VertexArrayObject<TVertex> : IDisposable where TVertex : struct, IEquatable<TVertex>, IVAOElement
    {
        private int handle;
        private Buffer<TVertex> vertexBuffer;
        private Buffer<uint> indexBuffer;

        private void Initialize()
        {
            if (0 != handle) return;
            handle = GL.GenVertexArray();
        }

        ~VertexArrayObject()
        {
            Dispose(false);
        }

        public void Bind()
        {
            if (0 == handle) throw new NotInitializedException();
            GL.BindVertexArray(handle);
        }

        public void BindVertexBuffer(Buffer<TVertex> buffer, Buffer<uint> indices = null)
        {
            vertexBuffer?.Dispose();
            vertexBuffer = buffer;
            indexBuffer?.Dispose();
            indexBuffer = indices;
            if (0 == handle) Initialize();
            Bind();
            vertexBuffer.Bind();
            SetupAttributes();
            indices?.Bind();
            Unbind();
            vertexBuffer.Unbind();
            indices?.Unbind();
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (handle == 0) return;
            vertexBuffer?.Dispose();
            GL.DeleteVertexArray(handle);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void SetupAttributes()
        {
            var stride = Marshal.SizeOf<TVertex>();
            var attributes = default(TVertex).GetVertexAttributes();
            for (var i = 0; i < attributes.Length; ++i)
            {
                GL.VertexAttribPointer(i, attributes[i].Count, attributes[i].GLType, attributes[i].Normalized, stride, attributes[i].ByteOffset);
                GL.EnableVertexAttribArray(i);
            }
        }
    }
}
