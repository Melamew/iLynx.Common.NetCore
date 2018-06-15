using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using iLynx.Common;
using iLynx.UI.Sfml;
using SFML.Graphics;

namespace iLynx.UI.SFML.Controls
{
    public class DisposableMonitor : IDisposable
    {
        private readonly object target;

        public DisposableMonitor(object target)
        {
            this.target = target;
            Monitor.Enter(this.target);
        }

        public void Dispose()
        {
            Monitor.Exit(target);
        }
    }
    // ReSharper disable once InconsistentNaming
    public abstract partial class UIElement : BindingSource, IUIElement
    {
        private Thickness margin = Thickness.Zero;
        protected Transform RenderTransform = Transform.Identity;
        //private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
        private readonly object lockObject = new object();
        
        protected DisposableMonitor AcquireLock()
        {
            return new DisposableMonitor(lockObject);
        }

        public virtual FloatRect Layout(FloatRect target)
        {
            using (AcquireLock())
            {
                target -= margin;
                return (BoundingBox = LayoutInternal(target)) + margin;
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
            using (AcquireLock())
                DrawLocked(target, states);
        }

        protected abstract void DrawLocked(RenderTarget target, RenderStates states);
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