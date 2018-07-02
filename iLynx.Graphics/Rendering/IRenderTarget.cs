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

namespace iLynx.Graphics.Rendering
{
    public interface IRenderTarget
    {
        ///// <summary>
        ///// Binds the specified <see cref="VertexBufferObject{TVertex}"/> and draws it with the currently set texture and shader
        ///// </summary>
        ///// <typeparam name="TVertex"></typeparam>
        ///// <param name="vertexBuffer"></param>
        //void DrawArrays<TVertex>(VertexBufferObject<TVertex> vertexBuffer, int offset, int count) where TVertex : struct, IEquatable<TVertex>;

        ///// <summary>
        ///// Draws the specified <see cref="VertexArrayObject{TVertex}"/> to this context
        ///// </summary>
        ///// <typeparam name="TVertex"></typeparam>
        ///// <param name="vao"></param>
        ///// <param name="offset"></param>
        ///// <param name="count"></param>
        //void DrawArrays<TVertex>(VertexArrayObject<TVertex> vao, int offset, int count) where TVertex : struct, IEquatable<TVertex>, IVAOElement;

        /// <summary>
        /// Draws the specified <see cref="IDrawable"/> in this context.
        /// NOTE: This method will essentially call <see cref="IDrawable.Draw(IRenderTarget)"/> with this <see cref="IRenderTarget"/> as its argument
        /// </summary>
        /// <param name="drawable"></param>
        void Draw(IDrawable drawable);
    }
}
