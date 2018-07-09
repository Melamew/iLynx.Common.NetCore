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

namespace iLynx.Graphics.Drawing
{
    public class Cuboid : GeometryBase
    {
        private readonly Vector3 dimensions;
        private readonly Vertex[] vertices = new Vertex[8];

        public Cuboid(Color32 fillColor, Vector3 dimensions, bool showOrigin = false) : base(fillColor, true, 8, showOrigin)
        {
            this.dimensions = dimensions;
            Update();
        }

        public Cuboid(Color32 fillColor, float width, float height, float depth, bool showOrigin = false) : this(
            fillColor, new Vector3(width, height, depth), showOrigin)
        { }

        protected override PrimitiveType PrimitiveType => PrimitiveType.Triangles;

        private static readonly uint[] Indices =
        {
            // Front Face
            0, 1, 3,
            3, 2, 0,
            // Left Face
            0, 2, 6,
            6, 4, 0,
            // Bottom Face
            4, 5, 1,
            1, 0, 4,
            // Back Face
            4, 6, 7,
            7, 5, 4,
            // Top Face
            7, 6, 2,
            2, 3, 7,
            // Right Face
            7, 3, 1,
            1, 5, 7
        };
        protected override Vertex[] GetVertices()
        {
            vertices[0] = new Vertex(new Vector3(0f, 0f, 0f), FillColor); // Front Bottom Left
            vertices[1] = new Vertex(new Vector3(dimensions.X, 0f, 0f), FillColor); // Front Bottom Right
            vertices[2] = new Vertex(new Vector3(0f, dimensions.Y, 0f), FillColor); // Front Top Left
            vertices[3] = new Vertex(new Vector3(dimensions.X, dimensions.Y, 0f), FillColor); // Front Top Right

            vertices[4] = new Vertex(new Vector3(0f, 0f, dimensions.Z), FillColor); // Back Bottom Left
            vertices[5] = new Vertex(new Vector3(dimensions.X, 0f, dimensions.Z), FillColor); // Back Bottom Right
            vertices[6] = new Vertex(new Vector3(0f, dimensions.Y, dimensions.Z), FillColor); // Back Top Left
            vertices[7] = new Vertex(new Vector3(dimensions.X, dimensions.Y, dimensions.Z), FillColor); // Back Top Right
            return vertices;
        }

        protected override uint[] GetIndices()
        {
            return Indices;
        }
    }
}
