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
    public abstract class Panel : SfmlControlBase
    {
        private readonly List<IUIElement> children = new List<IUIElement>();
        public IEnumerable<IUIElement> Children => children;
        private Color background;
        private RenderTexture texture;
        private Sprite sprite;
        private Vector2u textureDimensions;
        private volatile bool requireNewTexture = true;

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

        private void OnChildLayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnLayoutPropertyChanged(e.PropertyName);
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

        private (RenderTexture, Sprite) GetRenderItems()
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
            var t = renderItems.Item1;
            if (null == t) return;
            var s = renderItems.Item2;
            var c = children.ToArray();
            var pos = ComputedPosition;
            t.Clear(background);
            foreach (var child in c)
                child.Draw(t, states);
            t.Display();
            states.Transform.Translate(pos);
            target.Draw(s, states);
        }

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            var result = target;
            var dimensions = (Vector2u)result.Size();
            requireNewTexture = null == texture || textureDimensions != dimensions;
            textureDimensions = dimensions;
            return result;
        }
    }
}