using iLynx.UI.Sfml.Rendering;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Controls
{
    public class Button : SfmlControlBase
    {
        private Color background;
        private IUIElement content = new Label();
        private Geometry geometry;

        public Color Background
        {
            get => background;
            set
            {
                if (value == background) return;
                var old = background;
                background = value;
                geometry = new RectangleGeometry(RenderSize.X, RenderSize.Y, background);
                OnPropertyChanged(old, value);
            }
        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            if (null == geometry) return;
            states.Transform.Translate(RenderPosition);
            target.Draw(geometry, states);
            content?.Draw(target, RenderStates.Default); //, states);
        }

        public override Vector2f Measure(Vector2f availableSpace)
        {
            var dims = base.Measure(availableSpace);
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (0 == dims.X)
                dims.X = availableSpace.X;
            if (0 == dims.Y)
                dims.Y = availableSpace.Y;
            // ReSharper restore CompareOfFloatsByEqualityOperator
            return content?.Measure(dims) ?? dims;
        }

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            var available = finalRect;
            content?.Layout(available);
            geometry = new RectangleGeometry(available.Width, available.Height, background);
            return available;
        }

        public IUIElement Content
        {
            get => content;
            set
            {
                if (value == content) return;
                var old = content;
                content = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }
    }
}
