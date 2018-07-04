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
using iLynx.Graphics.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics
{
    public class RenderBatch
    {
        private readonly List<DrawCall<Vertex>> drawCalls = new List<DrawCall<Vertex>>();
        public Texture Texture { get; set; }

        public Shader Shader { get; set; }

        public void AddCall(DrawCall<Vertex> call)
        {
            drawCalls.Add(call);
        }

        public void RermoveCall(DrawCall<Vertex> call)
        {
            drawCalls.Remove(call);
        }

        public void Execute(Matrix4 viewTransform)
        {
            if (null == Shader) throw new InvalidOperationException("Cannot draw anything without a shader");
            Shader.ViewTransform = viewTransform;
            Shader.Activate();
            foreach (var call in drawCalls)
                call.Execute(Shader);
        }
    }

    public class DrawCall<TVertex> where TVertex : struct, IVAOElement, IEquatable<TVertex>
    {
        private readonly Matrix4 transform;
        private readonly VertexArrayObject<TVertex> vertexArrayObject;
        private readonly int vertexCount;
        private readonly PrimitiveType primitiveType;

        public DrawCall(Matrix4 transform, PrimitiveType primitiveType, VertexArrayObject<TVertex> vertexArrayObject,
            int vertexCount)
        {
            this.transform = transform;
            this.primitiveType = primitiveType;
            this.vertexArrayObject = vertexArrayObject;
            this.vertexCount = vertexCount;
        }

        public void Execute(Shader shader)
        {
            vertexArrayObject.Bind();
            shader.SetTransform(transform);
            GL.DrawElements(primitiveType, vertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            vertexArrayObject.Unbind();
        }
    }

    public interface IView : ITransformable
    {
        /// <summary>
        /// Gets or Sets the projection matrix of this view
        /// </summary>
        Matrix4 Projection { get; set; }

        /// <summary>
        /// Prepares this view for rendering
        /// </summary>
        void PrepareRender();

        /// <summary>
        /// Renders this view
        /// </summary>
        void Render();

        /// <summary>
        /// Adds the specified drawable to this view
        /// </summary>
        /// <param name="drawable"></param>
        void AddDrawable(IDrawable drawable);

        /// <summary>
        /// Removes the specified drawable from this view
        /// </summary>
        /// <param name="drawable"></param>
        void RemoveDrawable(IDrawable drawable);
    }
}
