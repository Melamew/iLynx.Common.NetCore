#region LICENSE
/*
 * Copyright 2018 Melanie Hjorth
 *
 * Redistribution and use in source and binary forms,
 * with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote
 * products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */
#endregion

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using iLynx.Common.Threading;

namespace iLynx.UI.OpenGL.Input
{
    //public class EventDispatcher : BackgroundWorker
    //{
    //    private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
    //    private readonly ConcurrentQueue<(Window Window, Event Event)> dispatchQueue = new ConcurrentQueue<(Window, Event)>();
    //    private readonly Dictionary<EventType, List<SfmlEventHandler>> eventHandlers =
    //        new Dictionary<EventType, List<SfmlEventHandler>>();
    //    private volatile bool isRunning;
    //    private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
    //    private static EventDispatcher instance;

    //    private static EventDispatcher Instance => instance ?? (instance = new EventDispatcher());

    //    public override void Start()
    //    {
    //        isRunning = true;
    //        base.Start();
    //    }

    //    public override void Stop()
    //    {
    //        isRunning = false;
    //        base.Stop();
    //    }

    //    static EventDispatcher()
    //    {
    //        AddHandler(EventType.Closed, (w, e) => w.Close());
    //        StartEventPump();
    //    }

    //    private static void StartEventPump()
    //    {
    //        Instance.Start();
    //    }

    //    public static void StopEventPump()
    //    {
    //        Instance.Stop();
    //    }

    //    public static void AddHandler(EventType type, SfmlEventHandler handler)
    //    {
    //        Instance.RegisterHandler(type, handler);
    //    }

    //    public void RegisterHandler(EventType type, SfmlEventHandler handler)
    //    {
    //        using (rwl.AcquireWriteLock())
    //        {
    //            if (eventHandlers.TryGetValue(type, out var list) && !list.Contains(handler))
    //                list.Add(handler);
    //            else if (null == list)
    //            {
    //                list = new List<SfmlEventHandler> { handler };
    //                eventHandlers.Add(type, list);
    //            }
    //        }
    //    }

    //    public static void RemoveHandler(EventType type, SfmlEventHandler handler)
    //    {
    //        Instance.UnregisterHandler(type, handler);
    //    }

    //    public void UnregisterHandler(EventType type, SfmlEventHandler handler)
    //    {
    //        using (rwl.AcquireWriteLock())
    //        {
    //            if (!eventHandlers.TryGetValue(type, out var list)) return;
    //            list.Remove(handler);
    //            if (0 >= list.Count)
    //                eventHandlers.Remove(type);
    //        }
    //    }

    //    public static void Dispatch(Window source, Event e)
    //    {
    //        Instance.DispatchEvent(source, e);
    //    }

    //    public void DispatchEvent(Window source, Event e)
    //    {
    //        dispatchQueue.Enqueue((source, e));
    //        autoResetEvent.Set();
    //    }

    //    protected override void Run()
    //    {
    //        while (isRunning)
    //        {
    //            autoResetEvent.WaitOne();
    //            while (!dispatchQueue.IsEmpty)
    //            {
    //                if (dispatchQueue.TryDequeue(out var e) && eventHandlers.TryGetValue(e.Event.Type, out var handlers))
    //                    handlers.ForEach(x => x?.Invoke(e.Window, e.Event));
    //            }
    //        }
    //    }
    //}
}