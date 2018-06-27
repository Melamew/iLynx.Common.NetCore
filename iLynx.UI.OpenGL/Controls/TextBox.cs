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
using iLynx.UI.OpenGL.Animation;
using iLynx.UI.OpenGL.Input;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.OpenGL.Controls
{
    public class TextBox : TextElement
    {
        private int caretIndex;
        private readonly TimeSpan caretTransitionDuration = TimeSpan.FromMilliseconds(50d);
        private readonly RectangleShape caret = new RectangleShape();
        private readonly ReaderWriterLockSlim caretLock = new ReaderWriterLockSlim();
        private IAnimation caretAnimation;
        private bool isModifying;
        private bool acceptsNewLine;

        protected override void OnMouseButtonDown(MouseDownEventArgs args)
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

        protected override void OnBoundingBoxChanged(FloatRect oldBox, FloatRect newBox)
        {
            base.OnBoundingBoxChanged(oldBox, newBox);
            MoveCaretToCurrentIndex();
        }

        public bool AcceptsReturn
        {
            get => acceptsNewLine;
            set
            {
                if (value == acceptsNewLine) return;
                var old = acceptsNewLine;
                acceptsNewLine = value;
                if (!value)
                    Text = Text.Replace(NewLine, Space);
                OnPropertyChanged(old, value);
            }
        }

        protected override void OnTextChanged(string oldValue, string newValue)
        {
            base.OnTextChanged(oldValue, newValue);
            if (isModifying) return;
            CaretIndex = newValue.Length;
        }

        private void SetCaretIndex(Vector2f position)
        {
            for (var i = Text.Length - 1; i >= 0; --i)
            {
                if (char.IsControl(Text[i])) continue;
                var rect = GetCharacterRectangleForIndex(i);
                if (!rect.Contains(position.X, position.Y)) continue;
                if (i == Text.Length - 1 && rect.Left + rect.Width / 2f < position.X)
                    CaretIndex = i + 1;
                else
                    CaretIndex = i;
                break;
            }
        }

        protected virtual Vector2f CharacterSize => new Vector2f(FontSize, FontSize);

        private FloatRect GetCharacterRectangleForIndex(int index)
        {
            var size = (float)FontSize;
            var charPos = FindCharacterPosition((uint)index);
            return new FloatRect(new Vector2f(charPos.X - size / 4f, charPos.Y), CharacterSize);
        }

        private void MoveCaretToCurrentIndex()
        {
            var index = caretIndex;
            var caretSize = CharacterSize;
            caretSize.X *= 0.1f;
            var rect = GetCharacterRectangleForIndex(index);
            var position = new Vector2f(rect.Left + caretSize.X * 2f, rect.Top);
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
            caretAnimation?.Tick(caretTransitionDuration);
            var oldPos = caret.Position - RenderPosition;
            var delta = position - oldPos;
            caretAnimation = Animator.Start(d =>
            {
                using (caretLock.AcquireWriteLock())
                    caret.Position = oldPos + delta * (float)d + RenderPosition;
            }, caretTransitionDuration, easingFunction: EasingFunctions.QuadraticOut);
        }

        protected override void DrawLocked(RenderTarget target, RenderStates states)
        {
            base.DrawLocked(target, states);
            if (!HasFocus) return;
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

        protected override void OnKeyDown(KeyboardEventArgs args)
        {
            base.OnKeyDown(args);
            switch (args.Key)
            {
                case Keyboard.Key.BackSpace:
                    BackSpace();
                    break;
                case Keyboard.Key.Return:
                    Return();
                    break;
                case Keyboard.Key.Left:
                    Left();
                    break;
                case Keyboard.Key.Right:
                    Right();
                    break;
                case Keyboard.Key.Up:
                    Up();
                    break;
                case Keyboard.Key.Down:
                    Down();
                    break;
                case Keyboard.Key.End:
                    End();
                    break;
                case Keyboard.Key.Home:
                    Home();
                    break;
                case Keyboard.Key.Delete:
                    Delete();
                    break;
                default:
                    return;
            }

        }

        private void Delete()
        {
            if (Text.Length == caretIndex) return;
            isModifying = true;
            Text = Text.Remove(caretIndex, 1);
            isModifying = false;
        }

        private void Home()
        {
            CaretIndex = 0;
        }

        private void End()
        {
            CaretIndex = Text.Length;
        }

        private void Down()
        {
        }

        private void Up()
        {
        }

        protected int CaretIndex
        {
            get => caretIndex;
            set
            {
                if (value == caretIndex) return;
                caretIndex = value;
                MoveCaretToCurrentIndex();
            }
        }

        protected virtual void Right()
        {
            if (caretIndex == Text.Length) return;
            ++CaretIndex;
        }

        protected virtual void Left()
        {
            if (caretIndex == 0) return;
            --CaretIndex;
        }

        protected virtual void Return()
        {
            if (!acceptsNewLine) return;
            isModifying = true;
            Text = Text.Insert(caretIndex, NewLine);
            isModifying = false;
            ++CaretIndex;
        }

        protected virtual void BackSpace()
        {
            if (caretIndex == 0) return;
            isModifying = true;
            Text = Text.Remove(caretIndex - 1, 1);
            isModifying = false;
            --CaretIndex;
        }

        protected override void OnTextEntered(TextInputEventArgs args)
        {
            base.OnTextEntered(args);
            var text = args.Text;
            if (text.Length <= 0 || char.IsControl(text, 0)) return;
            isModifying = true;
            Text = Text.Insert(caretIndex, text);
            isModifying = false;
            ++CaretIndex;
        }
    }
}
