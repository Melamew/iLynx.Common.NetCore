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
        private static readonly Animator Animator = new Animator(startThread: true);
        private static AbsolutePositionPanel canvas;
        private static CanvasPositionBinding canvasPositionBinding;

        private static void Main()
        {
            StartWindow();
            var button = new Button { Content = new Label("This is moving", Color.Red), Background = Color.Green };
            canvas.AddChild(button);
            canvasPositionBinding = new CanvasPositionBinding(canvas, button);
            var start = new Vector2f(0f, canvas.ComputedSize.Y / 2f - button.ComputedSize.Y / 2);
            var end = new Vector2f(canvas.ComputedSize.X - button.ComputedSize.X, start.Y);
            Animator.Start(new LinearAnimation<Vector2f>(start, end, TimeSpan.FromSeconds(.5), canvasPositionBinding, LoopMode.Reverse));
            window.Closed += (o, e) => Animator.StopAnimator();
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
                Background = Color.Yellow
            };
            window.RootPanel = root;
            var stackPanel = new StackPanel { Size = (Vector2f)window.Size * 0.5f, Background = Color.White, Margin = 4f };
            stackPanel.AddChild(new Label("Label 1", Color.Red)
            {
                Margin = 64f
            }, new Label("Label 2", Color.Green) { Margin = 4f });
            canvas = new AbsolutePositionPanel
            {
                Background = Color.Black,
                Margin = 4f
            };
            root.AddChild(stackPanel, canvas);
        }
    }

    public class CanvasPositionBinding : IBinding<Vector2f>
    {
        private readonly AbsolutePositionPanel canvas;
        private readonly IUIElement element;

        public CanvasPositionBinding(AbsolutePositionPanel canvas, IUIElement element)
        {
            this.canvas = canvas;
            this.element = element;
        }

        public void SetValue(Vector2f value)
        {
            canvas.SetRelativePosition(element, value);
        }

        public Vector2f GetValue()
        {
            return canvas.GetRelativePosition(element);
        }
    }


    //public class VectorAnimation : IAnimation
    //{
    //    public VectorAnimation(Vector2f start, Vector2f end, TimeSpan duration, LoopMode loopMode = LoopMode.None)
    //    {

    //    }

    //    public void Tick(TimeSpan elapsed)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Start()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool IsFinished { get; }
    //}
}