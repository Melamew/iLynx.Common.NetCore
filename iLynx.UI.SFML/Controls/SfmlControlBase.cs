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

        protected override FloatRect ComputeBoundingBox(FloatRect destinationRect)
        {
            var dims = size;
            if (dims.X > destinationRect.Width || float.IsNaN(dims.X))
                dims.X = destinationRect.Width;
            if (dims.Y > destinationRect.Height || float.IsNaN(dims.Y))
                dims.Y = destinationRect.Height;
            return new FloatRect(destinationRect.Position(), dims);
        }

        public event EventHandler<MouseButtonEventArgs> Clicked;
    }
}
