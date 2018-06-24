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
        private uint caretIndex = 0;

        protected override void OnMouseButtonDown(MouseDownEvent args)
        {
            base.OnMouseButtonDown(args);
            switch (args.Button)
            {
                case Mouse.Button.Left:
                    LeftMouseDown(args.Position);
                    break;
                case Mouse.Button.Right:
                    RightMouseDown(args.Position);
                    break;
                default:
                    return;
            }

            SetCaretIndex(args.Position);
        }

        private void SetCaretIndex(Vector2f position)
        {
            for (uint i = 0; i < Text.Length; ++i)
            {
                var charPos = FindCharacterPosition(i);

            }
        }

        private void RightMouseDown(Vector2f position)
        {

        }

        private void LeftMouseDown(Vector2f position)
        {

        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            base.DrawInternal(target, states);
        }

        protected override void OnTextEntered(TextInputEvent args)
        {
            base.OnTextEntered(args);
            //Console.WriteLine($"{args.Text.Length}");
        }
    }
}
