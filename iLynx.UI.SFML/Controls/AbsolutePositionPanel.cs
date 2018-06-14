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

        public override FloatRect Layout(FloatRect target)
        {
            target = base.Layout(target);
            var totalSize = new Vector2f(target.Width, target.Height);
            using (AcquireReaderLock())
            {
                foreach (var child in Children)
                {
                    if (!positions.TryGetValue(child, out var position))
                        position = new Vector2f();
                    var subTarget = new FloatRect(position, totalSize - position);
                    child.Layout(subTarget);
                }
            }
            return target;
        }

        public void SetPosition(IUIElement element, Vector2f position)
        {
            using (AcquireWriterLock())
            {
                if (Children.All(x => x != element))
                    throw new InvalidOperationException("The specified target element is not contained in this canvas");
                positions.AddOrUpdate(element, position);
            }
            OnLayoutPropertyChanged();
        }
    }
}