//using iLynx.UI.OpenGL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iLynx.Common;
using iLynx.UI.SFML;
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
                window = new Window(1280, 720, "Test", Styles.None) { Background = new Color(32, 32, 32, 128) };
                window.Show();
            });
            t.Start();
            while (null == window) Thread.CurrentThread.Join(250);
            var dimensions = new Vector2f(64, 64);
            var centreButton = new Button
            {
                Size = dimensions,
                Background = Color.Red,
            };
            var topButton = new Button
            {
                Size = dimensions,
                Background = Color.Green,
            };
            var leftButton = new Button
            {
                Size = dimensions,
                Background = Color.Blue,
            };
            var bottomButton = new Button
            {
                Size = dimensions,
                Background = Color.Cyan,
            };
            var rightButton = new Button
            {
                Size = dimensions,
                Background = Color.Magenta,
            };
            var root = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = 4f,
                Background = Color.Yellow
            };
            window.RootPanel = root;
            var stackPanel = new StackPanel { Size = (Vector2f)window.Size * 0.5f, Background = Color.White };
            stackPanel.AddChild(new Label("Label 1", Color.Red) { Margin = new Thickness(64f) }, new Label("Label 2", Color.Green) { Margin = new Thickness(4f) });
            var canvas = new AbsolutePositionPanel { Size = (Vector2f)window.Size * 0.5f, Background = Color.Black };
            canvas.AddChild(
                centreButton,
                topButton,
                leftButton,
                bottomButton,
                rightButton
                );
            root.AddChild(stackPanel, canvas);
            window.MouseMoved += (sender, e) =>
            {
                var centrePos = new Vector2f(e.X - dimensions.X / 2, e.Y - dimensions.Y / 2);
                canvas.SetPosition(centreButton, centrePos);
                canvas.SetPosition(topButton, new Vector2f(centrePos.X, centrePos.Y - dimensions.Y * 2));
                canvas.SetPosition(leftButton, new Vector2f(centrePos.X - dimensions.X * 2, centrePos.Y));
                canvas.SetPosition(bottomButton, new Vector2f(centrePos.X, centrePos.Y + dimensions.Y * 2));
                canvas.SetPosition(rightButton, new Vector2f(centrePos.X + dimensions.X * 2, centrePos.Y));
            };
        }
    }
}
