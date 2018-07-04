using SharpFont;

namespace iLynx.Graphics.Text
{
    public struct Glyph
    {

    }

    public class Font
    {
        private static readonly Library Library = new Library();
        private readonly Face face;
        private readonly Stroker stroker;

        public Font(string file, uint fontSize = 24)
        {
            face = new Face(Library, file);
            stroker = new Stroker(Library);
            face.SelectCharmap(Encoding.Unicode);
            face.SetPixelSizes(0, fontSize);
        }
    }

    public class DrawableText : IDrawable
    {
        private Font font;

        public DrawableText(string fontFile)
        {
            font = new Font(fontFile);
        }

        public void Draw(IDrawingContext target)
        {
            
        }
    }
}
