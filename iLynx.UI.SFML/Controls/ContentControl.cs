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
using System.ComponentModel;
using iLynx.UI.Sfml.Input;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Controls
{
    public class ContentControl : Control
    {
        private IUIElement content = new TextElement { HorizontalAlignment = Alignment.Center, VerticalAlignment = Alignment.Center, Color = Color.Black };
        private Color foreground;
        private Thickness padding = 4f;

        public ContentControl()
        {
            content.SetLogicalParent(this);
            content.LayoutPropertyChanged += Content_LayoutPropertyChanged;
        }

        public Thickness Padding
        {
            get => padding;
            set
            {
                if (value == padding) return;
                var old = padding;
                padding = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Color Foreground
        {
            get => (content as TextElement)?.Color ?? foreground;
            set
            {
                var old = foreground;
                if (value == old) return;
                foreground = value;
                if (content is TextElement label)
                    label.Color = foreground;
                OnPropertyChanged(old, value);
            }
        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            base.DrawInternal(target, states);
            content?.Draw(target, states);
        }

        public override Vector2f Measure(Vector2f availableSpace)
        {
            var dims = base.Measure(availableSpace);
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (0 == dims.X)
                dims.X = availableSpace.X;
            if (0 == dims.Y)
                dims.Y = availableSpace.Y;
            // ReSharper restore CompareOfFloatsByEqualityOperator
            return content?.Measure(dims - padding) + padding + content?.Margin ?? dims;
        }

        public override bool HitTest(Vector2f position, out IInputElement element)
        {
            var hit = base.HitTest(position, out element);
            if (!hit) return false;
            if (content is IInputElement input && input.HitTest(position, out var child))
                element = child;
            return true;
        }

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            finalRect = base.LayoutInternal(finalRect);
            var contentRect = new FloatRect(0f, 0f, finalRect.Width, finalRect.Height) - padding;
            content?.Layout(contentRect);
            return finalRect;
        }

        public string ContentString
        {
            get => (content as TextElement)?.Text;
            set
            {
                if (!(content is TextElement l))
                {
                    l = new TextElement(foreground) { Text = value };
                    Content = l;
                }
                else
                    l.Text = value;
            }
        }

        public IUIElement Content
        {
            get => content;
            set
            {
                if (value == content) return;
                var old = content;
                if (null != old)
                    old.LayoutPropertyChanged -= Content_LayoutPropertyChanged;
                content = value;
                if (null != content)
                {
                    content.SetLogicalParent(this);
                    content.LayoutPropertyChanged += Content_LayoutPropertyChanged;
                }
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        private void Content_LayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnLayoutPropertyChanged($"{nameof(Content)}.{e.PropertyName}");
        }
    }
}
