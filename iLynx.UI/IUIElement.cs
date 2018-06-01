using System.Drawing;

namespace iLynx.UI
{
    // ReSharper disable once InconsistentNaming
    public interface IUIElement
    {
        uint Width { get; set; }
        uint Height { get; set; }
        Point Position { get; set; }
    }

    public interface IVertex<TVector, TColor>
    {
        TVector Position { get; set; }
        TVector TextureCoordinates { get; set; }
        TColor VertexColor { get; set; }
    }
}
