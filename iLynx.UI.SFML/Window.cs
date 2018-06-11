using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iLynx.UI.SFML;
using iLynx.UI.SFML.Controls;
using SFML.Graphics;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public class Window : RenderWindow
    {
        private delegate void EventMapper(Window source, Event e);

        private int renderedChildren = 0;
        private static readonly Dictionary<EventType, EventMapper> EventMap = new Dictionary<EventType, EventMapper>
        {
            //{ EventType.MouseMoved, (w, e) =>
            //{
                
            //    //Console.WriteLine($"Mouse Moved to: {e.MouseMove.X},{e.MouseMove.Y}");
            //} },
            { EventType.Closed, (w, e) => w.Close() }
        };

        private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly List<IUIElement> children = new List<IUIElement>();

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

        public IEnumerable<IUIElement> Children => children;

        public void Show()
        {
            while (IsOpen)
            {
                DispatchEvents();
                Clear();
                RenderChildren();
                Display();
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
            rwl.EnterWriteLock();
            children.Add(element);
            rwl.ExitWriteLock();
        }

        public void AddChildren(params IUIElement[] elements)
        {
            if (null == elements) throw new ArgumentNullException(nameof(elements));
            rwl.EnterWriteLock();
            children.AddRange(elements);
            rwl.ExitWriteLock();
        }

        public IUIElement Parent { get; set; }
        //public event EventHandler<MouseEventArgs> MouseMove;
    }
}
