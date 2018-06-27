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

using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.OpenGL.Layout
{
    public class StackPanel : Panel
    {
        private bool reverse;
        private Orientation orientation = Orientation.Vertical;

        public bool Reverse
        {
            get => reverse;
            set
            {
                if (value == reverse) return;
                var old = reverse;
                reverse = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Orientation Orientation
        {
            get => orientation;
            set
            {
                if (value == orientation) return;
                var old = orientation;
                orientation = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected override void LayoutChildren(FloatRect target)
        {
            var availableSpace = target;
            var scalar = orientation == Orientation.Horizontal ? new Vector2f(0f, 1f) : new Vector2f(1f, 0f);
            var childSpaceScalar = orientation == Orientation.Horizontal ? new Vector2f(1f, 0f) : new Vector2f(0f, 1f); // The inverse for stepping size
            var usedSpace = new FloatRect(availableSpace.Position(), availableSpace.Size().Scale(scalar));
            foreach (var child in reverse ? Children.Reverse() : Children)
            {
                child.Layout(availableSpace);
                var childSpace = (child.BoundingBox + child.Margin).Size().Scale(childSpaceScalar);
                usedSpace.Height += childSpace.Y;
                usedSpace.Width += childSpace.X;
                availableSpace.Left += childSpace.X;
                availableSpace.Width -= childSpace.X;
                availableSpace.Top += childSpace.Y;
                availableSpace.Height -= childSpace.Y;
            }
        }
    }
}
