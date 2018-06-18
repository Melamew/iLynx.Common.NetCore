//using iLynx.UI.OpenGL;

using System;
using System.Threading;
using iLynx.Common;
using iLynx.UI.Sfml;
using iLynx.UI.Sfml.Animation;
using iLynx.UI.Sfml.Controls;
using iLynx.UI.Sfml.Layout;
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
        private static Canvas canvas;
        private static readonly Label BoundLabel = new Label(Color.Green);
        private static IBinding<string> textBinding;

        private static void Main()
        {
            Console.WriteLine(0 % 2);
            Console.WriteLine(1 % 2);
            Console.WriteLine(2 % 2);
            Console.WriteLine(3 % 2);
            Console.WriteLine(4 % 2);
            StartWindow();
            var button = new Button
            {
                Content = new Label("This is moving", Color.Red),
                Background = Color.Green
            };
            canvas.AddChild(button);
            canvas.AddChild(BoundLabel);
            var start = new Vector2f(0f, canvas.RenderSize.Y / 2f - button.RenderSize.Y / 2);
            var end = new Vector2f(canvas.RenderSize.X - button.RenderSize.X, start.Y);
            Animator.AddAnimation(new CallbackAnimation(
                p => canvas.SetRelativePosition(button, start + (end - start) * (float) p), TimeSpan.FromSeconds(2f),
                LoopMode.Reverse));
            var foo = new Foo();
            textBinding = new MultiBinding<string>().Bind(foo, nameof(Foo.A)).Bind(BoundLabel, nameof(Label.Text));
            InputHandler.TextEntered += (s, e) =>
            {
                if (e.Unicode == "\b")
                    foo.A = foo.A.Remove(foo.A.Length - 1);
                else if (!char.IsControl(e.Unicode, 0))
                    foo.A += e.Unicode;
            };
            Animator.AddAnimation(new CallbackAnimation(p =>
            {
                var offset = new Vector2f(0f, 50f);
                var target = new Vector2f(canvas.RenderSize.X - BoundLabel.RenderSize.X, 0f);
                canvas.SetRelativePosition(BoundLabel, offset + (float)p * target);
            }, TimeSpan.FromMilliseconds(2000d), LoopMode.Reverse, EasingFunctions.QuadraticInOut));
        }

        private static void StartWindow()
        {
            var t = new Thread(() =>
            {
                window = new Window(1280, 720, "Test", Styles.None) { Background = new Color(32, 32, 32, 128) };
                window.Show();
            });
            t.Start();
            while (null == window) Thread.CurrentThread.Join(250);
            var root = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Background = Color.Blue
            };
            window.RootPanel = root;
            var stackPanel = new StackPanel
            {
                Size = (Vector2f) window.Size * 0.5f,
                Background = Color.Black,
                Margin = 4f
            };
            stackPanel.AddChild(new Label("Label 1", Color.Red)
            {
                Margin = 64f
            },
                new Label("Label 2", Color.Green) {
                Margin = 4f,
                HorizontalAlignment = Alignment.End
                });
            canvas = new Canvas
            {
                Background = Color.Black,
                Margin = 4f
            };
            root.AddChild(stackPanel, canvas);
        }
    }
}