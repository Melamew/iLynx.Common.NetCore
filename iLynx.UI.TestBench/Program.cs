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
        private static ContentControl stretchedControl;

        private static void Main()
        {
            StartWindow();
            canvas.AddChild(BoundLabel);
            var foo = new Foo();
            textBinding = new MultiBinding<string>().Bind(foo, nameof(Foo.A)).Bind(BoundLabel, nameof(Label.Text));
            InputHandler.TextEntered += (s, e) =>
            {
                if (e.Unicode == "\b" && foo.A.Length > 0)
                    foo.A = foo.A.Remove(foo.A.Length - 1);
                else if (!char.IsControl(e.Unicode, 0))
                    foo.A += e.Unicode;
            };
            Animator.AddAnimation(new CallbackAnimation(p =>
            {
                var offset = new Vector2f(0f, 50f);
                var target = new Vector2f(canvas.RenderSize.X - BoundLabel.RenderSize.X, 0f);
                canvas.SetRelativePosition(BoundLabel, offset + (float)p * target);
            }, TimeSpan.FromSeconds(5d), LoopMode.Reverse, EasingFunctions.CubicInOut));
            var endMargin = stretchedControl.RenderSize.X / 2f - 120f;
            Animator.AddAnimation(new CallbackAnimation(p =>
                {
                    stretchedControl.Margin = new Thickness((float)p * endMargin, 4f);
                }, TimeSpan.FromSeconds(5d), LoopMode.Reverse));
        }

        private static void StartWindow()
        {
            var t = new Thread(() =>
            {
                window = new Window(new VideoMode(1920, 1080), "Test");
                window.Show();
            });
            t.Start();
            while (null == window) Thread.CurrentThread.Join(250);
            var root = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Background = ColorUtils.FromRgbA(.1f, .1f, .1f, 1f)
            };
            window.RootPanel = root;
            var stackPanel = new StackPanel
            {
                Size = (Vector2f)window.Size * 0.5f,
                Background = ColorUtils.FromRgbA(.2f, .2f, .2f, 1f),
                Margin = 16f
            };
            var labelMargins = new Thickness(8f);
            stackPanel.AddChild(new ContentControl
                {
                ContentString = "Left Aligned",
                Foreground = Color.Green,
                Background = Color.Black,
                Margin = labelMargins
            },
                new ContentControl
                {
                    Foreground = Color.Green,
                    Background = Color.Black,
                    Margin = labelMargins,
                    HorizontalAlignment = Alignment.End,
                    ContentString = "Right Aligned"
                },
                new ContentControl
                {
                    ContentString = "Centered",
                    Margin = labelMargins,
                    HorizontalAlignment = Alignment.Center,
                    Foreground = Color.Green,
                    Background = Color.Black
                },
                stretchedControl = new ContentControl
                {
                    ContentString = "Stretched",
                    Margin = labelMargins,
                    HorizontalAlignment = Alignment.Stretch,
                    Foreground = Color.Green,
                    Background = Color.Black
                });
            canvas = new Canvas
            {
                Background = ColorUtils.FromRgbA(.3f, .3f, .3f, 1f),
                Margin = 16f
            };
            root.AddChild(stackPanel, canvas);
        }
    }
}