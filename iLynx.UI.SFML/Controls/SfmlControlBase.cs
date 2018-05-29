using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using iLynx.UI.Controls;

namespace iLynx.UI.SFML.Controls
{
    public abstract class SfmlControlBase : IControl
    {
        public abstract uint Width { get; set; }
        public abstract uint Height { get; set; }
        public abstract Point Position { get; set; }
        public abstract Color Background { get; set; }
        public abstract Color Foreground { get; set; }
    }
}
