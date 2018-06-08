using System;

namespace iLynx.UI.SFML
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2
    }
    // ReSharper disable once InconsistentNaming
    public interface IUIElement
    {
        ///// <summary>
        ///// Gets or Sets the parent of this element
        ///// </summary>
        //IUIElement Parent { get; set; }

        ///// <summary>
        ///// Raised when the mouse is moved inside the bounding box / hitbox of this element
        ///// </summary>
        //event EventHandler<MouseEventArgs> MouseMove;

        ///// <summary>
        ///// Raised when a mouse button is pressed inside this element
        ///// </summary>
        //event EventHandler<MouseButtonEventArgs> MouseDown;

        ///// <summary>
        ///// Raised when this element is clicked
        ///// </summary>
        //event EventHandler<MouseButtonEventArgs> MouseClick;

        ///// <summary>
        ///// Raised when a mouse button is released inside this element
        ///// </summary>
        //event EventHandler<MouseButtonEventArgs> MouseUp;
    }

    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// The global X coordinate of the mouse
        /// </summary>
        public int X { get; }

        /// <summary>
        /// The global Y coordinate of the mouse
        /// </summary>
        public int Y { get; }
    }

    public class MouseButtonEventArgs : MouseEventArgs
    {
        public MouseButtonEventArgs(MouseButton button, int x, int y) : base(x, y)
        {
            Button = button;
        }

        public MouseButton Button { get; }
    }


}
