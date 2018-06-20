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
using System.ComponentModel;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Layout
{
    public class Canvas : Panel
    {
        private readonly Dictionary<IUIElement, Vector2f> positions = new Dictionary<IUIElement, Vector2f>();

        protected override void LayoutChildren(FloatRect target)
        {
            var totalSize = target.Size();
            foreach (var child in Children)
            {
                if (!positions.TryGetValue(child, out var position))
                    position = new Vector2f();
                var relativePosition = position;
                var subTarget = new FloatRect(relativePosition, totalSize - relativePosition);
                child.Layout(subTarget);
            }
        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            base.DrawInternal(target, states);
        }

        public void SetGlobalPosition(IUIElement element, Vector2f position)
        {
            SetRelativePosition(element, position - RenderPosition);
        }

        public void SetRelativePosition(IUIElement element, Vector2f position)
        {
            if (Children.All(x => x != element))
                throw new InvalidOperationException("The specified finalRect element is not contained in this canvas");
            positions.AddOrUpdate(element, position);
            OnChildLayoutPropertyChanged(element, new PropertyChangedEventArgs("Canvas.Position"));
        }

        public Vector2f GetGlobalPosition(IUIElement element)
        {
            return GetRelativePosition(element) - RenderPosition;
        }

        public Vector2f GetRelativePosition(IUIElement element)
        {
            return !positions.TryGetValue(element, out var value) ? new Vector2f() : value;
        }
    }
}