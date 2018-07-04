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
using iLynx.Common;
using iLynx.Graphics.shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics
{
    public abstract class Geometry : Transformable, IDrawable
    {
        private readonly VertexArrayObject<Vertex> fillVao = new VertexArrayObject<Vertex>();
        private readonly VertexArrayObject<Vertex> outlineVao = new VertexArrayObject<Vertex>();
        private readonly VertexBufferObject<Vertex> outlineBuffer;
        private readonly VertexBufferObject<Vertex> fillBuffer;
        private readonly VertexBufferObject<uint> indexBuffer;
        private readonly VertexBufferObject<uint> outlineIndexBuffer;
        private readonly RectangleGeometry originRect;

        private Color fillColor;

        private readonly bool showOrigin;
        //private readonly VertexBufferObject<Vertex> outlineBuffer = new VertexBufferObject<Vertex>(4) { PrimitiveType = PrimitiveType.LineLoop };

        protected Geometry(Color fillColor, Color borderColor, float borderThickness, bool isFixedSize = false, int length = 0, bool showOrigin = false)
        {
            this.fillColor = fillColor;
            this.showOrigin = showOrigin;
            BorderColor = borderColor;
            BorderThickness = borderThickness;
            fillBuffer = isFixedSize
                ? new VertexBufferObject<Vertex>(length, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw)
                : new VertexBufferObject<Vertex>(0, BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);
            indexBuffer = new VertexBufferObject<uint>(0, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            fillVao.AttachVertexBuffer(fillBuffer, indexBuffer);
            if (showOrigin)
            {
                originRect = new RectangleGeometry(20f, 20f, Color.Lime, false);
                originRect.Origin = new Vector3(originRect.Width / 2f, originRect.Height / 2f, 0f);
            }
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

        public Color BorderColor { get; set; }
        public float BorderThickness { get; set; }
        public Texture Texture { get; set; }
        public Shader Shader { get; set; } = Shader.DefaultShaderProgram;
        //public Matrix4 Transform { get; set; } = Matrix4.Identity;
        protected abstract PrimitiveType PrimitiveType { get; }

        protected void Update()
        {
            var verts = GetVertices();
            if (verts.Length < 3) return;
            fillBuffer.SetData(verts);
            indexBuffer.SetData(GetIndices());
        }

        protected abstract Vertex[] GetVertices();

        private uint[] GetIndices()
        {
            return 0u.To((uint)fillBuffer.Length - 1);
        }

        public void Dispose()
        {
            fillVao.Dispose();
            outlineVao.Dispose();
            //fillBuffer?.Dispose();
            //indexBuffer?.Dispose();
        }

        public void Draw(IDrawingContext target)
        {
            target.UseShader(Shader = Shader ?? target.ActiveShader);
            target.BindTexture(Texture);
            Shader?.SetTransform(Transform);
            fillVao.Bind();
            GL.DrawElements(PrimitiveType, fillBuffer.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            if (originRect != null && showOrigin)
            {
                //originRect.Transform = Transform.ClearScale();
                originRect.Rotation = Rotation;
                originRect.Translation = Translation;
                originRect.Draw(target);
            }

            fillVao.Unbind();
            //var transformLocation = Shader.GetUniformLocation("transform");
            //var transform = Transform;
            //GL.UniformMatrix4(transformLocation, false, ref transform);

            //context.Draw(vao);
        }
    }
}