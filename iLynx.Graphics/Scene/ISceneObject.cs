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
using iLynx.Graphics.Rendering;

namespace iLynx.Graphics.Scene
{
    /// <summary>
    /// The base interface for an object to be rendered in a scene
    /// </summary>
    public interface ISceneObject : IDisposable
    {
        /// <summary>
        /// Updates this scene object (Perform transform computations etc. here)
        /// </summary>
        void Update();

        /// <summary>
        /// Render this object to the specified <see cref="IRenderContext"/>
        /// </summary>
        /// <param name="renderContext"></param>
        void Display(IRenderContext renderContext);

        /// <summary>
        /// Gets a read-only collection of this <see cref="ISceneObject"/>'s children
        /// </summary>
        IReadOnlyCollection<ISceneObject> Children { get; }

        /// <summary>
        /// Gets the parent of this <see cref="ISceneObject"/>
        /// </summary>
        ISceneObject Parent { get; }
    }

    //public interface ITransform
    //{
    //    /// <summary>
    //    /// Applies this transform to the specified <see cref="ISceneObject"/>
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    void Apply(ISceneObject obj);
    //}
}
