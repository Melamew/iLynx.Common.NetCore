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
using iLynx.UI.Sfml.Rendering;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.Sfml.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract class SfmlControlBase : UIElement
    {
        private Color background = Color.White;
        private Shape shape;
        private Func<Vector2f, Shape> backgroundGenerator = size => new RectangleShape(size);

        public Func<Vector2f, Shape> BackgroundGenerator
        {
            get => backgroundGenerator;
            set
            {
                if (value == backgroundGenerator) return;
                if (null == value) return;
                var old = backgroundGenerator;
                backgroundGenerator = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected SfmlControlBase(Alignment horizontalAlignment = Alignment.Start,
            Alignment verticalAlignment = Alignment.Start)
            : base(horizontalAlignment, verticalAlignment)
        {
            HookEvents();
        }

        private void HookEvents()
        {
            //InputHandler.MouseDown += InputHandler_MouseDown;
            //InputHandler.MouseUp += InputHandler_MouseUp;
            //InputHandler.MouseMove += InputHandler_MouseMove;
        }

        private void UnhookEvents()
        {
            //InputHandler.MouseDown -= InputHandler_MouseDown;
            //InputHandler.MouseUp -= InputHandler_MouseUp;
            //InputHandler.MouseMove -= InputHandler_MouseMove;
        }

        private void InputHandler_MouseMove(Vector2f position)
        {
            //throw new NotImplementedException();
        }

        private void InputHandler_MouseUp(Vector2f position, Mouse.Button button)
        {
            //throw new NotImplementedException();
        }

        private void InputHandler_MouseDown(Vector2f position, Mouse.Button button)
        {
            //throw new NotImplementedException();
        }

        public Color Background
        {
            get => background;
            set
            {
                if (value == background) return;
                var old = background;
                background = value;
                if (null != shape) shape.FillColor = value;
                OnPropertyChanged(old, value);
            }
        }

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            shape = backgroundGenerator(finalRect.Size());
            shape.FillColor = background;
            return finalRect;
        }

        protected virtual void OnClick(MouseButtonEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        public event EventHandler<MouseButtonEventArgs> Clicked;

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            if (null == shape) return;
            target.Draw(shape, states);
        }

        public void Dispose()
        {
            shape?.Dispose();
        }
    }
}
