using System;
using System.Collections.Generic;
using System.Linq;
using iLynx.UI.Sfml;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public class AbsolutePositionPanel : Panel
    {
        private readonly Dictionary<IUIElement, Vector2f> positions = new Dictionary<IUIElement, Vector2f>();

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            target = base.LayoutInternal(target);
            var totalSize = new Vector2f(target.Width, target.Height);
            var offset = new Vector2f(target.Left, target.Top);
            foreach (var child in Children)
            {
                if (!positions.TryGetValue(child, out var position))
                    position = new Vector2f();
                var relativePosition = position - offset;
                var subTarget = new FloatRect(relativePosition, totalSize - relativePosition);
                child.Layout(subTarget);
            }
            return target;
        }

        public void SetPosition(IUIElement element, Vector2f position)
        {
            using (AcquireLock())
            {
                if (Children.All(x => x != element))
                    throw new InvalidOperationException("The specified target element is not contained in this canvas");
                positions.AddOrUpdate(element, position);
            }
            OnLayoutPropertyChanged();
        }
    }
}