using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using iLynx.UI.Sfml;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public class ReaderLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rwl;

        public ReaderLock(ReaderWriterLockSlim rwl)
        {
            this.rwl = rwl;
            this.rwl.EnterUpgradeableReadLock();
        }

        public void Dispose()
        {
            rwl.ExitUpgradeableReadLock();
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
        public IEnumerable<IUIElement> Children => children;
        private Color background;
        private RenderTexture texture;
        private bool requireNewTexture = true;
        private Vector2u textureDimensions;

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
            using (AcquireLock())
            {
                children.AddRange(elements.Select(x =>
                {
                    x.LayoutPropertyChanged += OnChildLayoutPropertyChanged;
                    return x;
                }));
            }
            OnLayoutPropertyChanged();
        }

        private void OnChildLayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnLayoutPropertyChanged(e.PropertyName);
        }

        public void RemoveChild(params IUIElement[] elements)
        {
            using (AcquireLock())
            {
                foreach (var element in elements)
                {
                    children.Remove(element);
                    element.LayoutPropertyChanged -= OnChildLayoutPropertyChanged;
                }
            }
            OnLayoutPropertyChanged();
        }

        protected override void DrawLocked(RenderTarget target, RenderStates states)
        {
            if (textureDimensions.X == 0 || textureDimensions.Y == 0) return;
            if (requireNewTexture || null == texture)
            {
                texture?.Dispose();
                texture = null;
                texture = new RenderTexture(textureDimensions.X, textureDimensions.Y);
                requireNewTexture = false;
            }
            texture.Clear(background);
            foreach (var child in children)
                child.Draw(texture, states);
            texture.Display();
            var sprite = new Sprite(texture.Texture);
            states.Transform.Translate(ComputedPosition);
            target.Draw(sprite, states);
        }

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            var result = base.LayoutInternal(target);
            var dimensions = (Vector2u) result.Size();
            textureDimensions = dimensions;
            requireNewTexture = dimensions != textureDimensions;
            return result;
        }
    }
}