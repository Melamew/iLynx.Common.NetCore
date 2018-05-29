using System;
using System.Collections.Generic;
using System.Text;
using iLynx.UI.Controls;
using SFML.Graphics;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public class Window : RenderWindow, IWindow
    {
        internal Window(VideoMode mode, string title) : base(mode, title)
        {
        }

        internal Window(VideoMode mode, string title, Styles style) : base(mode, title, style)
        {
        }

        internal Window(VideoMode mode, string title, Styles style, ContextSettings settings) : base(mode, title, style, settings)
        {
        }

        internal Window(IntPtr handle) : base(handle)
        {
        }

        internal Window(IntPtr handle, ContextSettings settings) : base(handle, settings)
        {
        }

        public Window(uint width, uint height, string title)
         : this(new VideoMode(width, height), title)
        {

        }


        protected override bool PollEvent(out Event eventToFill)
        {
            var result = base.PollEvent(out eventToFill);
            if (result && EventType.Closed == eventToFill.Type)
                Close();
            return result;
        }

        public void Show()
        {
            while (IsOpen)
            {
                DispatchEvents();
                Display();
            }
        }
    }
}
