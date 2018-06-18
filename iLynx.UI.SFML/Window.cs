using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using iLynx.Common;
using iLynx.UI.Sfml.Controls;
using iLynx.UI.Sfml.Layout;
using SFML.Graphics;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public class Window : RenderWindow, IBindingSource
    {
        private static readonly Dictionary<EventType, EventMapper> EventMap = new Dictionary<EventType, EventMapper>
        {
            {
                EventType.MouseMoved,
                (w, e) => InputHandler.OnMouseMoved(w, new MouseMoveEventArgs(e.MouseMove))
            },
            {
                EventType.MouseButtonPressed,
                (w, e) => InputHandler.OnMouseDown(w, new MouseButtonEventArgs(e.MouseButton))
            },
            {
                EventType.MouseButtonReleased,
                (w, e) => InputHandler.OnMouseUp(w, new MouseButtonEventArgs(e.MouseButton))
            },
            {
                EventType.KeyPressed,
                (w, e) => InputHandler.OnKeyDown(w, new KeyEventArgs(e.Key))
            },
            {
                EventType.KeyReleased,
                (w, e) => InputHandler.OnKeyUp(w, new KeyEventArgs(e.Key))
            },
            {
                EventType.Closed,
                (w, e) => w.Close()
            },
            {
                EventType.TextEntered,
                (w, e) => InputHandler.OnTextEntered(w, new TextEventArgs(e.Text))
            }
        };

        private readonly DetachedBindingSource bindingSource = new DetachedBindingSource();
        private Color background = Color.Black;
        private Panel rootPanel;
        private readonly StatisticsElement stats = new StatisticsElement();
        private TimeSpan frameTime;
        private readonly IBinding<TimeSpan> frameTimeBinding;
        private TimeSpan layoutTime;
        private readonly IBinding<TimeSpan> layoutTimeBinding;

        public Window(uint width, uint height, string title, Styles style = Styles.Default)
            : base(new VideoMode(width, height, 32), title, style)
        {
            SetupAlpha();
            base.SetFramerateLimit(120);
            rootPanel = new Canvas { Background = background };
            frameTimeBinding = new MultiBinding<TimeSpan>().Bind(this, nameof(FrameTime))
                .Bind(stats, nameof(StatisticsElement.FrameTime));
            layoutTimeBinding = new MultiBinding<TimeSpan>().Bind(this, nameof(LayoutTime))
                .Bind(stats, nameof(StatisticsElement.LayoutTime));
        }

        ~Window()
        {
            frameTimeBinding.Unbind(this).Unbind(stats);
        }

        public TimeSpan LayoutTime
        {
            get => layoutTime;
            set
            {
                if (value == layoutTime) return;
                var old = layoutTime;
                layoutTime = value;
                bindingSource.RaisePropertyChanged(old, value);
            }
        }

        public Panel RootPanel
        {
            get => rootPanel;
            set
            {
                if (value == rootPanel) return;
                var old = rootPanel;
                old.LayoutPropertyChanged -= OnLayoutChanged;
                rootPanel = value;
                rootPanel.LayoutPropertyChanged += OnLayoutChanged;
                bindingSource.RaisePropertyChanged(old, value);
                Layout();
            }
        }

        public Color Background
        {
            get => background;
            set
            {
                if (value == background) return;
                var old = background;
                background = value;
                bindingSource.RaisePropertyChanged(old, value);
            }
        }

        public void AddPropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler)
        {
            bindingSource.AddPropertyChangedHandler(valueName, handler);
        }

        public void RemovePropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler)
        {
            bindingSource.RemovePropertyChangedHandler(valueName, handler);
        }

        protected override bool PollEvent(out Event eventToFill)
        {
            if (base.PollEvent(out eventToFill) && EventMap.TryGetValue(eventToFill.Type, out var mapper))
            {
                var e = eventToFill;
                Task.Run(() => mapper?.Invoke(this, e));
                return true;
            }

            return false;
        }

        //public IEnumerable<IUIElement> Children => children;

        public void Show()
        {
            var sw = new Stopwatch();
            while (IsOpen)
            {
                sw.Start();
                DispatchEvents();
                Clear(background);
                Draw(rootPanel);
                sw.Stop();
                FrameTime = sw.Elapsed;
                Draw(stats);
                Display();
            }
        }

        public TimeSpan FrameTime
        {
            get => frameTime;
            set
            {
                if (value == frameTime) return;
                var old = frameTime;
                frameTime = value;
                bindingSource.RaisePropertyChanged(old, value);
            }
        }

        private void OnLayoutChanged(object sender, PropertyChangedEventArgs e)
        {
            var sw = Stopwatch.StartNew();
            Layout();
            sw.Stop();
            LayoutTime = sw.Elapsed;
            stats.Layout(new FloatRect(0f, 0f, Size.X, Size.Y));
        }

        protected virtual void Layout()
        {
            rootPanel.Layout(new FloatRect(0f, 0f, Size.X, Size.Y));
        }

        private void SetupAlpha()
        {
            // Such a hack...
            // TODO: Support other platforms?
            var handle = SystemHandle;
            var margins = new MARGINS { leftWidth = -1 };
            DwmExtendFrameIntoClientArea(handle, ref margins);
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        private delegate void EventMapper(Window source, Event e);

        [StructLayout(LayoutKind.Sequential)]
        // ReSharper disable once InconsistentNaming
        private struct MARGINS
        {
            // ReSharper disable FieldCanBeMadeReadOnly.Local
            // ReSharper disable MemberCanBePrivate.Local
            public int leftWidth;
            public int rightWidth;
            public int topHeight;

            public int bottomHeight;
            // ReSharper restore FieldCanBeMadeReadOnly.Local
            // ReSharper restore MemberCanBePrivate.Local
        }
    }

    public class StatisticsElement : ContentControl
    {
        private TimeSpan frameTime;
        private TimeSpan animationFrameTime;
        private TimeSpan layoutTime;
        readonly StringBuilder builder = new StringBuilder();

        private void GenContent()
        {
            builder.Clear();
            builder.AppendLine($"FrameTime: {frameTime.TotalMilliseconds:f2} ms");
            builder.AppendLine($"Animation FrameTime: {animationFrameTime.TotalMilliseconds:f2} ms");
            builder.AppendLine($"Layout Time: {layoutTime.TotalMilliseconds:f2} ms");
            Content = builder.ToString();
        }

        public TimeSpan FrameTime
        {
            get => frameTime;
            set
            {
                if (value == frameTime) return;
                var old = frameTime;
                frameTime = value;
                OnPropertyChanged(old, value);
                GenContent();
            }
        }

        public TimeSpan AnimationFrameTime
        {
            get => animationFrameTime;
            set
            {
                if (value == animationFrameTime) return;
                var old = animationFrameTime;
                animationFrameTime = value;
                OnPropertyChanged(old, value);
                GenContent();
            }
        }

        public TimeSpan LayoutTime
        {
            get => layoutTime;
            set
            {
                if (value == layoutTime) return;
                var old = layoutTime;
                layoutTime = value;
                OnPropertyChanged(old, value);
                GenContent();
            }
        }
    }
}