﻿using iLynx.UI.Sfml;
using SFML.Graphics;

namespace iLynx.UI.SFML.Controls
{
    public class Label : UIElement
    {
        private string text;
        private Text renderable = new Text();
        private uint fontSize = 24;
        private Color color;

        public Label()
            : this(string.Empty, Color.Black)
        {
        }

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
                RebuildRender();
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

        protected override void DrawLocked(RenderTarget target, RenderStates states)
        {
            renderable.Draw(target, states);
        }

        private void RebuildRender()
        {
            renderable = new Text(text, DefaultFont, fontSize) { FillColor = color };
        }

        public override string ToString()
        {
            return $"Label: {text}";
        }

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            using (AcquireLock())
            {
                RebuildRender();
                renderable.Position = target.Position();
            }
            return renderable.GetGlobalBounds();
        }
    }
}