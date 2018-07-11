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
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;

namespace iLynx.Graphics.Drawing
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Texture FromFile([NotNull] string filePath)
        {
            using (var stream = File.OpenRead(filePath))
                return FromImage(Image.Load<Color32>(stream));
        }

        /// <summary>
        /// Creates a new <see cref="Texture"/> with the data of the specified <see cref="Image{Rgba64}"/>
        /// </summary>
        /// <param name="image">The image to copy data from</param>
        /// <returns>A new <see cref="Texture"/> instance with the data of the specified image loaded in to GPU memory.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Texture FromImage(Image<Color32> image)
        {
            return new ImageTexture(image);
        }

        /// <summary>
        /// Creates a new texture of the specified size and fills it with the specified <see cref="Color32"/>
        /// </summary>
        /// <param name="width">The width of the texture to create</param>
        /// <param name="height">The height of the texture to create</param>
        /// <param name="fillColor">The color that is set on each pixel in the texture</param>
        /// <returns>A new <see cref="Texture"/> instance with the width and height specified, filled with the specified color</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Texture Create(uint width, uint height, Color32 fillColor)
        {
            var result = new Color32[width * height];
            Array.Fill(result, fillColor);
            return new ImageTexture(Image.LoadPixelData(result, (int)width, (int)height));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Texture"/> and binds the texture to the <see cref="TextureTarget.Texture2D"/> slot.
        /// Note that this constructor does NOT unbind the generated image, as inheritors are expected to update the actual data of the texture and then, eventually unbind it.
        /// </summary>
        /// <param name="generateMipMap"></param>
        /// <param name="horizontalWrapMode"></param>
        /// <param name="verticalWrapMode"></param>
        protected Texture(
            bool generateMipMap = false,
            TextureWrapMode horizontalWrapMode = TextureWrapMode.Repeat,
            TextureWrapMode verticalWrapMode = TextureWrapMode.Repeat
        )
        {
            var hmode = (uint)horizontalWrapMode;
            var vmode = (uint)verticalWrapMode;
            var magMode = (uint)TextureMagFilter.Linear;
            var minMode = (uint)(generateMipMap ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear);
            m_handle = GLCheck.Check(GL.GenTexture);
            Bind();
            GLCheck.Check(GL.TexParameterI, TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ref hmode);
            GLCheck.Check(GL.TexParameterI, TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ref vmode);
            GLCheck.Check(GL.TexParameter, TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new[] { 0f, 0f, 0f, 0f });
            GLCheck.Check(GL.TexParameterI, TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref magMode);
            GLCheck.Check(GL.TexParameterI, TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref minMode);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Texture(
            Color32[] data,
            uint width,
            uint height,
            bool generateMipMap = false,
            TextureWrapMode horizontalWrapMode = TextureWrapMode.Repeat,
            TextureWrapMode verticalWrapMode = TextureWrapMode.Repeat
        )
        : this(generateMipMap, horizontalWrapMode, verticalWrapMode)
        {
            try
            {
                UploadData(data, (int)width, (int)height);
                if (generateMipMap)
                    GLCheck.Check(GL.GenerateMipmap, GenerateMipmapTarget.Texture2D);
            }
            finally
            {
                Unbind();
            }
        }

        /// <summary>
        /// Uploads the specified image data to the gpu
        /// </summary>
        /// <param name="data">The image data</param>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        protected static void UploadData(Color32[] data, int width, int height)
        {
            GLCheck.Check(GL.TexImage2D, TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);
        }

        /// <summary>
        /// Uploads the specified image data to the gpu
        /// </summary>
        /// <param name="data">A pointer to the image data</param>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        protected static unsafe void UploadData(Color32* data, int width, int height)
        {
            UploadData((IntPtr)data, width, height);
        }

        /// <summary>
        /// Uploads the specified image data to the gpu
        /// </summary>
        /// <param name="data">A pointer to the image data</param>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        protected static void UploadData(IntPtr data, int width, int height)
        {
            GLCheck.Check(GL.TexImage2D, TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);
        }

        /// <summary>
        /// Binds the texture
        /// </summary>
        protected void Bind()
        {
            GLCheck.Check(GL.BindTexture, TextureTarget.Texture2D, m_handle);
        }

        /// <summary>
        /// Unbinds the texture
        /// </summary>
        protected void Unbind()
        {
            GLCheck.Check(GL.BindTexture, TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Binds the specified texture to the <see cref="TextureTarget.Texture2D"/> slot
        /// </summary>
        /// <param name="texture">The texture to bind.</param>
        /// <remarks>
        /// Calling this method with null is equivalent to unbinding the currently active texture
        /// <see cref="GL.BindTexture(TextureTarget,int)"/>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Bind([CanBeNull] Texture texture)
        {
            GL.BindTexture(TextureTarget.Texture2D, texture?.m_handle ?? 0);
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

    /// <summary>
    /// Represents a texture with an <see cref="Image{Color32}"/> as the data source
    /// </summary>
    /// <inheritdoc cref="Texture"/>
    public class ImageTexture : Texture
    {
        private Image<Color32> m_image;

        /// <inheritdoc />
        public ImageTexture([NotNull]Image<Color32> image, bool generateMipMap = false, TextureWrapMode horizontalWrapMode = TextureWrapMode.Repeat, TextureWrapMode verticalWrapMode = TextureWrapMode.Repeat)
            : base(generateMipMap, horizontalWrapMode, verticalWrapMode)
        {
            Image = image;
            Update();
        }

        /// <summary>
        /// Gets a reference to the image that is used to define this texture
        /// </summary>
        public Image<Color32> Image
        {
            get => m_image;
            set
            {
                if (value == m_image) return;
                m_image = value;
                Update();
            }
        }

        /// <summary>
        /// Updates the texture from the contained image
        /// </summary>
        public void Update()
        {
            if (null == m_image) return;
            Bind();
            try
            {
                if (0 == Image.Height || 0 == Image.Width) return;
                unsafe
                {
                    fixed (Color32* data = &Image.DangerousGetPinnableReferenceToPixelBuffer())
                        UploadData(data, Image.Width, Image.Height);
                }
            }
            finally
            {
                Unbind();
            }
        }
    }
}
