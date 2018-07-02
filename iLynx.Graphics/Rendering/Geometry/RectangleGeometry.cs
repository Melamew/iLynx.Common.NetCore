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
using iLynx.Graphics.Rendering.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Geometry
{
    public class RectangleGeometry : Geometry2D
    {
        private readonly Vertex2[] vertices = new Vertex2[4];
        private float width, height;

        public float Width
        {
            get => width;
            set
            {
                if (MathF.Abs(value - width) <= float.Epsilon) return;
                width = value;
            }
        }

        public float Height
        {
            get => height;
            set
            {
                if (MathF.Abs(value - height) <= float.Epsilon) return;
                height = value;
            }
        }

        protected override PrimitiveType PrimitiveType => PrimitiveType.TriangleFan;

        protected override Vertex2[] GetVertices()
        {
            return vertices;
        }

        public RectangleGeometry(float width, float height, Color fillColor)
            : base(true, 4)
        {
            this.width = width;
            this.height = height;
            FillColor = fillColor;
            vertices[0] = new Vertex2(fillColor);
            vertices[1] = new Vertex2(new Vector2(0f, height), fillColor);
            vertices[2] = new Vertex2(new Vector2(width, height), fillColor);
            vertices[3] = new Vertex2(new Vector2(width, 0f), fillColor);
        }

        public RectangleGeometry(SizeF dimensions, Color color)
            : this(dimensions.Width, dimensions.Height, color)
        {

        }
    }
}