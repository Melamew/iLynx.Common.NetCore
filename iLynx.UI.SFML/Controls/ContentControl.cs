using iLynx.UI.Sfml.Rendering;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Controls
{
    public class ContentControl : SfmlControlBase
    {
        private Color background = Color.White;
        private IUIElement content = new Label { Color = Color.Black };
        private Geometry geometry;
        private Color foreground;
        private Thickness padding = 4f;

        public Thickness Padding
        {
            get => padding;
            set
            {
                if (value == padding) return;
                var old = padding;
                padding = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

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

        public Color Foreground
        {
            get => (content as Label)?.Color ?? foreground;
            set
            {
                var old = foreground;
                if (value == old) return;
                foreground = value;
                if (content is Label label)
                    label.Color = foreground;
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
            return content?.Measure(dims - padding) + padding ?? dims;
        }

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            var available = finalRect;
            content?.Layout(available - padding);
            geometry = new RectangleGeometry(available.Width, available.Height, background);
            return available;
        }

        public object Content
        {
            get => content;
            set
            {
                var changed = false;
                object old = null;
                switch (value)
                {
                    case string s when content is Label l:
                        if (s == l.Text) return;
                        old = l.Text;
                        l.Text = s;
                        changed = true;
                        break;
                    case string s:
                        var label = new Label(s, foreground);
                        old = content;
                        content = label;
                        changed = true;
                        break;
                    case IUIElement e:
                        old = content;
                        content = e;
                        changed = true;
                        break;
                }

                if (!changed) return;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }
    }
}
