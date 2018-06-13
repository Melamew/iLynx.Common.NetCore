using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public class Button : SfmlControlBase// : IButton
    {
        //private float width;
        //private float height;
        private Vector2f dimensions;
        private Color background;
        private volatile bool isDirty = true;
        private Geometry geometry;
        private Vector2f position;

        ///// <summary>
        ///// Gets or sets the width of this control
        ///// </summary>
        //public float Width
        //{
        //    get => width;
        //    set
        //    {
        //        if (Abs(width - value) <= float.Epsilon) return;
        //        var old = width;
        //        width = value;
        //        isDirty = true;
        //        OnPropertyChanged(old, value);
        //    }
        //}

        /// <summary>
        /// Called whenever a parameter / property that (should) affect the geometry is changed (eg. <see cref="Dimensions"/> or <see cref="Position"/>).
        /// </summary>
        /// <returns>The new geometry to use</returns>
        protected virtual Geometry GenerateGeometry()
        {
            var pos = Position;
            var dims = dimensions;
            var bg = Background;
            return new RectangleGeometry(pos, pos + dims, bg);
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
                isDirty = true;
                OnPropertyChanged(old, value);
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
                isDirty = true;
                OnPropertyChanged(old, value);
            }
        }

        public Color Foreground { get; set; }

        public Vector2f Position
        {
            get => position;
            set
            {
                if (value == position) return;
                var old = position;
                position = value;
                isDirty = true;
                OnPropertyChanged(old, value);
            }
        }

        protected override Geometry Geometry
        {
            get
            {
                if (!isDirty) return geometry;
                try
                {
                    return geometry = GenerateGeometry();
                }
                finally { isDirty = false; }
            }
        }
    }
}
