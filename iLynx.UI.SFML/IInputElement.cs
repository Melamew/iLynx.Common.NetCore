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
using SFML.System;

namespace iLynx.UI.Sfml
{
    public interface IInputElement : IRenderElement
    {
        /// <summary>
        /// Gets a value indicating whether or not this element is "visible" when hit testing.
        /// </summary>
        bool IsHitTestVisible { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this element is focusable
        /// </summary>
        bool IsFocusable { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this element has keyboard focus
        /// </summary>
        bool HasFocus { get; }

        /// <summary>
        /// Gets a value indicating whether or not the mouse cursor is over this element
        /// </summary>
        bool IsMouseOver { get; }

        /// <summary>
        /// Perform a "hit test" to check if the specified <see cref="Vector2f"/> (Position) is within the bounds of this element.
        /// </summary>
        /// <param name="position">The position (in global coordinates) to check against this element</param>
        /// <param name="element">The element that this position hit</param>
        /// <returns>True if the position is within this element, otherwise false</returns>
        bool HitTest(Vector2f position, out IInputElement element);
    }
}