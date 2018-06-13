using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public class Button : SfmlControlBase
    {
        private Vector2f dimensions;
        private Color background;
        private Vector2f position;
        private IUIElement content;
        private Geometry geometry;

        /// <summary>
        /// Called whenever a parameter / property that (should) affect the geometry is changed (eg. <see cref="Dimensions"/> or <see cref="Position"/>).
        /// </summary>
        /// <returns>The new geometry to use</returns>
        protected override void PrepareRender()
        {
            var dims = dimensions;
            var bg = Background;
            geometry = new RectangleGeometry(dims, bg);
        }

        /// <summary>
        /// Gets or sets the height of this control
        /// </summary>
        public Vector2f Dimensions
        {
            get => dimensions;
            set
            {
                if (value == dimensions) return;
                var old = dimensions;
                dimensions = value;
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
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        public Vector2f Position
        {
            get => position;
            set
            {
                if (value == position) return;
                var old = position;
                position = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected override Transform ComputeRenderTransform(FloatRect destinationRect)
        {
            var transform = Transform.Identity;
            transform.Translate(position);
            return transform;
        }

        protected override void DrawTransformed(RenderTarget target, RenderStates states)
        {
            if (null == geometry) return;
            target.Draw(geometry.Vertices, geometry.PrimitiveType, states);
            content?.Draw(target, states);
        }

        //public override void Draw(RenderTarget target, RenderStates states)
        //{
        //    base.Draw(target, states);

        //    var previousTransform = states.Transform;
        //    var newTransform = previousTransform;
        //    newTransform.Translate(position);
        //    states.Transform = newTransform;
        //    content?.Draw(target, states);
        //    states.Transform = previousTransform;
        //}

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
