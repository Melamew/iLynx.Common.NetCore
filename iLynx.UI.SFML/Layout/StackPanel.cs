using System.Linq;
using iLynx.UI.Sfml.Controls;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Layout
{
    public class StackPanel : Panel
    {
        private bool reverse;
        private Orientation orientation = Orientation.Vertical;

        public bool Reverse
        {
            get => reverse;
            set
            {
                if (value == reverse) return;
                var old = reverse;
                reverse = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Orientation Orientation
        {
            get => orientation;
            set
            {
                if (value == orientation) return;
                var old = orientation;
                orientation = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            var availableSpace = base.LayoutInternal(finalRect);
            var scalar = orientation == Orientation.Horizontal ? new Vector2f(0f, 1f) : new Vector2f(1f, 0f);
            var childSpaceScalar = orientation == Orientation.Horizontal ? new Vector2f(1f, 0f) : new Vector2f(0f, 1f); // The inverse for stepping size
            var usedSpace = new FloatRect(availableSpace.Position(), availableSpace.Size().Scale(scalar));
            foreach (var child in reverse ? Children.Reverse() : Children)
            {
                var childSpace = child.Layout(availableSpace).Size().Scale(childSpaceScalar);
                usedSpace.Height += childSpace.Y;
                usedSpace.Width += childSpace.X;
                availableSpace.Left += childSpace.X;
                availableSpace.Width -= childSpace.X;
                availableSpace.Top += childSpace.Y;
                availableSpace.Height -= childSpace.Y;
            }
            return usedSpace;
        }
    }
}
