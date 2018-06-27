﻿#region LICENSE
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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using iLynx.Common;
using iLynx.UI.Sfml.Input;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract class UIElement : BindingSource, IUIElement
    {
        protected readonly ReaderWriterLockSlim LayoutLock = new ReaderWriterLockSlim();
        private Alignment horizontalAlignment;
        private Thickness margin = Thickness.Zero;
        private Vector2f size = new Vector2f(float.NaN, float.NaN);
        private Alignment verticalAlignment;
        private bool hasFocus;
        private bool isMouseOver;
        private bool isFocusable = true;
        private bool isHitTestVisible = true;
        private FloatRect boundingBox;
        private Vector2f renderSize;
        private Vector2f renderPosition;

        static UIElement()
        {
            InputHandler.Register<UIElement, MouseEventArgs>((e, args) => e?.OnMouseOver(args));
            InputHandler.Register<UIElement, MouseDownEventArgs>((e, args) => e?.OnMouseButtonDown(args));
            InputHandler.Register<UIElement, MouseUpEventArgs>((e, args) => e?.OnMouseButtonUp(args));
            InputHandler.Register<UIElement, GotFocusEventArgs>((e, args) => e?.OnGotFocus());
            InputHandler.Register<UIElement, LostFocusEventArgs>((e, args) => e?.OnLostFocus());
            InputHandler.Register<UIElement, KeyDownEventArgs>((e, args) => e?.OnKeyDown(args));
            InputHandler.Register<UIElement, KeyUpEventArgs>((e, args) => e?.OnKeyUp(args));
            InputHandler.Register<UIElement, TextInputEventArgs>((e, args) => e?.OnTextEntered(args));
            InputHandler.Register<UIElement, MouseEnterEventArgs>((e, args) => e?.OnMouseEntered(args));
            InputHandler.Register<UIElement, MouseLeaveEventArgs>((e, args) => e?.OnMouseLeft(args));
        }

        protected virtual void OnMouseLeft(MouseLeaveEventArgs args)
        {
            Console.WriteLine($"Mouse Left: {this} at {args.Position}");
        }

        protected virtual void OnMouseEntered(MouseEnterEventArgs args)
        {
            Console.WriteLine($"Mouse Entered: {this} at {args.Position}");
        }

        protected UIElement(Alignment horizontalAlignment = Alignment.Start,
            Alignment verticalAlignment = Alignment.Start)
        {
            this.horizontalAlignment = horizontalAlignment;
            this.verticalAlignment = verticalAlignment;
        }

        public Alignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (value == horizontalAlignment) return;
                var old = horizontalAlignment;
                horizontalAlignment = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Alignment VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (value == verticalAlignment) return;
                var old = verticalAlignment;
                verticalAlignment = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Vector2f RenderPosition
        {
            get => renderPosition;
            private set
            {
                if (value == renderPosition) return;
                var old = renderPosition;
                renderPosition = value;
                OnPropertyChanged(old, value);
                OnRenderPositionChanged(old, value);
            }
        }

        protected virtual void OnRenderPositionChanged(Vector2f old, Vector2f value)
        {
            RenderPositionChanged?.Invoke(this, new ValueChangedEventArgs<Vector2f>(old, value));
        }

        public Vector2f RenderSize
        {
            get => renderSize;
            private set
            {
                if (value == renderSize) return;
                var old = renderSize;
                renderSize = value;
                OnPropertyChanged(old, value);
                OnRenderSizeChanged(old, value);
            }
        }

        protected virtual void OnRenderSizeChanged(Vector2f old, Vector2f value)
        {
            RenderSizeChanged?.Invoke(this, new ValueChangedEventArgs<Vector2f>(old, value));
        }

        public event ValueChangedEventHandler<IRenderElement, FloatRect> BoundingBoxChanged;
        public event ValueChangedEventHandler<IRenderElement, Vector2f> RenderSizeChanged;
        public event ValueChangedEventHandler<IRenderElement, Vector2f> RenderPositionChanged;

        public Vector2f Size
        {
            get => size;
            set
            {
                if (value == size) return;
                var old = size;
                size = value;
                OnPropertyChanged(old, size);
                OnLayoutPropertyChanged();
            }
        }

        public static Font DefaultFont => new Font("fonts/Mechanical.otf");
        public Vector2f ToLocalCoords(Vector2f coords)
        {
            return (Parent?.ToLocalCoords(coords) ?? coords) - RenderPosition;
        }

        public Vector2f ToGlobalCoords(Vector2f coords)
        {
            return (Parent?.ToGlobalCoords(coords) ?? coords) + RenderPosition;
        }

        public virtual bool HitTest(Vector2f position, out IInputElement element)
        {
            element = null;
            if (!IsHitTestVisible) return false;
            position = ToLocalCoords(position);
            if (!new FloatRect(new Vector2f(), RenderSize).Contains(position.X, position.Y)) return false;
            element = this;
            return true;
        }

        public bool IsFocusable
        {
            get => isFocusable;
            set
            {
                if (value == isFocusable) return;
                var old = isFocusable;
                isFocusable = value;
                OnPropertyChanged(old, isFocusable);
            }
        }

        public bool HasFocus
        {
            get => hasFocus;
            protected set
            {
                if (value == hasFocus) return;
                var old = hasFocus;
                hasFocus = value;
                OnPropertyChanged(old, value);
            }
        }
        public bool IsMouseOver
        {
            get => isMouseOver;
            protected set
            {
                if (value == isMouseOver) return;
                var old = isMouseOver;
                isMouseOver = value;
                OnPropertyChanged(old, value);
            }
        }

        public bool IsHitTestVisible
        {
            get => isHitTestVisible;
            set
            {
                if (value == isHitTestVisible) return;
                var old = isHitTestVisible;
                isHitTestVisible = value;
                OnPropertyChanged(old, value);
            }
        }

        public virtual FloatRect Layout(FloatRect target)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                LayoutLock.EnterWriteLock();
                var va = verticalAlignment;
                var ha = horizontalAlignment;
                var computedSize = Measure(target.Size()) + margin;
                var final = target;
                final.Width = computedSize.X;
                final.Height = computedSize.Y;
                switch (ha)
                {
                    case Alignment.Stretch when float.IsNaN(size.X):
                        final.Width = target.Width;
                        break;
                    case Alignment.Center:
                        final.Left = final.Left + (target.Width / 2f - computedSize.X / 2f);
                        break;
                    case Alignment.End:
                        final.Left = final.Left + target.Width - computedSize.X;
                        break;
                }

                switch (va)
                {
                    case Alignment.Stretch when float.IsNaN(size.Y):
                        final.Height = target.Height;
                        break;
                    case Alignment.Center:
                        final.Top = final.Top + (target.Height / 2f - computedSize.Y / 2f);
                        break;
                    case Alignment.End:
                        final.Top = final.Top + target.Height - computedSize.Y;
                        break;
                }

                final -= margin;
                BoundingBox = LayoutLocked(final);
                return BoundingBox; // The final layout size of this element is it's bounding box plus margins
            }
            finally
            {
                LayoutLock.ExitWriteLock();
                sw.Stop();
                //Console.WriteLine($"Layout for {this} finished in {sw.Elapsed.TotalMilliseconds}ms");
            }
        }

        public virtual Vector2f Measure(Vector2f availableSpace)
        {
            var dims = size;
            var result = new Vector2f();
            if (!float.IsNaN(dims.X) && dims.X < availableSpace.X)
                result.X = dims.X;
            else if (dims.X >= availableSpace.X)
                result.X = availableSpace.X;

            if (!float.IsNaN(dims.Y) && dims.Y < availableSpace.Y)
                result.Y = dims.Y;
            else if (dims.Y >= availableSpace.Y)
                result.Y = availableSpace.Y;
            return result;
        }

        public Thickness Margin
        {
            get => margin;
            set
            {
                if (value == margin) return;
                var old = margin;
                margin = value;
                OnPropertyChanged(old, margin);
                OnLayoutPropertyChanged();
            }
        }

        public event EventHandler<PropertyChangedEventArgs> LayoutPropertyChanged;

        public FloatRect BoundingBox
        {
            get => boundingBox;
            private set
            {
                if (value == boundingBox) return;
                var old = boundingBox;
                boundingBox = value;
                OnPropertyChanged(old, value);
                OnBoundingBoxChanged(old, value);
                RenderSize = value.Size();
                RenderPosition = value.Position();
            }
        }

        protected virtual void OnBoundingBoxChanged(FloatRect oldBox, FloatRect newBox)
        {
            BoundingBoxChanged?.Invoke(this, new ValueChangedEventArgs<FloatRect>(oldBox, newBox));
        }

        public void SetLogicalParent(IRenderElement parent)
        {
            Parent = parent;
            OnParentChanged();
        }

        protected virtual void OnParentChanged() { }

        public IRenderElement Parent { get; private set; }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            LayoutLock.EnterReadLock();
            states.Transform.Translate(RenderPosition);
            DrawLocked(target, states);
            LayoutLock.ExitReadLock();
        }

        /// <summary>
        ///     Called when the layout process has determined the internal bounds of this element
        /// </summary>
        /// <param name="finalRect"></param>
        /// <returns></returns>
        protected abstract FloatRect LayoutLocked(FloatRect finalRect);

        protected virtual void OnLayoutPropertyChanged([CallerMemberName] string propertyName = null)
        {
            LayoutPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnMouseOver(MouseEventArgs args)
        {
        }

        protected virtual void OnMouseButtonDown(MouseDownEventArgs args)
        {
        }

        protected virtual void OnGotFocus()
        {
            HasFocus = true;
            Console.WriteLine($"Got Focus: {this}");
        }

        protected virtual void OnLostFocus()
        {
            HasFocus = false;
            Console.WriteLine($"Lost Focus: {this}");
        }

        protected virtual void OnMouseButtonUp(MouseUpEventArgs args)
        {
        }

        protected virtual void OnKeyDown(KeyboardEventArgs args)
        {
        }

        protected virtual void OnKeyUp(KeyboardEventArgs args)
        {
        }

        protected virtual void OnTextEntered(TextInputEventArgs args)
        {
        }

        protected abstract void DrawLocked(RenderTarget target, RenderStates states);
    }
}