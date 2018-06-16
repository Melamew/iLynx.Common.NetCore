using System;
using static System.MathF;
using System.Linq;
using iLynx.UI.Sfml;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.SFML.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract class SfmlControlBase : UIElement, IControl
    {
        private Vector2f size = new Vector2f(float.NaN, float.NaN);

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

        public virtual Vector2f ComputedPosition { get; private set; } = new Vector2f();

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            var dims = size;
            //var availableSize = target.Size();
            if (dims.X > target.Width || float.IsNaN(dims.X))
                dims.X = target.Width;
            if (dims.Y > target.Height || float.IsNaN(dims.Y))
                dims.Y = target.Height;
            var pos = target.Position();// + ((availableSize / 2f) - dims);
            ComputedPosition = pos;
            return new FloatRect(pos, dims);
        }

        public event EventHandler<MouseButtonEventArgs> Clicked;
    }
}
