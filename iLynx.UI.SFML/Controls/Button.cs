using iLynx.UI.Sfml.Rendering;
using SFML.Graphics;

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
                geometry = new RectangleGeometry(ComputedSize.X, ComputedSize.Y, background);
                OnPropertyChanged(old, value);
            }
        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            if (null == geometry) return;
            states.Transform.Translate(ComputedPosition);
            target.Draw(geometry, states);
            content?.Draw(target, RenderStates.Default); //, states);
        }

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            var available = target;
            var contentSize = content.Layout(available);
            available.Height = contentSize.Height;
            available.Width = contentSize.Width;
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
