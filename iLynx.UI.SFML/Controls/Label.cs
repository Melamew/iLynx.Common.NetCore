using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public class Label : UIElement
    {
        private string text;
        private Text renderable = new Text();
        private uint fontSize = 24;
        private Color color;

        public Label(string text, Color color)
        {
            this.text = text ?? string.Empty;
            this.color = color;
        }

        public string Text
        {
            get => text;
            set
            {
                if (value == text) return;
                var old = text;
                text = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Color Color
        {
            get => color;
            set
            {
                if (value == color) return;
                var old = color;
                color = value;
                OnPropertyChanged(old, color);
                OnRenderPropertyChanged();
            }
        }

        public uint FontSize
        {
            get => fontSize;
            set
            {
                if (value == fontSize) return;
                var old = fontSize;
                fontSize = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected override void DrawTransformed(RenderTarget target, RenderStates states)
        {
            renderable.Draw(target, states);
        }

        protected override FloatRect ComputeBoundingBox(FloatRect destinationRect)
        {
            return RenderTransform.TransformRect(renderable.GetLocalBounds()) + Margin;
        }

        protected override void PrepareRender()
        {
            renderable = new Text(text, DefaultFont, fontSize) { FillColor = Color };
        }
    }
}