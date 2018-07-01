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
using System.ComponentModel;
using OpenTK;

namespace iLynx.UI.OpenGL
{
    // ReSharper disable once InconsistentNaming
    public interface IUIElement : IInputElement
    {
        /// <summary>
        /// Lays out this element within the specified <see cref="RectangleF"/>
        /// </summary>
        /// <param name="target"></param>
        RectangleF Layout(RectangleF target);

        /// <summary>
        /// Given the available space (<paramref name="available"/>), returns the space taken up by this element.
        /// </summary>
        /// <param name="available"></param>
        /// <returns></returns>
        SizeF Measure(SizeF available);

        /// <summary>
        /// Gets or Sets the margin of this element
        /// </summary>
        Thickness Margin { get; set; }

        /// <summary>
        /// Fired whenever a property that would affect the layout of this element is changed.
        /// (ie: <see cref="Margin"/>)
        /// </summary>
        event EventHandler<PropertyChangedEventArgs> LayoutPropertyChanged;
    }
}
