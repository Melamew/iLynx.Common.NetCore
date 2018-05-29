using System.Drawing;

namespace iLynx.UI.Controls
{
    public interface IControl : IUIElement
    {
        Color Background { get; set; }
        Color Foreground { get; set; }
    }
}
