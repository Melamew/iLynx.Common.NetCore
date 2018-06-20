using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using iLynx.Common.Threading;
using iLynx.UI.Sfml.Controls;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Layout
{
    public abstract class Panel : UIElement
    {
        private readonly List<IUIElement> children = new List<IUIElement>();
        public IEnumerable<IUIElement> Children => children;
        private Color background = Color.Transparent;
        private RenderTexture texture;
        private Sprite sprite;
        private Vector2u textureDimensions;
        private volatile bool requireNewTexture = true;

        protected Panel()
            : base(Alignment.Stretch, Alignment.Stretch)
        {
        }

        public Color Background
        {
            get => background;
            set
            {
                if (value == background) return;
                var old = background;
                background = value;
                OnPropertyChanged(old, value);
            }
        }

        protected override void OnLayoutPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnLayoutPropertyChanged(propertyName);
        }

        public void AddChild(params IUIElement[] elements)
        {
            children.AddRange(elements.Select(x =>
            {
                x.LayoutPropertyChanged += OnChildLayoutPropertyChanged;
                return x;
            }));
            OnLayoutPropertyChanged(nameof(Children));
        }

        protected virtual void OnChildLayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (null == texture || requireNewTexture)
                OnLayoutPropertyChanged(e.PropertyName);
            else
                Layout(BoundingBox + Margin);
        }

        public void RemoveChild(params IUIElement[] elements)
        {
            foreach (var element in elements)
            {
                children.Remove(element);
                element.LayoutPropertyChanged -= OnChildLayoutPropertyChanged;
            }
            OnLayoutPropertyChanged(nameof(Children));
        }

        private (RenderTexture texture, Sprite sprite) GetRenderItems()
        {
            if (requireNewTexture && textureDimensions.X > 0 && textureDimensions.Y > 0)
            {
                texture?.Dispose();
                texture = null;
                texture = new RenderTexture(textureDimensions.X, textureDimensions.Y);
                sprite = new Sprite(texture.Texture);
                requireNewTexture = false;
            }
            return (texture, sprite);
        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            var renderItems = GetRenderItems();
            var t = renderItems.texture;
            if (null == t) return;
            var s = renderItems.sprite;
            var c = children.ToArray();
            t.Clear(background);
            var childStates = new RenderStates(states) {Transform = Transform.Identity};
            foreach (var child in c)
                child.Draw(t, childStates);
            t.Display();
            target.Draw(s, states);
        }

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            var dimensions = (Vector2u)finalRect.Size();
            requireNewTexture = null == texture || textureDimensions != dimensions;
            textureDimensions = dimensions;
            LayoutChildren(new FloatRect(0f, 0f, dimensions.X, dimensions.Y));
            return finalRect;
        }

        protected abstract void LayoutChildren(FloatRect target);
    }
}