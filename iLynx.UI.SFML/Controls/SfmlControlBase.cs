using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.Sfml.Controls
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

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            var dims = size;
            if (dims.X > target.Width || float.IsNaN(dims.X))
                dims.X = target.Width;
            if (dims.Y > target.Height || float.IsNaN(dims.Y))
                dims.Y = target.Height;
            var pos = target.Position();
            return new FloatRect(pos, dims);
        }

        public event EventHandler<MouseButtonEventArgs> Clicked;
    }
}
