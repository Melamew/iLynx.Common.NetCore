using System.Drawing;

namespace iLynx.UI.Controls
{
    public interface IControl : IUIElement
    {
        float Width { get; }
        float Height { get; }
        Point Position { get; }
        Color Background { get; }
        Color Foreground { get; }
    }
}
