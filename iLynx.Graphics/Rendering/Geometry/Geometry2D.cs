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

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering.Geometry
{
    public abstract class Geometry2D : IDrawable
    {
        private readonly VertexArrayObject<Vertex2> vao = new VertexArrayObject<Vertex2>();
        private Buffer<Vertex2> fillBuffer;
        private Buffer<uint> indexBuffer;
        //private readonly Buffer<Vertex2> outlineBuffer = new Buffer<Vertex2>(4) { PrimitiveType = PrimitiveType.LineLoop };

        protected Geometry2D(bool isFixedSize = false, int length = 0)
        {
            if (!isFixedSize)
                fillBuffer = new Buffer<Vertex2>(0);
            else // TODO: Make this use a single VBO to store all the "static / fixed size" geometries
                fillBuffer = new Buffer<Vertex2>(length);
            vao.BindVertexBuffer(fillBuffer);
        }

        public Color FillColor
        {
            get;
            set;
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
            if (!fillBuffer.IsInitialized)
                fillBuffer.Initialize();
            fillBuffer.SetVertices(verts);
        }

        protected abstract Vertex2[] GetVertices();

        public void Dispose()
        {
            fillBuffer?.Dispose();
        }

        public void Draw(IRenderContext context)
        {
            //var transformLocation = Shader.GetUniformLocation("transform");
            //var transform = Transform;
            //GL.UniformMatrix4(transformLocation, false, ref transform);

            //context.Draw(vao);
        }
    }
}