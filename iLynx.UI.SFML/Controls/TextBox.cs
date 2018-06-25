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
using System.Threading;
using iLynx.Common.Threading;
using iLynx.UI.Sfml.Animation;
using iLynx.UI.Sfml.Input;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.Sfml.Controls
{
    public class TextBox : TextElement
    {
        private int caretIndex;
        private readonly TimeSpan caretTransitionDuration = TimeSpan.FromMilliseconds(50d);
        private readonly RectangleShape caret = new RectangleShape();
        private readonly ReaderWriterLockSlim caretLock = new ReaderWriterLockSlim();
        private IAnimation caretAnimation;

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
            for (var i = Text.Length - 1; i >= 0; --i)
            {
                if (char.IsControl(Text[i])) continue;
                var rect = GetCharacterRectangleForIndex(i);
                if (!rect.Contains(position.X, position.Y)) continue;
                if (i == Text.Length - 1 && rect.Left + rect.Width / 2f < position.X)
                    caretIndex = i + 1;
                else
                    caretIndex = i;
                break;
            }

            BuildCaret();
        }

        protected virtual Vector2f CharacterSize => new Vector2f(FontSize, FontSize);

        private FloatRect GetCharacterRectangleForIndex(int index)
        {
            var size = (float)FontSize;
            var charPos = FindCharacterPosition((uint)index);
            return new FloatRect(new Vector2f(charPos.X - size / 4f, charPos.Y), CharacterSize);
        }

        private void BuildCaret()
        {
            var index = caretIndex;
            Vector2f position;
            FloatRect rect;
            var caretSize = CharacterSize;
            caretSize.X *= 0.1f;
            if (index >= Text.Length)
            {
                rect = GetCharacterRectangleForIndex(Text.Length - 1);
                position = new Vector2f(rect.Left + rect.Width - caretSize.X * 2f, rect.Top);
            }
            else
            {
                rect = GetCharacterRectangleForIndex(index);
                position = new Vector2f(rect.Left + caretSize.X * 2f, rect.Top);
            }
            using (caretLock.AcquireWriteLock())
            {
                caret.Size = caretSize;
                caret.FillColor = Foreground;
            }
            SetCaretPosition(position);
        }

        protected virtual void SetCaretPosition(Vector2f position)
        {
            caretAnimation?.Cancel();
            var oldPos = caret.Position - RenderPosition;
            var delta = position - oldPos;
            caretAnimation = Animator.Start(d =>
            {
                using (caretLock.AcquireWriteLock())
                    caret.Position = oldPos + delta * (float)d + RenderPosition;
            }, caretTransitionDuration, easingFunction: EasingFunctions.QuadraticOut);
        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            base.DrawInternal(target, states);
            caretLock.EnterReadLock();
            target.Draw(caret);
            caretLock.ExitReadLock();
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
