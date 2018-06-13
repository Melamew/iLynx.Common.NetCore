using System;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public class RectangleGeometry : Geometry
    {
        private float width, height;
        private Color color;

        public Color Color
        {
            get => color;
            set
            {
                if (value == color) return;
                color = value;
                GenerateVertices();
            }
        }

        public float Width
        {
            get => width;
            set
            {
                if (MathF.Abs(value - width) <= float.Epsilon) return;
                width = value;
                GenerateVertices();
            }
        }

        public float Height
        {
            get => height;
            set
            {
                if (MathF.Abs(value - height) <= float.Epsilon) return;
                height = value;
                GenerateVertices();
            }
        }

        private void GenerateVertices()
        {
            float w = width, h = height;
            ClearVertices();
            AddVertex(new Vector2f(0f, 0f), color);
            AddVertex(new Vector2f(0f, h), color);
            AddVertex(new Vector2f(w, 0f), color);
            AddVertex(new Vector2f(w, h), color);
        }

        public RectangleGeometry(float width, float height, Color color)
        {
            this.width = width;
            this.height = height;
            this.color = color;
            PrimitiveType = PrimitiveType.TriangleStrip;
            GenerateVertices();
        }

        public RectangleGeometry(Vector2f dimensions, Color color)
            : this(dimensions.X, dimensions.Y, color) { }
    }
}