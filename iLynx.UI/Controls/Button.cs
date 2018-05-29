using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace iLynx.UI.Controls
{
    public abstract class ControlBase : IControl
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public Point Position { get; set; }
        public Color Background { get; set; }
        public Color Foreground { get; set; }
    }

    public abstract class ContentControlBase : ControlBase, IContentControl
    {
        public IControl Content { get; set; }
    }

    public abstract class Button : ContentControlBase
    {
        
    }
}
