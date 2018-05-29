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
}
