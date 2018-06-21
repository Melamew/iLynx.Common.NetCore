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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using iLynx.Common;
using iLynx.Common.Threading;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Controls
{
    public enum Alignment
    {
        Start = 0,
        Center = 1,
        End = 2,
        Stretch = 3
    }

    // ReSharper disable once InconsistentNaming
    public abstract class UIElement : BindingSource, IUIElement
    {
        private Thickness margin = Thickness.Zero;
        protected Transform RenderTransform = Transform.Identity;
        private Vector2f size = new Vector2f(float.NaN, float.NaN);
        private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
        private Alignment horizontalAlignment;
        private Alignment verticalAlignment;

        protected UIElement(Alignment horizontalAlignment = Alignment.Start,
            Alignment verticalAlignment = Alignment.Start)
        {
            this.horizontalAlignment = horizontalAlignment;
            this.verticalAlignment = verticalAlignment;
        }

        protected WriterLock AcquireWriteLock()
        {
            return new WriterLock(rwl);
        }

        protected ReaderLock AcquireReadLock()
        {
            return new ReaderLock(rwl);
        }

        public virtual FloatRect Layout(FloatRect target)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                rwl.EnterWriteLock();
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
                BoundingBox = LayoutInternal(final);
                return BoundingBox; // The final layout size of this element is it's bounding box plus margins
            }
            finally
            {
                rwl.ExitWriteLock();
                sw.Stop();
                //Console.WriteLine($"Layout for {this} finished in {sw.Elapsed.TotalMilliseconds}ms");
            }
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

        public Vector2f RenderPosition => new Vector2f(BoundingBox.Left, BoundingBox.Top);

        public Vector2f RenderSize => new Vector2f(BoundingBox.Width, BoundingBox.Height);

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


        /// <summary>
        /// Called when the layout process has determined the internal bounds of this element
        /// </summary>
        /// <param name="finalRect"></param>
        /// <returns></returns>
        protected abstract FloatRect LayoutInternal(FloatRect finalRect);

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

        protected virtual void OnLayoutPropertyChanged([CallerMemberName] string propertyName = null)
        {
            LayoutPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<PropertyChangedEventArgs> LayoutPropertyChanged;

        public FloatRect BoundingBox { get; private set; }
        public Vector2f ToLocalCoords(Vector2f coords)
        {
            return (Parent?.ToLocalCoords(coords) ?? coords) - RenderPosition;
        }

        public Vector2f ToGlobalCoords(Vector2f coords)
        {
            return (Parent?.ToGlobalCoords(coords) ?? coords) + RenderPosition;
        }

        public void SetLogicalParent(IUIElement parent)
        {
            Parent = parent;
        }

        public IUIElement Parent { get; protected set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            rwl.EnterReadLock();
            states.Transform.Translate(RenderPosition);
            DrawInternal(target, states);
            rwl.ExitReadLock();
        }

        protected abstract void DrawInternal(RenderTarget target, RenderStates states);

        public static Font DefaultFont => new Font("fonts/Mechanical.otf");
        //public static Font DefaultFont => new Font("fonts/OpenSans-Regular.ttf");
    }
}