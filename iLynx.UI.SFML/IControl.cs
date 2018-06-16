using System;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public interface IControl : IUIElement
    {
        event EventHandler<MouseButtonEventArgs> Clicked;

    }
}