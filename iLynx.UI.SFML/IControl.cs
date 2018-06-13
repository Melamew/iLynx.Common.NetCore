using System;
using SFML.Window;

namespace iLynx.UI.SFML
{
    public interface IControl : IUIElement
    {
        event EventHandler<MouseButtonEventArgs> Clicked;

    }
}