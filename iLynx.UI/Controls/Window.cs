using System.Collections.Generic;
using System.Drawing;

namespace iLynx.UI.Controls
{
    public interface IWindow
    {
        ICollection<IUIElement> Children { get; }

        Color BackgroundColor { get; set; }
    }
}