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
using System.Diagnostics;
using System.Threading;

namespace iLynx.Common.Threading
{
    public class BackgroundTicker : BackgroundWorker
    {
        private volatile bool isRunning;
        private TimeSpan frameInterval;
        private double desiredFrequency = 60d;

        protected BackgroundTicker()
        {
            frameInterval = TimeSpan.FromMilliseconds(1000d / desiredFrequency);
        }

        public override void Start()
        {
            if (isRunning) return;
            isRunning = true;
            base.Start();
        }

        protected override void Run()
        {
            var sw = new Stopwatch();
            while (isRunning)
            {
                sw.Start();
                Tick();
                sw.Stop();
                var interval = frameInterval - sw.Elapsed;
                sw.Reset();
                if (TimeSpan.Zero > interval)
                {
                    Console.WriteLine(
                        $"{nameof(CallbackTicker)} tick took longer than desired {frameInterval}, clamping to zero. Delta: {interval}");
                    interval = TimeSpan.Zero;
                }
                Thread.CurrentThread.Join(interval);
            }
        }

        public override void Stop()
        {
            if (!isRunning) return;
            isRunning = false;
            base.Stop();
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

        protected virtual void Tick()
        {

        }
    }
}