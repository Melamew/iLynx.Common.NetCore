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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
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
            EventManager.AddHandler(EventType.Closed, e => Close());
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

        public void Show()
        {
            var sw = new Stopwatch();
            while (IsOpen)
            {
                sw.Start();
                if (PollEvent(out var e))
                    EventManager.Dispatch(e);
                Clear(background);
                Draw(rootPanel);
                sw.Stop();
                FrameTime = sw.Elapsed;
                sw.Reset();
                if (ShowStats)
                    Draw(stats);
                Display();
            }
        }

        public bool ShowStats { get; set; } = true;

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