using iLynx.Graphics.Shaders;
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

        public void Draw(IView target)
        {
            
        }

        public Vector2 FindCharacterPos(uint index)
        {
            return new Vector2(0);
        }

        public DrawCall<Vertex> CreateDrawCall()
        {
            throw new System.NotImplementedException();
        }

        public Shader Shader { get; }
        public Texture Texture { get; }
    }
}