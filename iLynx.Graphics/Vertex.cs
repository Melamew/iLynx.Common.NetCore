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
using static OpenTK.Graphics.OpenGL.GL;

namespace iLynx.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex : IEquatable<Vertex>, IVAOElement
    {
        public readonly Vector3 Position;
        public readonly Vector2 TexCoord;
        public readonly Color32 VertexColor;
        
        public Vertex(Vector3 position, Color32 vertexColor, Vector2 texCoord)
        {
            Position = position;
            VertexColor = vertexColor;
            TexCoord = texCoord;
        }

        public Vertex(Vector3 position, Color32 vertexColor)
        {
            Position = position;
            TexCoord = new Vector2();
            VertexColor = vertexColor;
        }

        public Vertex(Vector3 position)
        {
            Position = position;
            TexCoord = new Vector2();
            VertexColor = Color32.Transparent;
        }

        public Vertex(Vertex copy)
        {
            Position = copy.Position;
            TexCoord = copy.TexCoord;
            VertexColor = copy.VertexColor;
        }

        public Vertex(Color32 color)
        {
            Position = new Vector3();
            TexCoord = new Vector2();
            VertexColor = color;
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
            return Position == other.Position && VertexColor == other.VertexColor && TexCoord == other.TexCoord;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Vertex vertex && Equals(vertex);
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

        public override string ToString()
        {
            return $"Pos: {Position} Col: {VertexColor} TexCoord: {TexCoord}";
        }

        public void SetupVertexAttributePointers()
        {
            var size = Marshal.SizeOf<Vertex>();
            GLCheck.Check(EnableVertexAttribArray, 0);
            GLCheck.Check(EnableVertexAttribArray, 1);
            GLCheck.Check(EnableVertexAttribArray, 2);
            GLCheck.Check(VertexAttribPointer, 0, 3, VertexAttribPointerType.Float, false, size, 0);
            GLCheck.Check(VertexAttribPointer, 1, 2, VertexAttribPointerType.Float, false, size, Marshal.SizeOf<Vector3>());
            GLCheck.Check(VertexAttribPointer, 2, 4, VertexAttribPointerType.Float, false, size, Marshal.SizeOf<Vector3>() + Marshal.SizeOf<Vector2>());
        }
    }
}