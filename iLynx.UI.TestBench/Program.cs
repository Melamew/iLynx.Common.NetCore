//using iLynx.UI.OpenGL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iLynx.Common;
using iLynx.UI.SFML.Controls;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Window = iLynx.UI.Sfml.Window;

namespace iLynx.UI.TestBench
{
    public class Foo : BindingSource
    {
        private string a;

        public string A
        {
            get => a;
            set
            {
                if (value == a) return;
                var oldValue = a;
                a = value;
                OnPropertyChanged(oldValue, value);
            }
        }
    }

    public class Bar : BindingSource
    {
        private string b;

        public string B
        {
            get => b;
            set
            {
                if (value == b) return;
                var oldValue = b;
                b = value;
                OnPropertyChanged(oldValue, value);
            }
        }
    }

    public static class Program
    {
        private static Window window;

        static Program()
        {

        }

        static void Main(string[] args)
        {
            var t = new Thread(() =>
            {
                window = new Window(1280, 720, "Test", Styles.None)
                {
                    Background = Color.Transparent
                };
                window.Show();
            });
            t.Start();
            while (null == window) Thread.CurrentThread.Join(250);
            var dimensions = new Vector2f(64, 64);
            var clientRect = window.GetViewport(window.DefaultView);
            var viewportDimensions = new Vector2f(clientRect.Width, clientRect.Height);
            var centreButton = new Button
            {
                Dimensions = dimensions,
                Background = Color.Red,
                Position = viewportDimensions / 2 - dimensions / 2
            };
            var topButton = new Button
            {
                Dimensions = dimensions,
                Background = Color.Green,
                Position = new Vector2f(centreButton.Position.X, centreButton.Position.Y - dimensions.Y * 2)
            };
            var leftButton = new Button
            {
                Dimensions = dimensions,
                Background = Color.Blue,
                Position = new Vector2f(centreButton.Position.X - dimensions.X * 2, centreButton.Position.Y)
            };
            var bottomButton = new Button
            {
                Dimensions = dimensions,
                Background = Color.Cyan,
                Position = new Vector2f(centreButton.Position.X, centreButton.Position.Y + dimensions.Y * 2)
            };
            var rightButton = new Button
            {
                Dimensions = dimensions,
                Background = Color.Magenta,
                Position = new Vector2f(centreButton.Position.X + dimensions.X * 2, centreButton.Position.Y)
            };
            window.AddChildren(centreButton, topButton, leftButton, bottomButton, rightButton);
            window.MouseMoved += (sender, e) =>
            {
                centreButton.Position = new Vector2f(e.X - dimensions.X / 2, e.Y - dimensions.Y / 2);
                topButton.Position = new Vector2f(centreButton.Position.X, centreButton.Position.Y - dimensions.Y * 2);
                leftButton.Position = new Vector2f(centreButton.Position.X - dimensions.X * 2, centreButton.Position.Y);
                bottomButton.Position = new Vector2f(centreButton.Position.X, centreButton.Position.Y + dimensions.Y * 2);
                rightButton.Position = new Vector2f(centreButton.Position.X + dimensions.X * 2, centreButton.Position.Y);
            };
        }
    }
}
