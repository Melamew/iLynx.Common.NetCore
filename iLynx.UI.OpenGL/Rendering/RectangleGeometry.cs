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

namespace iLynx.UI.OpenGL.Rendering
{
    public class RectangleGeometry : Geometry
    {
        private float width, height;
        private Color color;

        public Color Color
        {
            get => color;
            set
            {
                if (value == color) return;
                color = value;
                GenerateVertices();
            }
        }

        public float Width
        {
            get => width;
            set
            {
                if (MathF.Abs(value - width) <= float.Epsilon) return;
                width = value;
                GenerateVertices();
            }
        }

        public float Height
        {
            get => height;
            set
            {
                if (MathF.Abs(value - height) <= float.Epsilon) return;
                height = value;
                GenerateVertices();
            }
        }

        private void GenerateVertices()
        {
            float w = width, h = height;
            ClearVertices();
            AddVertex(new Vector2(0f, 0f), color);
            AddVertex(new Vector2(0f, h), color);
            AddVertex(new Vector2(w, 0f), color);
            AddVertex(new Vector2(w, h), color);
        }

        public RectangleGeometry(float width, float height, Color color)
        {
            this.width = width;
            this.height = height;
            this.color = color;
            PrimitiveType = PrimitiveType.TriangleStrip;
            GenerateVertices();
        }

        public RectangleGeometry(SizeF dimensions, Color color)
            : this(dimensions.Width, dimensions.Height, color) { }
    }
}