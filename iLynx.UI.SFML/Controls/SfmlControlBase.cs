using System;
using SFML.Window;

namespace iLynx.UI.Sfml.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract class SfmlControlBase : UIElement, IControl
    {
        protected SfmlControlBase(Alignment horizontalAlignment = Alignment.Start,
            Alignment verticalAlignment = Alignment.Start)
            : base(horizontalAlignment, verticalAlignment) { }
        public event EventHandler<MouseButtonEventArgs> Clicked;
    }
}
