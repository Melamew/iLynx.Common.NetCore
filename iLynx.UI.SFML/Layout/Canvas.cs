using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Layout
{
    public class Canvas : Panel
    {
        private readonly Dictionary<IUIElement, Vector2f> positions = new Dictionary<IUIElement, Vector2f>();

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            finalRect = base.LayoutInternal(finalRect);
            var totalSize = new Vector2f(finalRect.Width, finalRect.Height);
            foreach (var child in Children)
            {
                if (!positions.TryGetValue(child, out var position))
                    position = new Vector2f();
                var relativePosition = position;
                var subTarget = new FloatRect(relativePosition, totalSize - relativePosition);
                child.Layout(subTarget);
            }
            return finalRect;
        }

        public void SetGlobalPosition(IUIElement element, Vector2f position)
        {
            SetRelativePosition(element, position - RenderPosition);
        }

        public void SetRelativePosition(IUIElement element, Vector2f position)
        {
            if (Children.All(x => x != element))
                throw new InvalidOperationException("The specified finalRect element is not contained in this canvas");
            positions.AddOrUpdate(element, position);
            OnLayoutPropertyChanged(nameof(Children));
        }

        public Vector2f GetGlobalPosition(IUIElement element)
        {
            return GetRelativePosition(element) - RenderPosition;
        }

        public Vector2f GetRelativePosition(IUIElement element)
        {
            return !positions.TryGetValue(element, out var value) ? new Vector2f() : value;
        }
    }
}