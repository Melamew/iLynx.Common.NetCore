using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Controls
{
    public class Label : UIElement
    {
        private readonly Text renderable = new Text(string.Empty, DefaultFont, 24);

        public Label()
            : this(string.Empty, Color.Black) { }

        public Label(Color color)
            : this(string.Empty, color) { }

        public Label(string text, Color color)
        {
            Text = text ?? string.Empty;
            Color = color;
        }

        public Label(string text, Font font, uint fontSize)
            : this(text, Color.Black)
        {
            Text = text ?? string.Empty;
            Font = font;
            FontSize = fontSize;
        }

        public string Text
        {
            get => renderable.DisplayedString;
            set
            {
                if (value == renderable.DisplayedString) return;
                var old = renderable.DisplayedString;
                renderable.DisplayedString = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Color Color
        {
            get => renderable.FillColor;
            set
            {
                if (value == renderable.FillColor) return;
                var old = renderable.FillColor;
                renderable.FillColor = value;
                OnPropertyChanged(old, value);
            }
        }

        public uint FontSize
        {
            get => renderable.CharacterSize;
            set
            {
                if (value == renderable.CharacterSize) return;
                var old = renderable.CharacterSize;
                renderable.CharacterSize = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Font Font
        {
            get => renderable.Font;
            set
            {
                if (value == renderable.Font) return;
                var old = renderable.Font;
                renderable.Font = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected override void DrawInternal(RenderTarget target, RenderStates states)
        {
            states.Transform.Translate(RenderPosition);
            renderable.Draw(target, states);
        }

        public override string ToString()
        {
            return $"Label: {renderable.DisplayedString}";
        }

        public override Vector2f Measure(Vector2f availableSpace)
        {
            var localBounds = renderable.GetLocalBounds();
            return new Vector2f(localBounds.Width + localBounds.Left,
                renderable.CharacterSize);
        }

        protected override FloatRect LayoutInternal(FloatRect finalRect)
        {
            var localBounds = renderable.GetLocalBounds();
            return new FloatRect(
                finalRect.Left - localBounds.Left,
                finalRect.Top - localBounds.Top,
                localBounds.Width + localBounds.Left,
                renderable.CharacterSize);
        }
    }
}