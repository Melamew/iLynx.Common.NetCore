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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using iLynx.Common.Threading;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public class EventManager : BackgroundWorker
    {
        private readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
        private readonly ConcurrentQueue<Event> dispatchQueue = new ConcurrentQueue<Event>();
        private readonly Dictionary<EventType, List<Action<Event>>> eventHandlers =
            new Dictionary<EventType, List<Action<Event>>>();
        private volatile bool isRunning = false;
        private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private static EventManager instance;

        private static EventManager Instance => instance ?? (instance = new EventManager());

        public override void Start()
        {
            isRunning = true;
            base.Start();
        }

        public override void Stop()
        {
            isRunning = false;
            base.Stop();
        }

        static EventManager()
        {
            StartEventPump();
        }

        private static void StartEventPump()
        {
            Instance.Start();
        }

        public static void StopEventPump()
        {
            Instance.Stop();
        }

        public static void AddHandler(EventType type, Action<Event> handler)
        {
            Instance.RegisterHandler(type, handler);
        }

        public void RegisterHandler(EventType type, Action<Event> handler)
        {
            using (rwl.AcquireWriteLock())
            {
                if (eventHandlers.TryGetValue(type, out var list) && !list.Contains(handler))
                    list.Add(handler);
                else if (null == list)
                {
                    list = new List<Action<Event>> { handler };
                    eventHandlers.Add(type, list);
                }
            }
        }

        public static void RemoveHandler(EventType type, Action<Event> handler)
        {
            Instance.UnregisterHandler(type, handler);
        }

        public void UnregisterHandler(EventType type, Action<Event> handler)
        {
            using (rwl.AcquireWriteLock())
            {
                if (!eventHandlers.TryGetValue(type, out var list)) return;
                list.Remove(handler);
                if (0 >= list.Count)
                    eventHandlers.Remove(type);
            }
        }

        public static void Dispatch(Event e)
        {
            Instance.DispatchEvent(e);
        }

        public void DispatchEvent(Event e)
        {
            dispatchQueue.Enqueue(e);
            autoResetEvent.Set();
        }

        protected override void Run()
        {
            while (isRunning)
            {
                autoResetEvent.WaitOne();
                while (!dispatchQueue.IsEmpty)
                {
                    if (dispatchQueue.TryDequeue(out var e) && eventHandlers.TryGetValue(e.Type, out var handlers))
                        handlers.ForEach(x => x?.Invoke(e));
                }
            }
        }
    }
}