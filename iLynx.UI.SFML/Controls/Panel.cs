using System;
using System.Collections.Generic;
using System.Linq;
using iLynx.UI.Sfml;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public abstract class Panel : UIElement
    {
        private readonly List<IUIElement> children = new List<IUIElement>();
        public IEnumerable<IUIElement> Children => children;
        private Color background;
        private Geometry backgroundGeometry;
        private Vector2f size;

        public Color Background
        {
            get => background;
            set
            {
                if (value == background) return;
                var old = background;
                background = value;
                OnPropertyChanged(old, value);
                OnRenderPropertyChanged();
            }
        }

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

        public void AddChild(params IUIElement[] elements)
        {
            children.AddRange(elements);
            OnLayoutPropertyChanged();
        }

        public void RemoveChild(params IUIElement[] elements)
        {
            foreach (var element in elements)
                children.Remove(element);
            OnLayoutPropertyChanged();
        }

        protected override void DrawTransformed(RenderTarget target, RenderStates states)
        {
            target.Draw(backgroundGeometry);
            foreach (var child in children.Where(c => c.BoundingBox.Intersects(BoundingBox)))
                child.Draw(target, states);
        }

        protected override void PrepareRender()
        {
            backgroundGeometry = new RectangleGeometry(BoundingBox.Size(), background);
        }
    }

    public class AbsolutePositioningPanel : Panel
    {
        private readonly Dictionary<IUIElement, Vector2f> positions = new Dictionary<IUIElement, Vector2f>();

        public override FloatRect Layout(FloatRect target)
        {
            target = base.Layout(target);
            var totalSize = new Vector2f(target.Width, target.Height);
            foreach (var child in Children)
            {
                if (!positions.TryGetValue(child, out var position))
                    position = new Vector2f();
                var subTarget = new FloatRect(position, totalSize - position);
                child.Layout(subTarget);
            }
            return target;
        }

        public void SetPosition(IUIElement element, Vector2f position)
        {
            if (Children.All(x => x != element))
                throw new InvalidOperationException("The specified target element is not contained in this canvas");
            positions.AddOrUpdate(element, position);
            OnLayoutPropertyChanged();
        }
    }
}