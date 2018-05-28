using System.Drawing;
using System.Numerics;

namespace iLynx.UI.Shapes
{
    public class Line : IShape
    {
        public float Width => 0;
        public float Height => 0;
        public Vector2 Position { get; }
        public Color Background { get; }
        public Color Foreground { get; }
    }
}
