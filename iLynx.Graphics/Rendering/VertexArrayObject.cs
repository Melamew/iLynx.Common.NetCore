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
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using static iLynx.Graphics.GLCheck;

namespace iLynx.Graphics.Rendering
{
    public class VertexArrayObject<TVertex> : IDisposable where TVertex : struct, IEquatable<TVertex>, IVAOElement
    {
        private readonly int handle;
        protected static readonly int ElementSize = Marshal.SizeOf<TVertex>();
        private VertexBufferObject<TVertex> vertexBuffer;
        private VertexBufferObject<uint> indexBuffer;

        public VertexArrayObject()
        {
            handle = Check(GL.GenVertexArray);
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

        public void AttachVertexBuffer(VertexBufferObject<TVertex> buffer, VertexBufferObject<uint> indices = null)
        {
            if (0 == handle) throw new NotInitializedException();
            vertexBuffer?.Dispose();
            vertexBuffer = buffer;
            indexBuffer?.Dispose();
            indexBuffer = indices;
            Bind();
            vertexBuffer.Bind();
            indexBuffer?.Bind();
            SetupAttributes();
            Unbind();
            vertexBuffer.Unbind();
            indexBuffer?.Unbind();
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
            indexBuffer?.Dispose();
            GL.DeleteVertexArray(handle);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void SetupAttributes()
        {
            default(TVertex).SetupVertexAttributePointers();
        }
    }
}
