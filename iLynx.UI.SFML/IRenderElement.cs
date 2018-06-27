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
using iLynx.Common;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml
{
    public interface IRenderElement : Drawable, IBindingSource
    {
        /// <summary>
        /// Gets the bounding box of this element
        /// </summary>
        FloatRect BoundingBox { get; }

        /// <summary>
        /// Gets the parent element (if any) of this element.
        /// If this element does not have a parent (IE. It is the root of a tree) this property will return null
        /// </summary>
        IRenderElement Parent { get; }

        /// <summary>
        /// Called when this element is added to or removed from a content control, panel or other elements with children
        /// WARNING: Do not call this method unless the parent of this element has ACTUALLY changed.
        /// </summary>
        /// <param name="parent"></param>
        void SetLogicalParent(IRenderElement parent);

        /// <summary>
        /// Transforms the specified coordinates from global to local coordinates (coordinates relative to this element)
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        Vector2f ToLocalCoords(Vector2f coords);

        /// <summary>
        /// Transforms the specified coordinates from local to global coordinates (coordinates relative to this element)
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        Vector2f ToGlobalCoords(Vector2f coords);

        /// <summary>
        /// Gets the size of this element in "render coordinates"
        /// </summary>
        Vector2f RenderSize { get; }

        /// <summary>
        /// Raised when the bounding box of this element has changed
        /// </summary>
        event ValueChangedEventHandler<IRenderElement, FloatRect> BoundingBoxChanged;

        /// <summary>
        /// Raised when the render size of this element has changed
        /// </summary>
        event ValueChangedEventHandler<IRenderElement, Vector2f> RenderSizeChanged;

        /// <summary>
        /// Raised when the render position of this element has changed
        /// </summary>
        event ValueChangedEventHandler<IRenderElement, Vector2f> RenderPositionChanged;
    }
}