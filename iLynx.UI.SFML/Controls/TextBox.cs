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
using iLynx.UI.Sfml.Input;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.Sfml.Controls
{
    public class TextBox : TextElement
    {
        private int caretIndex = 0;
        private FloatRect charRect = new FloatRect();
        protected override void OnMouseButtonDown(MouseDownEvent args)
        {
            base.OnMouseButtonDown(args);
            var relPos = ToLocalCoords(args.Position);
            switch (args.Button)
            {
                case Mouse.Button.Left:
                    LeftMouseDown(relPos);
                    break;
                case Mouse.Button.Right:
                    RightMouseDown(relPos);
                    break;
                default:
                    return;
            }

            SetCaretIndex(relPos);
        }

        protected override void OnTextChanged(string oldValue, string newValue)
        {
            base.OnTextChanged(oldValue, newValue);
            caretIndex = newValue.Length;
        }

        private void SetCaretIndex(Vector2f position)
        {
            var size = (float)FontSize;
            var offset = new Vector2f(size / 4f, 0f);
            for (var i = Text.Length - 1; i >= 0; --i)
            {
                var glyphSize = new Vector2f(size, size);
                var charPos = FindCharacterPosition((uint)i);
                charRect = new FloatRect(charPos - offset, glyphSize);
                if (charRect.Contains(position.X, position.Y))
                {
                    caretIndex = i;
                    Console.WriteLine($"Index should be {i} ({Text[i]}), {charRect}");
                    break;
                }
            }
        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            base.DrawInternal(target, states);
            var caret = new RectangleShape(charRect.Size())
            {
                Position = charRect.Position() + RenderPosition,
                OutlineThickness = 2f,
                OutlineColor = Color.Red,
                FillColor = Color.Transparent
            };
            target.Draw(caret);
        }

        private void RightMouseDown(Vector2f position)
        {

        }

        private void LeftMouseDown(Vector2f position)
        {

        }

        protected override void OnKeyDown(KeyboardEvent args)
        {
            base.OnKeyDown(args);
            switch (args.Key)
            {
                case Keyboard.Key.BackSpace:
                    Console.WriteLine("Backspace");
                    break;
                case Keyboard.Key.Return:
                    Console.WriteLine("Return");
                    Text += '\n';
                    break;
            }
        }

        protected override void OnTextEntered(TextInputEvent args)
        {
            base.OnTextEntered(args);
            var text = args.Text;
            if (text.Length <= 0 || char.IsControl(text, 0)) return;
            Text += text;
        }
    }
}
