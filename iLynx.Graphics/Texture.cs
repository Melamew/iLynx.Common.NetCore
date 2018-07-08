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
using System.IO;
using System.Linq;
using ImageMagick;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics
{
    public class Texture
    {
        private readonly int m_handle;

        public static Texture FromFile(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
                return FromStream(stream);
        }

        public static Texture FromStream(Stream stream)
        {
            return FromImage(new MagickImage(stream));
        }

        public static Texture FromImage(IMagickImage image)
        {
            return new Texture(
                image.GetPixelsUnsafe().Select(x => (Color32) x.ToColor()).ToArray(),
                image.Width,
                image.Height
            );
        }

        public Texture(
            Color32[] data,
            int width,
            int height,
            TextureWrapMode horizontalWrapMode = TextureWrapMode.Repeat,
            TextureWrapMode verticalWrapMode = TextureWrapMode.Repeat
        )
        {
            var hmode = (uint) horizontalWrapMode;
            var vmode = (uint) verticalWrapMode;
            var magMode = (uint) TextureMagFilter.Linear;
            var minMode = (uint) TextureMinFilter.LinearMipmapLinear;
            m_handle = GLCheck.Check(GL.GenTexture);
            GLCheck.Check(() => GL.BindTexture(TextureTarget.Texture2D, m_handle));
            GLCheck.Check(() => GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ref hmode));
            GLCheck.Check(() => GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ref vmode));
            GLCheck.Check(() => GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new[] {0f, 0f, 0f, 0f}));
            GLCheck.Check(() => GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref magMode));
            GLCheck.Check(() => GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref minMode));
            GLCheck.Check(() => GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, (int) width, (int) height, 0, PixelFormat.Rgba, PixelType.Float, data));
            GLCheck.Check(() => GL.BindTexture(TextureTarget.Texture2D, 0));
        }

        public void Bind()
        {
            GLCheck.Check(() => GL.BindTexture(TextureTarget.Texture2D, m_handle));
        }
    }
}
