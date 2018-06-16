using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using iLynx.UI.Sfml.Controls;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Layout
{
    public abstract class Panel : SfmlControlBase
    {
        private readonly List<IUIElement> children = new List<IUIElement>();
        public IEnumerable<IUIElement> Children => children;
        private Color background;
        private RenderTexture texture;
        private Sprite sprite;
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
                sprite = new Sprite(texture.Texture);
            }
            texture.Clear(background);
            foreach (var child in children)
                child.Draw(texture, states);
            texture.Display();
            states.Transform.Translate(ComputedPosition);
            target.Draw(sprite, states);
        }

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            var result = base.LayoutInternal(target);
            var dimensions = (Vector2u)result.Size();
            textureDimensions = dimensions;
            requireNewTexture = dimensions != textureDimensions;
            return result;
        }
    }
}