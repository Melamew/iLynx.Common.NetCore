using OpenTK;

namespace iLynx.Graphics
{
    public class Text : IDrawable
    {
        private Font font;

        public Text(string fontFile)
        {
            font = new Font(fontFile);
        }

        public Color FillColor { get; set; }

        public void Draw(IDrawingContext target)
        {
            
        }

        public Vector2 FindCharacterPos(uint index)
        {
            return new Vector2(0);
        }
    }
}