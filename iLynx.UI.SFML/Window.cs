using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using iLynx.Common;
using iLynx.UI.Sfml.Layout;
using SFML.Graphics;
using SFML.System;
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
        private Color background = Color.Transparent;
        private Panel rootPanel;
        private readonly StatisticsElement stats = new StatisticsElement { Padding = 16f, Margin = 16f };
        private TimeSpan frameTime;
        private readonly IBinding<TimeSpan> frameTimeBinding;
        private TimeSpan layoutTime;
        private readonly IBinding<TimeSpan> layoutTimeBinding;

        public Window(string title = "", Styles style = Styles.None)
            : this(VideoMode.DesktopMode, title, style) { }

        public Window(VideoMode mode, string title = "", Styles style = Styles.None)
            : base(mode, title, style)
        {
            SetupAlpha();
            base.SetFramerateLimit(120);
            stats.LayoutPropertyChanged += OnStatsLayoutPropertyChanged;
            rootPanel = new Canvas { Background = background };
            frameTimeBinding = new MultiBinding<TimeSpan>().Bind(this, nameof(FrameTime))
                .Bind(stats, nameof(StatisticsElement.FrameTime));
            layoutTimeBinding = new MultiBinding<TimeSpan>().Bind(this, nameof(LayoutTime))
                .Bind(stats, nameof(StatisticsElement.LayoutTime));
        }

        ~Window()
        {
            frameTimeBinding.Unbind(this).Unbind(stats);
            layoutTimeBinding.Unbind(this).Unbind(stats);
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
                sw.Reset();
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
            Layout();
        }

        protected virtual void Layout()
        {
            var sw = Stopwatch.StartNew();
            rootPanel.Layout(new FloatRect(0f, 0f, Size.X, Size.Y));
            sw.Stop();
            LayoutTime = sw.Elapsed;
        }

        private void OnStatsLayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var statsSize = stats.Measure((Vector2f)Size);
            var verticalOffset = Size.Y - statsSize.Y - stats.Margin.Vertical;
            stats.Layout(new FloatRect(0f, verticalOffset, Size.X, Size.Y - verticalOffset));
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
}