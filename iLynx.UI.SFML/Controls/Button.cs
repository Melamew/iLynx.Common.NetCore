using System.Runtime.CompilerServices;
using iLynx.UI.Sfml;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
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
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected override void DrawLocked(RenderTarget target, RenderStates states)
        {
            states.Transform.Translate(ComputedPosition);
            target.Draw(geometry, states);
            content?.Draw(target, states);
        }

        //protected override void DrawTransformed(RenderTarget target, RenderStates states)
        //{
        //    if (null == geometry) return;
        //    target.Draw(geometry, states);
        //    content?.Draw(target, states);
        //}

        protected override FloatRect LayoutInternal(FloatRect target)
        {
            var available = base.LayoutInternal(target);
            var contentSize = content.Layout(available);
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
