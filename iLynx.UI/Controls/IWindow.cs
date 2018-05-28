using System.Collections.Generic;
using System.Drawing;

namespace iLynx.UI.Controls
{
    public interface IWindow : IControl
    {
        ICollection<IUIElement> Children { get; }
    }
}