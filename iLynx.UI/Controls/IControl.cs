using OpenTK;

namespace iLynx.UI.Controls
{
    public interface IControl : IUIElement
    {
        float Width { get; }
        float Height { get; }
        Vector2d Position { get; }
    }
}
