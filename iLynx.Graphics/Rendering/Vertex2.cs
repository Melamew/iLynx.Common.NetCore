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

namespace iLynx.Graphics.Rendering
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex2 : IEquatable<Vertex2>, IVertex
    {
        public readonly Vector2 Position;
        public readonly Color Color;
        public readonly Vector2 TexCoord;
        
        public Vertex2(Vector2 position, Color color, Vector2 texCoord)
        {
            Position = position;
            Color = color;
            TexCoord = texCoord;
        }

        //public void SetupVertexAttributes()
        //{
        //    GL.EnableVertexAttribArray(0);
        //    GL.EnableVertexAttribArray(1);
        //    GL.EnableVertexAttribArray(2);
        //    GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex2>(), 0);
        //    GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Byte, false, Marshal.SizeOf<Vertex2>(),
        //        Marshal.SizeOf<Vector2>());
        //    GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex2>(),
        //        Marshal.SizeOf<Vector2>() + Marshal.SizeOf<Color>());
        //}

        public Vertex2(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
            TexCoord = new Vector2();
        }

        public Vertex2(Vector2 position)
        {
            Position = position;
            Color = Color.Transparent;
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
            return Position == other.Position && Color == other.Color && TexCoord == other.TexCoord;
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
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                hashCode = (hashCode * 397) ^ TexCoord.GetHashCode();
                return hashCode;
            }
        }

        public VertexAttribute[] GetVertexAttributes()
        {
            return new[]
            {
                // The first attribute is a Vector2, so 2 glFloats, offset is 0 as this is the first attribute, and we don't want our data to be normalized
                new VertexAttribute { Count = 2, GLType = VertexAttribPointerType.Float, ByteOffset = 0, Normalized = false },
                // The second attribute is the color of the vertex, 4 glUnsignedBytes, offset is simply the size of the first attribute in bytes, and again, we don't want our data normalized
                new VertexAttribute { Count = 4, GLType = VertexAttribPointerType.UnsignedByte, ByteOffset = Marshal.SizeOf<Vector2>(), Normalized = false },
                // Our third attribute are the texture coordinates of the vertex, same as the first attribute, but offset is the size of a vector2 + 4 bytes (size of color).
                new VertexAttribute { Count = 2, GLType = VertexAttribPointerType.Float, ByteOffset = Marshal.SizeOf<Vector2>() + 4, Normalized = false }
            };
        }
    }
}