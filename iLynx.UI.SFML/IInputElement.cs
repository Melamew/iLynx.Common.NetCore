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
using SFML.System;

namespace iLynx.UI.Sfml
{
    public interface IInputElement
    {
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
        /// Perform a "hit test" to check if the specified <see cref="Vector2f"/> (Position) is within the bounds of this element.
        /// </summary>
        /// <param name="position">The position (in global coordinates) to check against this element</param>
        /// <param name="element">The element that this position hit</param>
        /// <returns>True if the position is within this element, otherwise false</returns>
        bool HitTest(Vector2f position, out IInputElement element);

        /// <summary>
        /// Gets a value indicating whether or not this element is focusable
        /// </summary>
        bool Focusable { get; }

        /// <summary>
        /// Called whenever the mouse cursor is moved inside the bounds of this element
        /// </summary>
        /// <param name="args"></param>
        void OnMouseOver(MouseArgs args);

        /// <summary>
        /// Called whenever a mouse button is pressed while the mouse cursor is within this element
        /// </summary>
        /// <param name="args"></param>
        void OnMouseButtonDown(MouseButtonArgs args);

        /// <summary>
        /// Called whenever this element receives focus
        /// </summary>
        void OnReceivedFocus();

        /// <summary>
        /// Called whenever this element loses focus
        /// </summary>
        void OnLostFocus();

        /// <summary>
        /// Called whenever a mouse button that was pressed inside this element is released
        /// </summary>
        /// <param name="args"></param>
        void OnMouseButtonUp(MouseButtonArgs args);

        /// <summary>
        /// Called whenever a key is pressed while this element is in focus
        /// </summary>
        /// <param name="args"></param>
        void OnKeyDown(KeyArgs args);

        /// <summary>
        /// Called whenever a key is released while this element is in focus
        /// </summary>
        /// <param name="args"></param>
        void OnKeyUp(KeyArgs args);
    }
}