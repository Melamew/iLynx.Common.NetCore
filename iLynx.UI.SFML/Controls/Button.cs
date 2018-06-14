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

        //protected override void OnRenderPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    base.OnRenderPropertyChanged(propertyName);
        //    var dims = content?.BoundingBox + Margin;
        //    var bg = Background;
        //    geometry = new RectangleGeometry(dims?.Size() ?? new Vector2f(), bg);
        //}

        public Color Background
        {
            get => background;
            set
            {
                if (value == background) return;
                var old = background;
                background = value;
                OnPropertyChanged(old, value);
                OnRenderPropertyChanged();
            }
        }

        protected override void DrawTransformed(RenderTarget target, RenderStates states)
        {
            if (null == geometry) return;
            target.Draw(geometry, states);
            content?.Draw(target, states);
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
