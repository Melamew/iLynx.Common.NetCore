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
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using OpenTK;
using SixLabors.Fonts;

namespace iLynx.Graphics.Drawing.Text
{
    public class DrawableText : IDrawable
    {
        private static readonly FontCollection s_Collection = new FontCollection();
        private Font m_font;
        private float m_fontSize;
        private string m_text;

        /// <summary>
        /// Gets a font with the specified size from the specified font family.
        /// </summary>
        /// <param name="fontFamily">The font family to load</param>
        /// <param name="size">The character size</param>
        /// <param name="style">The font style</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"><paramref name="fontFamily"/> is not available</exception>
        public static Font GetFont([NotNull]string fontFamily, float size, FontStyle style = FontStyle.Regular)
        {
            if (size < 0f) throw new ArgumentOutOfRangeException(nameof(size));
            if (!s_Collection.TryFind(fontFamily, out var family)) throw new KeyNotFoundException($"Could not find a font family with the specified name ({fontFamily})");
            return s_Collection.CreateFont(family.Name, size, style);
        }

        public DrawableText([NotNull]string text)
        {
            m_text = text;
        }

        public DrawableText([NotNull]Font font, [NotNull]string text)
        {

        }

        private Font Load(Stream source)
        {
            var description = FontDescription.LoadDescription(source);
            if (s_Collection.Families.Any(x => x.Name == description.FontFamily)) return s_Collection.CreateFont(description.FontFamily, m_fontSize);
            source.Seek(0, SeekOrigin.Begin);
            s_Collection.Install(source);
            return s_Collection.CreateFont(description.FontFamily, m_fontSize);
        }

        public Color FillColor { get; set; }

        public void Draw(IRenderStates states)
        {
            
        }

        public Vector2 FindCharacterPos(uint index)
        {
            return new Vector2(0);
        }

        public Shader Shader { get; set; } = Shader.DefaultShader;
        public Texture Texture { get; set; }
    }
}