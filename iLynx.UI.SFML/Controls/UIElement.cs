﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using iLynx.Common;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Controls
{
    public enum Alignment
    {
        Start = 0,
        Centre = 1,
        End = 2,
        Fill = 3
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

        public virtual FloatRect Layout(FloatRect target)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                rwl.EnterWriteLock();
                var va = verticalAlignment;
                var ha = horizontalAlignment;
                target -= margin;
                var computedSize = Measure(target.Size());
                var final = target;
                final.Width = computedSize.X;
                final.Height = computedSize.Y;
                switch (ha)
                {
                    case Alignment.Fill when float.IsNaN(size.X):
                        final.Width = target.Width;
                        break;
                    case Alignment.Centre:
                        final.Left = final.Left + (target.Width / 2f - computedSize.X / 2f);
                        break;
                    case Alignment.End:
                        final.Left = final.Left + target.Width - computedSize.X;
                        break;
                }

                switch (va)
                {
                    case Alignment.Fill when float.IsNaN(size.Y):
                        final.Height = target.Height;
                        break;
                    case Alignment.Centre:
                        final.Top = final.Top + (target.Height / 2f - computedSize.Y / 2f);
                        break;
                    case Alignment.End:
                        final.Top = final.Top + target.Height - computedSize.Y;
                        break;
                }
                BoundingBox = LayoutInternal(final);
                return BoundingBox + margin; // The final layout size of this element is it's bounding box plus margins
            }
            finally
            {
                rwl.ExitWriteLock();

                sw.Stop();
                Console.WriteLine($"Layout for {this} finished in {sw.Elapsed.TotalMilliseconds}ms");
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

        public void Draw(RenderTarget target, RenderStates states)
        {
            rwl.EnterReadLock();
            DrawInternal(target, states);
            rwl.ExitReadLock();
        }

        protected abstract void DrawInternal(RenderTarget target, RenderStates states);

        static UIElement()
        {
            DefaultFont = new Font("fonts/Mechanical.otf");
        }
        public static Font DefaultFont { get; }
    }
}