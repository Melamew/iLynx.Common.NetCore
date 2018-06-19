using System;
using iLynx.UI.Sfml.Rendering;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.Sfml.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract class SfmlControlBase : UIElement, IControl
    {
        private Color background = Color.White;
        private Shape shape;
        private Func<Vector2f, Shape> backgroundGenerator = size => new RectangleShape(size);

        public Func<Vector2f, Shape> BackgroundGenerator
        {
            get => backgroundGenerator;
            set
            {
                if (value == backgroundGenerator) return;
                if (null == value) return;
                var old = backgroundGenerator;
                backgroundGenerator = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected SfmlControlBase(Alignment horizontalAlignment = Alignment.Start,
            Alignment verticalAlignment = Alignment.Start)
            : base(horizontalAlignment, verticalAlignment) { }

        public Color Background
        {
            get => background;
            set
            {
                if (value == background) return;
                var old = background;
                background = value;
                if (null != shape) shape.FillColor = value;
                OnPropertyChanged(old, value);
            }
        }

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            shape = backgroundGenerator(finalRect.Size());
            shape.FillColor = background;
            return finalRect;
        }

        protected virtual void OnClick(MouseButtonEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        public event EventHandler<MouseButtonEventArgs> Clicked;

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            if (null == shape) return;
            target.Draw(shape, states);
        }
    }
}
