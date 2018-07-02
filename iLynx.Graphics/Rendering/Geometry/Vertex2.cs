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
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex2 : IEquatable<Vertex2>, IVAOElement
    {
        public readonly Vector2 Position;
        public readonly Vector2 TexCoord;
        public readonly Color VertexColor;
        
        public Vertex2(Vector2 position, Color vertexColor, Vector2 texCoord)
        {
            Position = position;
            VertexColor = vertexColor;
            TexCoord = texCoord;
        }

        public Vertex2(Vector2 position, Color vertexColor)
        {
            Position = position;
            VertexColor = vertexColor;
            TexCoord = new Vector2();
        }

        public Vertex2(Vector2 position)
        {
            Position = position;
            VertexColor = Color.Transparent;
            TexCoord = new Vector2();
        }

        public static bool operator ==(Vertex2 left, Vertex2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vertex2 left, Vertex2 right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Vertex2 other)
        {
            return Position == other.Position && VertexColor == other.VertexColor && TexCoord == other.TexCoord;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Vertex2 vertex && Equals(vertex);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ VertexColor.GetHashCode();
                hashCode = (hashCode * 397) ^ TexCoord.GetHashCode();
                return hashCode;
            }
        }

        public void SetupVertexAttributePointers()
        {
            var size = Marshal.SizeOf<Vector2>();
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, size, 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, size, Marshal.SizeOf<Vector2>());
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.UnsignedByte, true, size, Marshal.SizeOf<Vector2>() * 2);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            //return new[]
            //{
            //    // The first attribute is a Vector2, so 2 glFloats, offset is 0 as this is the first attribute, and we don't want our data to be normalized
            //    new VertexAttribute { Count = 2, GLType = VertexAttribPointerType.Float, ByteOffset = 0, Normalized = false },
            //    // The second attribute is the color of the vertex, 4 , offset is simply the size of the first attribute in bytes (sizeOf(Vector2)), and again, we don't want our data normalized
            //    new VertexAttribute { Count = 4, GLType = VertexAttribPointerType.Float, ByteOffset = Marshal.SizeOf<Vector2>(), Normalized = false },
            //    // Our third attribute are the texture coordinates of the vertex, same as the first attribute, but offset is sizeOf(vector2) + sizeOf(vector4).
            //    new VertexAttribute { Count = 2, GLType = VertexAttribPointerType.Float, ByteOffset = Marshal.SizeOf<Vector2>() + Marshal.SizeOf<Vector4>(), Normalized = false }
            //};
        }
    }
}