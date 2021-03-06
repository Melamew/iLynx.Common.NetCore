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

using iLynx.Graphics.Drawing;
using iLynx.Graphics.Maths;
using OpenTK;

namespace iLynx.Graphics
{
    /// <summary>
    /// Base interface for drawable elements.
    /// NOTE: A drawable must NOT call <see cref="IView.AddDrawable"/>
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws this drawable in the currently active context
        /// </summary>
        void Draw(IRenderStates states);
        /// <summary>
        /// Gets or Sets the shader to use for rendering this drawable
        /// </summary>
        Shader Shader { get; }
        /// <summary>
        /// Gets or Sets the  texture to apply to this drawable
        /// </summary>
        Texture Texture { get; set; }

        /// <summary>
        /// Returns a value indicating whether or not the specified line intersects with any part of this drawable
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        bool Intersects(LineSegment3D line);
    }
}
