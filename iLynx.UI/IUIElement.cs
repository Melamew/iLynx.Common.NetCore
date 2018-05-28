using System.Drawing;

namespace iLynx.UI
{
    // ReSharper disable once InconsistentNaming
    public interface IUIElement
    {
        float Width { get; }
        float Height { get; }
        Point Position { get; }
        Color Background { get; }
        Color Foreground { get; }
    }
}
