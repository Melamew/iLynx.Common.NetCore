using System.Drawing;
using System.Numerics;

namespace iLynx.UI
{
    // ReSharper disable once InconsistentNaming
    public interface IUIElement
    {
        float Width { get; }
        float Height { get; }
        Vector2 Position { get; }
        Color Background { get; }
        Color Foreground { get; }
    }
}
