using System;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public static class InputHandler
    {
        public static event EventHandler<MouseMoveEventArgs> MouseMove;

        public static void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            MouseMove?.Invoke(sender, e);
        }

        public static event EventHandler<MouseButtonEventArgs> MouseDown;

        public static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown?.Invoke(sender, e);
        }

        public static event EventHandler<MouseButtonEventArgs> MouseUp;

        public static void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUp?.Invoke(sender, e);
        }

        public static event EventHandler<KeyEventArgs> KeyDown;

        public static void OnKeyDown(object sender, KeyEventArgs e)
        {
            KeyDown?.Invoke(sender, e);
        }

        public static event EventHandler<KeyEventArgs> KeyUp;

        public static void OnKeyUp(object sender, KeyEventArgs e)
        {
            KeyUp?.Invoke(sender, e);
        }

        public static event EventHandler<TextEventArgs> TextEntered;

        public static void OnTextEntered(object sender, TextEventArgs e)
        {
            TextEntered?.Invoke(sender, e);
        }
    }
}