using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering
{
    public class VertexArrayObject<TVertex> : IDisposable where TVertex : struct, IEquatable<TVertex>
    {
        //private readonly int handle;
        //private readonly List<VertexBuffer<TVertex>> buffers = new List<VertexBuffer<TVertex>>();

        public VertexArrayObject()
        {
            throw new NotImplementedException();
            //handle = GL.GenVertexArray();
        }

        ~VertexArrayObject()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            //if (disposing)
            //    GL.DeleteVertexArray(handle);
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
