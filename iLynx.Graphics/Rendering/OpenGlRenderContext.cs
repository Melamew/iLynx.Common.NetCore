using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering
{
    public class OpenGlRenderContext : IRenderContext
    {
        public void Bind<TVertex>(VertexBuffer<TVertex> buffer) where TVertex : struct, IEquatable<TVertex>
        {
            throw new NotImplementedException();
        }

        public void UseShader(ShaderProgram program)
        {
            throw new NotImplementedException();
        }

        public void BindTexture(Texture texture)
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void Draw<TVertex>(VertexBuffer<TVertex> vertexBuffer) where TVertex : struct, IEquatable<TVertex>
        {
            VertexBuffer<TVertex>.BindBuffer(vertexBuffer);
            GL.DrawArrays(vertexBuffer.PrimitiveType, 0, vertexBuffer.Length);
            VertexBuffer<TVertex>.BindBuffer(null);
        }
    }
}
