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

using System.Linq;
using JetBrains.Annotations;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Drawing.Text
{
    public class DrawableText : GeometryBase, IDrawable
    {
        [NotNull] private readonly string m_text;
        private readonly GraphicsFont m_font;

        public DrawableText([NotNull]string text)
            : this(GraphicsFont.DefaultFont, text)
        {
        }

        public DrawableText([NotNull]GraphicsFont font, [NotNull]string text)
            : base(Color32.Transparent)
        {
            m_font = font;
            m_text = text;
            Texture = new ImageTexture(m_font.CachePage);
        }

        /// <inheritdoc />
        protected override PrimitiveType PrimitiveType => PrimitiveType.TriangleStrip;
        
        /// <inheritdoc />
        protected override Vertex[] GetVertices()
        {
            var result = new Vertex[m_text.Length * 4];
            
            var i = 0;
            foreach (var glyph in m_text.Select(x => m_font.GetGlyph(x)))
            {
                result[i++] = new Vertex();
                result[i++] = new Vertex();
                result[i++] = new Vertex();
                result[i++] = new Vertex();
            }
            return new Vertex[] { };
        }

        /// <inheritdoc />
        protected override uint[] GetIndices()
        {
            return new uint[] { };
        }

        public Vector2 FindCharacterPos(uint index)
        {
            
            return new Vector2(0);
        }
    }
}