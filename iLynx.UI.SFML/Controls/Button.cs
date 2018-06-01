using System;
using iLynx.Common;
using SFML.Graphics;
using SFML.Window;

namespace iLynx.UI.SFML.Controls
{

    public class Button : SfmlControlBase// : IButton
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
        public Color Background { get; set; }
        public Color Foreground { get; set; }
        //public IControl Content { get; set; }
        public event EventHandler<MouseButtonEventArgs> Clicked;
        //public event EventHandler<MouseEventArgs> MouseOver;
    }
}
