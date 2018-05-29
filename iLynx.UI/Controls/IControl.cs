using System.Drawing;

namespace iLynx.UI.Controls
{
    public interface IControl : IUIElement
    {
        float Width { get; set; }
        float Height { get; set; }
        Point Position { get; set; }
        Color Background { get; set; }
        Color Foreground { get; set; }
    }
}
