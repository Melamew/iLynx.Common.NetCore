using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using iLynx.Common;
using iLynx.UI.SFML.Controls;
using SFML.Graphics;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public class Window : RenderWindow, IBindingSource
    {
        private delegate void EventMapper(Window source, Event e);

        private readonly DetachedBindingSource bindingSource = new DetachedBindingSource();
        private static readonly Dictionary<EventType, EventMapper> EventMap = new Dictionary<EventType, EventMapper>
        {
            { EventType.MouseMoved, (w, e) => w.OnMouseMove(new MouseMoveEventArgs(e.MouseMove)) },
            { EventType.Closed, (w, e) => w.Close() }
        };
        private Color background = Color.Black;
        private Panel rootPanel;

        public Window(uint width, uint height, string title, Styles style = Styles.Default)
         : base(new VideoMode(width, height, 32), title, style)
        {
            SetupAlpha();
            rootPanel = new AbsolutePositionPanel { Background = background };
        }

        protected virtual void OnMouseMove(MouseMoveEventArgs e)
        {
            MouseMoved?.Invoke(this, e);
        }

        public new event EventHandler<MouseMoveEventArgs> MouseMoved;

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

        //public IEnumerable<IUIElement> Children => children;

        public void Show()
        {
            while (IsOpen)
            {
                DispatchEvents();
                Clear(background);
                Draw(rootPanel);
                Display();
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

        private void OnLayoutChanged(object sender, PropertyChangedEventArgs e)
        {
            Layout();
        }

        protected virtual void Layout()
        {
            rootPanel.Layout(new FloatRect(0f, 0f, Size.X, Size.Y));
        }

        public void AddPropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler)
        {
            bindingSource.AddPropertyChangedHandler(valueName, handler);
        }

        public void RemovePropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler)
        {
            bindingSource.RemovePropertyChangedHandler(valueName, handler);
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
        static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

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
