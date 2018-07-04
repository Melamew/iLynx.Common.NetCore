using SharpFont;

namespace iLynx.Graphics
{
    public class Font
    {
        private static readonly Library Library = new Library();
        private readonly Face face;
        private readonly Stroker stroker;
        private uint fontSize;
        private readonly string fontFamily;

        public Font(string file, uint fontSize = 24)
        {
            face = new Face(Library, file);
            stroker = new Stroker(Library);
            this.fontSize = fontSize;
            fontFamily = face.FamilyName;
        }

        private void Setup()
        {
            face.SelectCharmap(Encoding.Unicode);
            face.SetPixelSizes(0, fontSize);
        }

        public Glyph GetGlyph(char character)
        {
            var index = face.GetCharIndex(character);
            return new Glyph();
        }

        public uint FontSize
        {
            get => fontSize;
            set
            {
                if (value == fontSize) return;
                fontSize = value;
                Setup();
            }
        }
    }
}