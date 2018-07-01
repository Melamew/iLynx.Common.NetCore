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

namespace iLynx.Graphics.Rendering
{
    public interface IRenderContext
    {
        /// <summary>
        /// Binds the specified <see cref="VertexBuffer{TVertex}"/> to 
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="buffer"></param>
        void Bind<TVertex>(VertexBuffer<TVertex> buffer) where TVertex : struct, IEquatable<TVertex>;

        /// <summary>
        /// Sets the specified <see cref="ShaderProgram"/> as the current shader
        /// </summary>
        /// <param name="program"></param>
        void UseShader(ShaderProgram program);

        /// <summary>
        /// Binds the specified <see cref="Texture"/> for use in the current <see cref="IRenderContext"/>
        /// </summary>
        /// <param name="texture"></param>
        void BindTexture(Texture texture);

        /// <summary>
        /// Draws the currently bound <see cref="VertexBuffer{TVertex}"/>
        /// </summary>
        void Draw();

        /// <summary>
        /// Binds the specified <see cref="VertexBuffer{TVertex}"/> and draws it with the currently set texture and shader
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="vertexBuffer"></param>
        void Draw<TVertex>(VertexBuffer<TVertex> vertexBuffer) where TVertex : struct, IEquatable<TVertex>;
    }
}