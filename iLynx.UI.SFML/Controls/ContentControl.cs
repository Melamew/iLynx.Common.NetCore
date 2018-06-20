using System.ComponentModel;
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

        public ContentControl()
        {
            content.LayoutPropertyChanged += Content_LayoutPropertyChanged;
        }

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

        public string ContentString
        {
            get => (content as Label)?.Text;
            set
            {
                if (!(content is Label l))
                {
                    l = new Label(foreground) { Text = value };
                    Content = l;
                }
                else
                    l.Text = value;
            }
        }

        public IUIElement Content
        {
            get => content;
            set
            {
                if (value == content) return;
                var old = content;
                if (null != old)
                    old.LayoutPropertyChanged -= Content_LayoutPropertyChanged;
                content = value;
                content.LayoutPropertyChanged += Content_LayoutPropertyChanged;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        private void Content_LayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnLayoutPropertyChanged($"{nameof(Content)}.{e.PropertyName}");
        }
    }
}
