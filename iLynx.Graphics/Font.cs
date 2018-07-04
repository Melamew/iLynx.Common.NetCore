using SharpFont;

namespace iLynx.Graphics
{
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
}