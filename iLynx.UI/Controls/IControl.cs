using System.Drawing;

namespace iLynx.UI.Controls
{
    public interface IControl : IUIElement
    {
        uint Width { get; set; }
        uint Height { get; set; }
        Point Position { get; set; }
        Color Background { get; set; }
        Color Foreground { get; set; }
    }
}
