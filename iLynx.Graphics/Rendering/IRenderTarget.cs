﻿#region LICENSE
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
using iLynx.Graphics.Rendering.Shaders;
using OpenTK;

namespace iLynx.Graphics.Rendering
{
    public interface IRenderTarget
    {
        /// <summary>
        /// Gets or Sets the current view transform.
        /// </summary>
        Matrix4 ViewTransform { get; set; }

        /// <summary>
        /// Gets the currently active shader
        /// </summary>
        ShaderProgram ActiveShader { get; }

        /// <summary>
        /// Gets the currently active texture
        /// </summary>
        Texture ActiveTexture { get; }

        /// <summary>
        /// Binds the specified shader to this target.
        /// </summary>
        /// <param name="shader">The shader to bind</param>
        void UseShader(ShaderProgram shader);

        /// <summary>
        /// Binds the specified texture to this target.
        /// </summary>
        /// <param name="texture"></param>
        void BindTexture(Texture texture);

        /// <summary>
        /// Draws the specified <see cref="IDrawable"/> in this context.
        /// NOTE: This method will essentially call <see cref="IDrawable.Draw(IRenderTarget)"/> with this <see cref="IRenderTarget"/> as its argument
        /// </summary>
        /// <param name="drawable"></param>
        void Draw(IDrawable drawable);
    }
}
