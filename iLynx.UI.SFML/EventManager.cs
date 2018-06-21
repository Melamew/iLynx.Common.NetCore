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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using iLynx.Common.Threading;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public class BackgroundTicker
    {
        private static Thread thread;
        private volatile bool isRunning;
        private TimeSpan frameInterval;
        private double desiredFrequency = 60d;
        private Action callback;

        public BackgroundTicker()
        {
            frameInterval = TimeSpan.FromMilliseconds(1000d / desiredFrequency);
        }

        public void Start(Action tickCallback)
        {
            if (isRunning) return;
            isRunning = true;
            callback = tickCallback;
            thread = new Thread(DoTicks) { IsBackground = true };
            thread.Start();
        }

        public void Stop()
        {
            if (!isRunning) return;
            isRunning = false;
            thread.Join();
            thread = null;
            callback = null;
        }

        private void DoTicks()
        {
            var sw = new Stopwatch();
            while (isRunning)
            {
                sw.Start();
                callback();
                sw.Stop();
                var interval = frameInterval - sw.Elapsed;
                sw.Reset();
                if (TimeSpan.Zero > interval)
                {
                    Console.WriteLine(
                        $"{nameof(BackgroundTicker)} tick took longer than desired {frameInterval}, clamping to zero. Delta: {interval}");
                    interval = TimeSpan.Zero;
                }
                Thread.CurrentThread.Join(interval);
            }
        }

        public double DesiredFrequency
        {
            get => desiredFrequency;
            set
            {
                if (Math.Abs(value - desiredFrequency) <= double.Epsilon * 10d) return;
                desiredFrequency = value;
                frameInterval = TimeSpan.FromMilliseconds(1000d / desiredFrequency);
            }
        }

        public TimeSpan TickInterval => frameInterval;
    }

    public static class EventManager
    {
        private static readonly ReaderWriterLockSlim Rwl = new ReaderWriterLockSlim();
        private static readonly BackgroundTicker Ticker = new BackgroundTicker();
        private static readonly Queue<Event> DispatchQueue = new Queue<Event>();

        private static readonly Dictionary<EventType, List<Action<Event>>> EventHandlers =
            new Dictionary<EventType, List<Action<Event>>>();

        static EventManager()
        {
            Ticker.DesiredFrequency = 120d;
            StartEventPump();
        }

        private static void StartEventPump()
        {
            Ticker.Start(PollEvents);
        }

        public static void StopEventPump()
        {
            Ticker.Stop();
        }

        private static void PollEvents()
        {
            using (Rwl.AcquireReadLock())
            {
                if (DispatchQueue.TryDequeue(out var e) && EventHandlers.TryGetValue(e.Type, out var handlers))
                    handlers.ForEach(x => x?.Invoke(e));
            }
        }

        public static void AddHandler(EventType type, Action<Event> handler)
        {
            using (Rwl.AcquireWriteLock())
            {
                if (EventHandlers.TryGetValue(type, out var list) && !list.Contains(handler))
                    list.Add(handler);
                else if (null == list)
                {
                    list = new List<Action<Event>> { handler };
                    EventHandlers.Add(type, list);
                }
            }
        }

        public static void RemoveHandler(EventType type, Action<Event> handler)
        {
            using (Rwl.AcquireWriteLock())
            {
                if (!EventHandlers.TryGetValue(type, out var list)) return;
                list.Remove(handler);
                if (0 >= list.Count)
                    EventHandlers.Remove(type);
            }
        }

        public static double PollFrequency
        {
            get => Ticker.DesiredFrequency;
            set => Ticker.DesiredFrequency = value;
        }

        public static void Dispatch(Event e)
        {
            using (Rwl.AcquireWriteLock())
                DispatchQueue.Enqueue(e);
        }
    }
}