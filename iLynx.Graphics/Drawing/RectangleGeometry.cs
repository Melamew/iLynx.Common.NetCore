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

namespace iLynx.Graphics.Drawing
{
    public class RectangleGeometry : GeometryBase
    {
        private readonly Vertex[] m_vertices = new Vertex[4];
        private float m_width, m_height;
        private static readonly uint[] s_Indices = { 0u, 1u, 2u, 3u };

        public float Width
        {
            get => m_width;
            set
            {
                if (MathF.Abs(value - m_width) <= float.Epsilon) return;
                m_width = value;
                Update();
            }
        }

        public float Height
        {
            get => m_height;
            set
            {
                if (MathF.Abs(value - m_height) <= float.Epsilon) return;
                m_height = value;
                Update();
            }
        }

        public RectangleF TextureRect { get; set; } = new RectangleF(0f, 0f, 1f, 1f);

        protected override PrimitiveType PrimitiveType => PrimitiveType.TriangleFan;

        protected override Vertex[] GetVertices()
        {
            m_vertices[0] = new Vertex(Vector3.Zero, FillColor, new Vector2(TextureRect.X, TextureRect.Y));
            m_vertices[1] = new Vertex(new Vector3(0f, m_height, 0f), FillColor, new Vector2(TextureRect.X, TextureRect.Bottom));
            m_vertices[2] = new Vertex(new Vector3(m_width, m_height, 0f), FillColor, new Vector2(TextureRect.Right, TextureRect.Bottom));
            m_vertices[3] = new Vertex(new Vector3(m_width, 0f, 0f), FillColor, new Vector2(TextureRect.Right, TextureRect.Y));
            return m_vertices;
        }

        protected override uint[] GetIndices()
        {
            return s_Indices;
        }

        public RectangleGeometry(float width, float height, Color32 fillColor, bool showOrigin = false)
            : base(fillColor, true, 4, showOrigin)
        {
            m_width = width;
            m_height = height;
            Update();
        }

        public RectangleGeometry(SizeF dimensions, Color32 color, bool showOrigin = false)
            : this(dimensions.Width, dimensions.Height, color, showOrigin)
        {

        }
    }
}