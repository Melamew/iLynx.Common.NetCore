using System;
using SFML.Window;

namespace iLynx.UI.Sfml.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract class SfmlControlBase : UIElement, IControl
    {
        public event EventHandler<MouseButtonEventArgs> Clicked;
    }
}
