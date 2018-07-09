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
using iLynx.Graphics;
using iLynx.Graphics.Animation;
using iLynx.UI.OpenGL.Input;
using OpenTK;
using OpenTK.Input;

namespace iLynx.UI.OpenGL.Controls
{
    public class TextBox : TextElement
    {
        private int caretIndex;
        private readonly TimeSpan caretTransitionDuration = TimeSpan.FromMilliseconds(50d);
        //private readonly RectangleShape caret = new RectangleShape();
        private readonly ReaderWriterLockSlim caretLock = new ReaderWriterLockSlim();
        private IAnimation caretAnimation;
        private bool isModifying;
        private bool acceptsNewLine;

        protected override void OnMouseButtonDown(MouseDownEventArgs args)
        {
            base.OnMouseButtonDown(args);
            var relPos = ToLocalCoords(args.Position.AsVector());
            switch (args.Button)
            {
                case MouseButton.Left:
                    LeftMouseDown(relPos);
                    break;
                case MouseButton.Right:
                    RightMouseDown(relPos);
                    break;
                default:
                    return;
            }

            SetCaretIndex(relPos);
        }

        protected override void OnBoundingBoxChanged(RectangleF oldBox, RectangleF newBox)
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

        private void SetCaretIndex(Vector2 position)
        {
            for (var i = Text.Length - 1; i >= 0; --i)
            {
                if (char.IsControl(Text[i])) continue;
                var rect = GetCharacterRectangleForIndex(i);
                if (!rect.Contains(position.AsPoint())) continue;
                if (i == Text.Length - 1 && rect.Left + rect.Width / 2f < position.X)
                    CaretIndex = i + 1;
                else
                    CaretIndex = i;
                break;
            }
        }

        protected virtual SizeF CharacterSize => new SizeF(FontSize, FontSize);

        private RectangleF GetCharacterRectangleForIndex(int index)
        {
            var size = (float)FontSize;
            var charPos = FindCharacterPosition((uint)index);
            return new RectangleF(new PointF(charPos.X - size / 4f, charPos.Y), CharacterSize);
        }

        private void MoveCaretToCurrentIndex()
        {
            var index = caretIndex;
            var caretSize = CharacterSize;
            caretSize.Width *= 0.1f;
            var rect = GetCharacterRectangleForIndex(index);
            var position = new Vector2(rect.Left + caretSize.Width * 2f, rect.Top);
            using (caretLock.AcquireWriteLock())
            {
                //caret.Size = caretSize;
                //caret.FillColor = Foreground;
            }
            SetCaretPosition(position);
        }

        protected virtual void SetCaretPosition(Vector2 position)
        {
            caretAnimation?.Cancel();
            caretAnimation?.Tick(caretTransitionDuration);
            //var oldPos = caret.Position - RenderPosition;
            //var delta = position - oldPos;
            //caretAnimation = Animator.Start(d =>
            //{
            //    using (caretLock.AcquireWriteLock())
            //        caret.Position = oldPos + delta * (float)d + RenderPosition;
            //}, caretTransitionDuration, easingFunction: EasingFunctions.QuadraticOut);
        }

        protected override void DrawLocked(IRenderStates states)
        {
            base.DrawLocked(states);
            //if (!HasFocus) return;
            //caretLock.EnterReadLock();
            //target.Draw(caret);
            //caretLock.ExitReadLock();
        }

        private void RightMouseDown(Vector2 position)
        {

        }

        private void LeftMouseDown(Vector2 position)
        {

        }

        protected override void OnKeyDown(KeyboardEventArgs args)
        {
            base.OnKeyDown(args);
            switch (args.Key)
            {
                case Key.BackSpace:
                    BackSpace();
                    break;
                case Key.Enter:
                    Return();
                    break;
                case Key.Left:
                    Left();
                    break;
                case Key.Right:
                    Right();
                    break;
                case Key.Up:
                    Up();
                    break;
                case Key.Down:
                    Down();
                    break;
                case Key.End:
                    End();
                    break;
                case Key.Home:
                    Home();
                    break;
                case Key.Delete:
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
