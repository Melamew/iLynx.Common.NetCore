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
using JetBrains.Annotations;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace iLynx.Graphics
{
    /// <inheritdoc/>
    /// <summary>
    /// Represents and OpenGL texture
    /// </summary>
    public class Texture : IDisposable
    {
        private readonly int m_handle;

        /// <summary>
        /// The default texture used for shading - This is a 1x1 square white texture
        /// </summary>
        public static Texture DefaultTexture { get; } = Create(1, 1, Color32.White);

        /// <summary>
        /// Loads an image from the specified file
        /// </summary>
        /// <param name="filePath">The path to the file to load</param>
        /// <returns>A new <see cref="Texture"/> instance with the data of the image loaded in to GPU memory.</returns>
        public static Texture FromFile([NotNull] string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                var result = new byte[stream.Length];
                stream.Read(result, 0, result.Length);
                return FromImage(Image.Load<Rgba64>(result));
            }
        }

        /// <summary>
        /// Creates a new <see cref="Texture"/> with the data of the specified <see cref="Image{Rgba64}"/>
        /// </summary>
        /// <param name="image">The image to copy data from</param>
        /// <returns>A new <see cref="Texture"/> instance with the data of the specified image loaded in to GPU memory.</returns>
        public static Texture FromImage(Image<Rgba64> image)
        {
            var dst = new Rgba64[image.Width * image.Height];
            image.SavePixelData(dst);
            var converted = dst.Select(x => (Color32) x).ToArray();
            return new Texture(
                converted,
                (uint) image.Width,
                (uint) image.Height
            );
        }

        public static Texture FromImage(Image<Color32> image)
        {
            var dst = new Color32[image.Width * image.Height];
            image.SavePixelData(dst);
            return new Texture(dst, (uint) image.Width, (uint) image.Height);
        }

        /// <summary>
        /// Creates a new texture of the specified size and fills it with the specified <see cref="Color32"/>
        /// </summary>
        /// <param name="width">The width of the texture to create</param>
        /// <param name="height">The height of the texture to create</param>
        /// <param name="fillColor">The color that is set on each pixel in the texture</param>
        /// <returns>A new <see cref="Texture"/> instance with the width and height specified, filled with the specified color</returns>
        public static Texture Create(uint width, uint height, Color32 fillColor)
        {
            var result = new Color32[width * height];
            Array.Fill(result, fillColor);
            return new Texture(result, width, height);
        }

        /// <summary>
        /// Creates a new instance with the specified data.
        /// </summary>
        /// <param name="data">The raw color (pixel) data of the texture</param>
        /// <param name="width">The width of the texture (in pixels)</param>
        /// <param name="height">The height of the texture (in pixels)</param>
        /// <param name="generateMipMap"></param>
        /// <param name="horizontalWrapMode"></param>
        /// <param name="verticalWrapMode"></param>
        public Texture(
            Color32[] data,
            uint width,
            uint height,
            bool generateMipMap = false,
            TextureWrapMode horizontalWrapMode = TextureWrapMode.Repeat,
            TextureWrapMode verticalWrapMode = TextureWrapMode.Repeat
        )
        {
            var hmode = (uint) horizontalWrapMode;
            var vmode = (uint) verticalWrapMode;
            var magMode = (uint) TextureMagFilter.Linear;
            var minMode = (uint) (generateMipMap ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear);
            m_handle = GLCheck.Check(GL.GenTexture);
            GLCheck.Check(() => GL.BindTexture(TextureTarget.Texture2D, m_handle));
            GLCheck.Check(() => GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ref hmode));
            GLCheck.Check(() => GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ref vmode));
            GLCheck.Check(() => GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new[] {0f, 0f, 0f, 0f}));
            GLCheck.Check(() => GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref magMode));
            GLCheck.Check(() => GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref minMode));
            GLCheck.Check(() => GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, (int) width, (int) height, 0, PixelFormat.Rgba, PixelType.Float, data));
            if (generateMipMap)
                GLCheck.Check(GL.GenerateMipmap, GenerateMipmapTarget.Texture2D);
            GLCheck.Check(() => GL.BindTexture(TextureTarget.Texture2D, 0));
        }

        /// <summary>
        /// Binds the specified texture to the <see cref="TextureTarget.Texture2D"/> slot
        /// </summary>
        /// <param name="texture">The texture to bind.</param>
        /// <remarks>
        /// Calling this method with null is equivalent to unbinding the currently active texture
        /// <see cref="GL.BindTexture(TextureTarget,int)"/>
        /// </remarks>
        public static void Bind([CanBeNull] Texture texture)
        {
            GLCheck.Check(GL.BindTexture, TextureTarget.Texture2D, texture?.m_handle ?? 0);
        }

        /// <inheritdoc />
        /// <remarks>
        /// <see cref="GL.DeleteTexture(int)"/>
        /// </remarks>
        public void Dispose()
        {
            GLCheck.Check(GL.DeleteTexture, m_handle);
        }
    }
}
