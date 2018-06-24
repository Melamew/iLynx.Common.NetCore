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
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Controls
{
    public class TextElement : Control
    {
        private readonly Text renderable = new Text();
        private string text;
        private Color foreground;
        private uint fontSize = 24;
        private Font font = DefaultFont;

        public TextElement()
            : this(string.Empty, Color.Black) { }

        public TextElement(Color color)
            : this(string.Empty, color) { }

        public TextElement(string text, Color foreground)
        {
            this.text = text;
            this.foreground = foreground;
            Margin = 2f;
        }

        public TextElement(string text, Font font, uint fontSize)
            : this(text, Color.Black)
        {
            this.font = font;
            this.fontSize = fontSize;
        }

        protected Vector2f FindCharacterPosition(uint index)
        {
            return renderable?.FindCharacterPos(index) ?? new Vector2f();
        }

        public string Text
        {
            get => text;
            set
            {
                if (value == text) return;
                var old = text;
                text = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Color Foreground
        {
            get => foreground;
            set
            {
                if (value == foreground) return;
                var old = foreground;
                foreground = value;
                OnPropertyChanged(old, value);
            }
        }

        public uint FontSize
        {
            get => fontSize;
            set
            {
                if (value == fontSize) return;
                var old = fontSize;
                fontSize = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Font Font
        {
            get => font;
            set
            {
                if (value == font) return;
                var old = font;
                font = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            var textStates = states;
            textStates.Transform.Translate(-renderable.GetLocalBounds().Position());
            renderable.FillColor = foreground;
            target.Draw(renderable, textStates);
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {renderable.DisplayedString}";
        }

        public override Vector2f Measure(Vector2f availableSpace)
        {
            var localBounds = renderable.GetLocalBounds();
            return new Vector2f(localBounds.Width,
                localBounds.Height);
        }

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            renderable.CharacterSize = fontSize;
            renderable.DisplayedString = text;
            renderable.Font = font;
            var localBounds = renderable.GetLocalBounds();
            return new FloatRect(
                finalRect.Left,
                finalRect.Top,
                localBounds.Width,
                localBounds.Height);
        }
    }
}