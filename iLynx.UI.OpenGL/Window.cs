﻿#region LICENSE
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
using System.ComponentModel;
using System.Diagnostics;
using iLynx.Common;
using iLynx.Graphics;
using iLynx.Graphics.Animation;
using iLynx.UI.OpenGL.Layout;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL
{
    public sealed class Window : GameWindow, IBindingSource//, IRenderContext
    {
        private readonly DetachedBindingSource bindingSource = new DetachedBindingSource();
        private Color background = Color.Black;
        private IUIElement root;
        private readonly StatisticsElement stats = new StatisticsElement { Padding = 16f, Margin = 16f };
        private TimeSpan frameTime;
        private readonly IBinding<TimeSpan> frameTimeBinding;
        private TimeSpan layoutTime;
        private readonly IBinding<TimeSpan> layoutTimeBinding;

        //private readonly IView view = new View();
        //private readonly Thread renderThread;

        public Window(int width, int height, string title = "")
            : base(width, height, GraphicsMode.Default, title)
        {
            stats.LayoutPropertyChanged += OnStatsLayoutPropertyChanged;
            root = new Canvas { Background = background };
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

        public IUIElement RootElement
        {
            get => root;
            set
            {
                if (value == root) return;
                var old = root;
                old.LayoutPropertyChanged -= OnLayoutChanged;
                root = value;
                root.LayoutPropertyChanged += OnLayoutChanged;
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

        public void AddPropertyChangedHandler<TValue>(string valueName, ValueChangedCallback<TValue> callback)
        {
            bindingSource.AddPropertyChangedHandler(valueName, callback);
        }

        public void RemovePropertyChangedHandler<TValue>(string valueName, ValueChangedCallback<TValue> callback)
        {
            bindingSource.RemovePropertyChangedHandler(valueName, callback);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //Animator.Tick();
            root?.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();
            GL.ClearColor(Color.Lime);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //root.Draw(view);
            SwapBuffers();
            sw.Stop();
            FrameTime = sw.Elapsed;
            sw.Reset();
        }

        public void Show()
        {
            Run(120d);
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

        private void Layout()
        {
            var sw = Stopwatch.StartNew();
            root.Layout(new RectangleF(0f, 0f, ClientSize.Width, ClientSize.Height));
            sw.Stop();
            LayoutTime = sw.Elapsed;
        }

        private void OnStatsLayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var statsSize = stats.Measure(new SizeF(ClientSize.Width, ClientSize.Height));
            var verticalOffset = ClientSize.Height - statsSize.Height - stats.Margin.Vertical;
            stats.Layout(new RectangleF(0f, verticalOffset, ClientSize.Width, ClientSize.Height - verticalOffset));
        }
    }
}
