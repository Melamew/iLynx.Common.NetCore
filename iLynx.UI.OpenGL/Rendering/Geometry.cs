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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Rendering
{
    public class Texture
    {

    }

    public class Shader
    {

    }

    public class ShaderProgram
    {

    }

    public abstract class Geometry// : Drawable
    {
        private readonly VertexBuffer buffer = new VertexBuffer();

        protected virtual void AddVertex(Vector2 position, Color color)
        {
            AddVertex(new Vertex(position, color));
        }

        protected virtual void DeleteVertex(int index)
        {
            buffer.RemoveAt(index);
        }

        protected virtual void ClearVertices()
        {
            //vertices.Clear();
        }

        protected virtual void AddVertex(params Vertex[] v)
        {
            //vertices.AddRange(v);
        }

        public void Draw(IRenderTarget target)
        {
            target.Draw(buffer, PrimitiveType);
        }
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }
        public float BorderThickness { get; set; }
        public float CornerRadius { get; set; }
        public Texture Texture { get; set; }
        public ShaderProgram Shader { get; set; }
        public PrimitiveType PrimitiveType { get; protected set; }
    }

    public struct Vertex : IEquatable<Vertex>
    {
        public Vector2 Position;
        public Color Color;
        public Vector2 TexCoord;
        public Vertex(Vector2 position, Color color, Vector2 texCoord)
        {
            Position = position;
            Color = color;
            TexCoord = texCoord;
        }

        public Vertex(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
            TexCoord = new Vector2();
        }

        public Vertex(Vector2 position)
        {
            Position = position;
            Color = Color.Transparent;
            TexCoord = new Vector2();
        }

        public static bool operator ==(Vertex left, Vertex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vertex left, Vertex right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Vertex other)
        {
            return Position.Equals(other.Position) && Color.Equals(other.Color) && TexCoord.Equals(other.TexCoord);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vertex vertex && Equals(vertex);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                hashCode = (hashCode * 397) ^ TexCoord.GetHashCode();
                return hashCode;
            }
        }
    }
}