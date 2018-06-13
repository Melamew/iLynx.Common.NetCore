using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using iLynx.Common;
using iLynx.UI.SFML;
using SFML.Graphics;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public class Window : RenderWindow, IBindingSource
    {
        private delegate void EventMapper(Window source, Event e);

        private readonly DetachedBindingSource bindingSource = new DetachedBindingSource();
        private int renderedChildren;
        private static readonly Dictionary<EventType, EventMapper> EventMap = new Dictionary<EventType, EventMapper>
        {
            { EventType.Closed, (w, e) => w.Close() }
        };

        private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly List<IUIElement> children = new List<IUIElement>();
        private Color background = Color.Black;

        public Window(uint width, uint height, string title, Styles style = Styles.Default)
         : base(new VideoMode(width, height, 32), title, style)
        {
            SetupAlpha();
        }

        protected override bool PollEvent(out Event eventToFill)
        {
            var result = base.PollEvent(out eventToFill);
            if (result && EventMap.TryGetValue(eventToFill.Type, out var mapper))
            {
                var e = eventToFill;
                Task.Run(() => mapper?.Invoke(this, e));
            }
            return result;
        }

        public IEnumerable<IUIElement> Children => children;

        public void Show()
        {
            while (IsOpen)
            {
                DispatchEvents();
                Clear(background);
                RenderChildren();
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

        private void RenderChildren()
        {
            rwl.EnterReadLock();
            var viewPort = GetViewport(DefaultView);
            var rendered = 0;
            foreach (var child in children.Where(c => viewPort.Intersects(c.BoundingBox))) // Simple screenspace culling
            {
                Draw(child);
                ++rendered;
            }
            rwl.ExitReadLock();
            var renderedDelta = rendered - renderedChildren;
            renderedChildren = rendered;
            if (0 != renderedDelta)
                Console.WriteLine($"{(0 < renderedDelta ? "+" : "")}{renderedDelta} items to render, total: {rendered}");
        }

        public void AddChild(IUIElement element)
        {
            AddChildren(element);
            //rwl.EnterWriteLock();
            //children.Add(element);
            //element.LayoutPropertyChanged += OnLayoutChanged;
            //rwl.ExitWriteLock();
        }

        private void OnLayoutChanged(object sender, PropertyChangedEventArgs e)
        {
            Layout();
        }

        private void Layout()
        {
            var clientRect = new FloatRect(0f, 0f, Size.X, Size.Y);
            rwl.EnterReadLock();
            foreach (var child in children)
                child.Layout(clientRect);
            rwl.ExitReadLock();
        }

        public void AddChildren(params IUIElement[] elements)
        {
            if (null == elements) throw new ArgumentNullException(nameof(elements));
            rwl.EnterWriteLock();
            children.AddRange(elements.Select(x =>
            {
                x.LayoutPropertyChanged += OnLayoutChanged;
                return x;
            }));
            rwl.ExitWriteLock();
            Layout();
        }

        public IUIElement Parent { get; set; }

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
