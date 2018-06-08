using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iLynx.UI.SFML;
using SFML.Graphics;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public class Window : RenderWindow, IUIElement
    {
        private delegate void EventMapper(Window source, Event e);

        private static readonly Dictionary<EventType, EventMapper> EventMap = new Dictionary<EventType, EventMapper>
        {
            { EventType.MouseMoved, (w, e) =>
            {
                //Console.WriteLine($"Mouse Moved to: {e.MouseMove.X},{e.MouseMove.Y}");
            } },
            { EventType.Closed, (w, e) => w.Close() }
        };

        private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private readonly List<Drawable> children = new List<Drawable>();

        internal Window(VideoMode mode, string title)
            : base(mode, title)
        {
        }

        internal Window(VideoMode mode, string title, Styles style)
            : base(mode, title, style)
        {
        }

        internal Window(VideoMode mode, string title, Styles style, ContextSettings settings)
            : base(mode, title, style, settings)
        {
        }

        internal Window(IntPtr handle)
            : base(handle)
        {
        }

        internal Window(IntPtr handle, ContextSettings settings)
            : base(handle, settings)
        {
        }

        public Window(uint width, uint height, string title)
         : this(new VideoMode(width, height), title)
        {

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

        public IEnumerable<Drawable> Children => children;

        public void Show()
        {
            while (IsOpen)
            {
                DispatchEvents();
                RenderChildren();
                Display();
            }
        }

        private void RenderChildren()
        {
            rwl.EnterReadLock();
            foreach (var child in children)
                Draw(child);
            rwl.ExitReadLock();
        }

        public void AddChild(Drawable element)
        {
            rwl.EnterWriteLock();
            children.Add(element);
            rwl.ExitWriteLock();
        }

        public IUIElement Parent { get; set; }
        public event EventHandler<MouseEventArgs> MouseMove;
    }
}
