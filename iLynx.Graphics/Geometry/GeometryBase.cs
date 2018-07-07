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
using iLynx.Graphics.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Geometry
{
    public abstract class GeometryBase : Transformable, IGeometry
    {
        private readonly VertexArrayObject<Vertex> fillVao;
        private readonly VertexBufferObject<Vertex> fillBuffer;
        private readonly VertexBufferObject<uint> indexBuffer;
        //private readonly RectangleGeometry originRect;
        private Color fillColor;
        private readonly bool showOrigin;

        protected GeometryBase(Color fillColor, bool isFixedSize = false, int vertexCount = 0, bool showOrigin = false)
        {
            this.fillColor = fillColor;
            this.showOrigin = showOrigin;
            fillBuffer = isFixedSize
                ? new VertexBufferObject<Vertex>(vertexCount, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw)
                : new VertexBufferObject<Vertex>(0, BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);
            indexBuffer = new VertexBufferObject<uint>(0, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            fillVao = new VertexArrayObject<Vertex>();
            fillVao.AttachVertexBuffer(fillBuffer, indexBuffer);
            //if (showOrigin)
            //{
            //    originRect = new RectangleGeometry(20f, 20f, Color.Lime, false);
            //    originRect.Origin = new Vector3(originRect.Width / 2f, originRect.Height / 2f, 0f);
            //}
        }

        public Color FillColor
        {
            get => fillColor;
            set
            {
                if (value == fillColor) return;
                fillColor = value;
                fillBuffer.Transform((ref Vertex v) => v.VertexColor = value);
            }
        }

        public Texture Texture { get; set; }
        public Shader Shader { get; set; } = Shader.DefaultShaderProgram;
        protected abstract PrimitiveType PrimitiveType { get; }

        protected void Update()
        {
            fillBuffer.SetData(GetVertices());
            indexBuffer.SetData(GetIndices());
        }

        protected abstract Vertex[] GetVertices();

        protected abstract uint[] GetIndices();

        public void Dispose()
        {
            fillVao.Dispose();
        }

        public DrawCall<Vertex> CreateDrawCall()
        {
            return new DrawCall<Vertex>(Transform, PrimitiveType, fillVao, indexBuffer.Length);
        }
    }
}