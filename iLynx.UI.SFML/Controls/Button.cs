using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using iLynx.UI.Controls;

namespace iLynx.UI.SFML.Controls
{
    public class Button : IButton
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
        public Point Position { get; set; }
        public Color Background { get; set; }
        public Color Foreground { get; set; }
        public IControl Content { get; set; }
        public event EventHandler<MouseButtonEventArgs> Clicked;
        public event EventHandler<MouseEventArgs> MouseOver;
    }
}
