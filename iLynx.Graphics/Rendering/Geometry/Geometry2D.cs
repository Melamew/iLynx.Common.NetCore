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

using iLynx.Common;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering.Geometry
{
    public abstract class Geometry2D : IDrawable
    {
        private readonly VertexArrayObject<Vertex2> vao = new VertexArrayObject<Vertex2>();
        private readonly VertexBufferObject<Vertex2> fillBuffer;
        private readonly VertexBufferObject<uint> indexBuffer;

        private Color fillColor;
        //private readonly VertexBufferObject<Vertex2> outlineBuffer = new VertexBufferObject<Vertex2>(4) { PrimitiveType = PrimitiveType.LineLoop };

        protected Geometry2D(bool isFixedSize = false, int length = 0)
        {
            fillBuffer = isFixedSize
                ? new VertexBufferObject<Vertex2>(length, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw)
                : new VertexBufferObject<Vertex2>(0, BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);
            indexBuffer = new VertexBufferObject<uint>(0, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            vao.AttachVertexBuffer(fillBuffer, indexBuffer);
        }

        public Color FillColor
        {
            get => fillColor;
            set
            {
                if (value == fillColor) return;
                fillColor = value;
                fillBuffer.BackingArray.Transform(x =>
                {
                    x.VertexColor = value;
                    return x;
                });
                fillBuffer.UpdateGpuData();
            }
        }

        public Color BorderColor { get; set; }
        public float BorderThickness { get; set; }
        public Texture Texture { get; set; }
        public ShaderProgram Shader { get; set; }
        public Matrix4 Transform { get; set; }
        protected abstract PrimitiveType PrimitiveType { get; }

        protected virtual void Update()
        {
            var verts = GetVertices();
            if (verts.Length < 3) return;
            fillBuffer.SetVertices(verts);
        }

        protected abstract Vertex2[] GetVertices();

        protected virtual uint[] GetIndices()
        {
            return 
        }

        public void Dispose()
        {
            fillBuffer?.Dispose();
        }

        public void Draw(IRenderTarget target)
        {
            //var transformLocation = Shader.GetUniformLocation("transform");
            //var transform = Transform;
            //GL.UniformMatrix4(transformLocation, false, ref transform);

            //context.Draw(vao);
        }
    }
}