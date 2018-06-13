using System.Collections.Generic;
using SFML.Graphics;

namespace iLynx.UI.SFML.Controls
{
    public abstract class Panel : UIElement
    {
        private readonly List<IUIElement> children = new List<IUIElement>();
        public IEnumerable<IUIElement> Children => children;
        private Color background;

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

        public void AddChild(params IUIElement[] elements)
        {
            children.AddRange(elements);
        }

        public void RemoveChild(params IUIElement[] elements)
        {
            foreach (var element in elements)
                children.Remove(element);
        }

        protected override void DrawTransformed(RenderTarget target, RenderStates states)
        {
            foreach (var child in children)
                child.Draw(target, states);
        }
    }
}