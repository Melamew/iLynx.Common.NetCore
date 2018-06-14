using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using iLynx.UI.Sfml;
using SFML.Graphics;

namespace iLynx.UI.SFML.Controls
{
    public class ReaderLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rwl;

        public ReaderLock(ReaderWriterLockSlim rwl)
        {
            this.rwl = rwl;
            this.rwl.EnterReadLock();
        }

        public void Dispose()
        {
            rwl.ExitReadLock();
        }
    }

    public class WriterLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rwl;

        public WriterLock(ReaderWriterLockSlim rwl)
        {
            this.rwl = rwl;
            this.rwl.EnterWriteLock();
        }

        public void Dispose()
        {
            rwl.ExitWriteLock();
        }
    }

    public abstract class Panel : SfmlControlBase
    {
        private readonly List<IUIElement> children = new List<IUIElement>();
        private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
        public IEnumerable<IUIElement> Children => children;
        private Color background;
        private Geometry backgroundGeometry;

        public Color Background
        {
            get => background;
            set
            {
                if (value == background) return;
                var old = background;
                background = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public void AddChild(params IUIElement[] elements)
        {
            using (AcquireWriterLock())
            {
                children.AddRange(elements.Select(x =>
                {
                    x.LayoutPropertyChanged += OnChildLayoutPropertyChanged;
                    return x;
                }));
            }
            OnLayoutPropertyChanged();
        }

        protected ReaderLock AcquireReaderLock()
        {
            return new ReaderLock(rwl);
        }

        protected WriterLock AcquireWriterLock()
        {
            return new WriterLock(rwl);
        }

        private void OnChildLayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnLayoutPropertyChanged(e.PropertyName);
        }

        public void RemoveChild(params IUIElement[] elements)
        {
            using (AcquireWriterLock())
            {
                foreach (var element in elements)
                {
                    children.Remove(element);
                    element.LayoutPropertyChanged -= OnChildLayoutPropertyChanged;
                }
            }
            OnLayoutPropertyChanged();
        }

        protected override void DrawTransformed(RenderTarget target, RenderStates states)
        {
            if (null == backgroundGeometry) return;
            using (AcquireReaderLock())
            {
                target.Draw(backgroundGeometry, states);
                foreach (var child in children.Where(c => c.BoundingBox.Intersects(BoundingBox)))
                    child.Draw(target, states);
            }
        }

        protected override FloatRect ComputeBoundingBox(FloatRect destinationRect)
        {
            var result = base.ComputeBoundingBox(destinationRect);
            backgroundGeometry = new RectangleGeometry(result.Size(), background);
            return result;
        }
    }
}