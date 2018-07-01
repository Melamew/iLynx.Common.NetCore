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
using System.Reflection;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering
{
    public abstract class VertexArrayObject<TVertex> : IDisposable where TVertex : struct, IEquatable<TVertex>, IVertex
    {
        private int handle;
        private readonly List<VertexBuffer<TVertex>> buffers = new List<VertexBuffer<TVertex>>();

        protected VertexArrayObject()
        {
        }

        public void Initialize()
        {
            if (0 != handle) return;
            handle = GL.GenVertexArray();
            GL.BindVertexArray(handle);
            SetupAttributes();
        }

        ~VertexArrayObject()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (handle == 0) return;
            GL.DeleteVertexArray(handle);
            buffers.ForEach(x => x.Dispose());
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
                GL.EnableVertexAttribArray(i);
                GL.VertexAttribPointer(i, attributes[i].Count, attributes[i].GLType, attributes[i].Normalized, stride, attributes[i].ByteOffset);
            }
            //GL.EnableVertexAttribArray(0);
            //GL.EnableVertexAttribArray(1);
            //GL.EnableVertexAttribArray(2);
            //GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex2>(), 0);
            //GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Byte, false, Marshal.SizeOf<Vertex2>(),
            //    Marshal.SizeOf<Vector2>());
            //GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex2>(),
            //    Marshal.SizeOf<Vector2>() + Marshal.SizeOf<Color>());

        }
    }
}
