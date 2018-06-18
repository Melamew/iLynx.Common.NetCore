using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using iLynx.Common;
using iLynx.Common.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.Sfml.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract partial class UIElement : BindingSource, IUIElement
    {
        private Thickness margin = Thickness.Zero;
        protected Transform RenderTransform = Transform.Identity;
        private Vector2f size = new Vector2f(float.NaN, float.NaN);
        private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();

        public virtual FloatRect Layout(FloatRect target)
        {
            try
            {
                rwl.EnterWriteLock();
                var dims = size;
                var m = margin;
                target -= m; // Shrink available space to account for the element's margin
                if (!float.IsNaN(dims.X) && dims.X < target.Width)
                    target.Width = dims.X;
                if (!float.IsNaN(dims.Y) && dims.Y < target.Height)
                    target.Height = dims.Y;
                BoundingBox = LayoutInternal(target);
                return BoundingBox + m; // The final layout size of this element is it's bounding box plus margins
            }
            finally
            {
                rwl.ExitWriteLock();
            }
        }

        public Vector2f ComputedPosition => new Vector2f(BoundingBox.Left, BoundingBox.Top);

        public Vector2f ComputedSize => new Vector2f(BoundingBox.Width, BoundingBox.Height);

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
        /// <param name="target"></param>
        /// <returns></returns>
        protected abstract FloatRect LayoutInternal(FloatRect target);

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
    }

    // ReSharper disable once InconsistentNaming
    public abstract partial class UIElement : BindingSource, IUIElement
    {
        static UIElement()
        {
            DefaultFont = new Font("fonts/Mechanical.otf");
        }
        public static Font DefaultFont { get; }
    }
}