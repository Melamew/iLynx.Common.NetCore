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
using System.Linq;
using System.Text;
using System.Threading;
using iLynx.UI.OpenGL.Controls;
using OpenTK;

namespace iLynx.UI.OpenGL
{
    public class StatisticsElement : ContentControl
    {
        private TimeSpan frameTime;
        private TimeSpan animationFrameTime;
        private TimeSpan layoutTime;
        private readonly StringBuilder builder = new StringBuilder();
        private readonly object syncObj = new object();
        private readonly Queue<TimeSpan> frameTimes = new Queue<TimeSpan>();

        public StatisticsElement()
        {
            Background = new Color(0, 0, 0, 128);
            Foreground = new Color(255, 255, 255, 255);
        }

        private void GenContent()
        {
            if (!Monitor.TryEnter(syncObj)) return;
            try
            {
                while (frameTimes.Count > 100)
                    frameTimes.Dequeue();
                var avgTimes = frameTimes.Sum(x => x.TotalMilliseconds) / frameTimes.Count;
                builder.Clear();
                builder.Append($"FrameTime: {frameTime.TotalMilliseconds:f2} ms\n");
                builder.Append($"Avg. FrameTime: {avgTimes:f2} ms\n");
                //builder.Append($"Animation FrameTime: {animationFrameTime.TotalMilliseconds:f2} ms\n");
                builder.Append($"Layout Time: {layoutTime.TotalMilliseconds:f2} ms");
                ContentString = builder.ToString();
            }
            finally
            {
                Monitor.Exit(syncObj);
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
                OnPropertyChanged(old, value);
                Monitor.Enter(syncObj);
                frameTimes.Enqueue(value);
                Monitor.Exit(syncObj);
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