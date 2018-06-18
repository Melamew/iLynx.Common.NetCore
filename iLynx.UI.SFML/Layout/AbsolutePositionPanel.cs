using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Layout
{
    public class AbsolutePositionPanel : Panel
    {
        private readonly Dictionary<IUIElement, Vector2f> positions = new Dictionary<IUIElement, Vector2f>();

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            target = base.LayoutInternal(target);
            var totalSize = new Vector2f(target.Width, target.Height);
            foreach (var child in Children)
            {
                if (!positions.TryGetValue(child, out var position))
                    position = new Vector2f();
                var relativePosition = position;
                var subTarget = new FloatRect(relativePosition, totalSize - relativePosition);
                child.Layout(subTarget);
            }
            return target;
        }

        public void SetGlobalPosition(IUIElement element, Vector2f position)
        {
            SetRelativePosition(element, position - ComputedPosition);
        }

        public void SetRelativePosition(IUIElement element, Vector2f position)
        {
            if (Children.All(x => x != element))
                throw new InvalidOperationException("The specified target element is not contained in this canvas");
            positions.AddOrUpdate(element, position);
            OnLayoutPropertyChanged(nameof(Children));
        }

        public Vector2f GetGlobalPosition(IUIElement element)
        {
            return GetRelativePosition(element) - ComputedPosition;
        }

        public Vector2f GetRelativePosition(IUIElement element)
        {
            return !positions.TryGetValue(element, out var value) ? new Vector2f() : value;
        }
    }
}