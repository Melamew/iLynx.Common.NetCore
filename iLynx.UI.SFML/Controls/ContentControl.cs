using iLynx.UI.Sfml.Rendering;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Controls
{
    public class ContentControl : SfmlControlBase
    {
        private IUIElement content = new Label { Color = Color.Black };
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
            base.DrawInternal(target, states);
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
            finalRect = base.LayoutInternal(finalRect);
            content?.Layout(finalRect - padding);
            return finalRect;
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
