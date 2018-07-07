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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Geometry
{
    public class RectangleGeometry : GeometryBase
    {
        private readonly Vertex[] vertices = new Vertex[4];
        private float width, height;
        private static readonly uint[] Indices = {0u, 1u, 2u, 3u};

        public float Width
        {
            get => width;
            set
            {
                if (MathF.Abs(value - width) <= float.Epsilon) return;
                width = value;
                Update();
            }
        }

        public float Height
        {
            get => height;
            set
            {
                if (MathF.Abs(value - height) <= float.Epsilon) return;
                height = value;
                Update();
            }
        }

        protected override PrimitiveType PrimitiveType => PrimitiveType.TriangleFan;

        protected override Vertex[] GetVertices()
        {
            vertices[0] = new Vertex(FillColor);
            vertices[1] = new Vertex(new Vector3(0f, height, 0f), FillColor);
            vertices[2] = new Vertex(new Vector3(width, height, 0f), FillColor);
            vertices[3] = new Vertex(new Vector3(width, 0f, 0f), FillColor);
            return vertices;
        }

        protected override uint[] GetIndices()
        {
            return Indices;
        }

        public RectangleGeometry(float width, float height, Color fillColor, bool showOrigin = false)
            : base(fillColor, true, 4, showOrigin)
        {
            this.width = width;
            this.height = height;
            Update();
        }

        public RectangleGeometry(SizeF dimensions, Color color, bool showOrigin = false)
            : this(dimensions.Width, dimensions.Height, color, showOrigin)
        {

        }
    }
}