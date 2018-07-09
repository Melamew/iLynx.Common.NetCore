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
using iLynx.Graphics.Shaders;
using OpenTK;

namespace iLynx.Graphics
{
    /// <summary>
    /// Defines a context that is used to render OpenGL content and synchronize off-loaded operations (Such as texture loading, etc.)
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// Gets a value indicating whether or not this context is active on the current thread.
        /// </summary>
        bool IsCurrent { get; }
        /// <summary>
        /// Attempts to activate this context on the current thread and returns a value indicating whether or not it is.
        /// </summary>
        /// <returns></returns>
        bool MakeCurrent();
        /// <summary>
        /// Gets a value indicating whether or not this context has been initialized (Meaning all the initial OpenGL set-up has been completed).
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Initializes this context
        /// </summary>
        void Initialize();
        /// <summary>
        /// Gets or Sets the currently active shader
        /// </summary>
        Shader Shader { get; set; }
        /// <summary>
        /// Gets or Sets the currently active texture
        /// </summary>
        Texture Texture { get; set; }
        /// <summary>
        /// Gets or Sets the view transform that is applied to subsequent transformations when rendering
        /// </summary>
        Matrix4 ViewTransform { get; set; }
        /// <summary>
        /// Applies the specified transform on top of the <see cref="ViewTransform"/> and uploads the final value to the shader
        /// </summary>
        /// <param name="transform"></param>
        void ApplyTransform(Matrix4 transform);
        /// <summary>
        /// Enqueues the specified method (with parameters) in the synchronization queue for this context
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        void QueueForSync(Delegate method, params object[] parameters);
        /// <summary>
        /// Processes the synchronization queue of this context, up to a maximum calls / operations of <paramref name="maxCalls"/>
        /// <remarks>If <see cref="maxCalls"/> is 0, this method will attempt to process all pending sync calls</remarks>
        /// </summary>
        /// <param name="maxCalls">The maximum number of calls to process (0 = unlimited)</param>
        void ProcessSyncQueue(uint maxCalls = 0);
    }
}