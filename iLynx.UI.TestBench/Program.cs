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
using System.Threading;
using iLynx.UI.Sfml;
using iLynx.UI.Sfml.Animation;
using iLynx.UI.Sfml.Controls;
using iLynx.UI.Sfml.Layout;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Window = iLynx.UI.Sfml.Window;

namespace iLynx.UI.TestBench
{
    public static class Program
    {
        private static Window window;
        private static Canvas canvas;
        private static ContentControl stretchedControl;
        private static TextBox textBox;

        private static void Main()
        {
            StartWindow();
            var endMargin = stretchedControl.RenderSize.X / 2f - stretchedControl.Content.BoundingBox.Width / 2f;
            Animator.AddAnimation(new CallbackAnimation(p =>
                {
                    stretchedControl.Margin = new Thickness((float)p * endMargin, 4f);
                }, TimeSpan.FromSeconds(1d), LoopMode.Reverse, EasingFunctions.CubicIn));
        }

        private static void StartWindow()
        {
            var t = new Thread(() =>
            {
                window = new Window(new VideoMode(1920, 1080), "Test");
                window.Show();
            });
            t.Start();
            while (null == window) Thread.CurrentThread.Join(250);
            var root = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Background = ColorUtils.FromRgbA(.1f, .1f, .1f, 1f)
            };
            window.RootElement = root;
            var stackPanel = new StackPanel
            {
                Size = (Vector2f)window.Size * 0.5f,
                Background = ColorUtils.FromRgbA(.2f, .2f, .2f, 1f),
                Margin = 16f
            };
            var labelMargins = new Thickness(8f);
            stackPanel.AddChild(new ContentControl
            {
                ContentString = "Left Aligned",
                Foreground = Color.Green,
                Background = Color.Black,
                Margin = labelMargins,
                Padding = 16f
            },
                new ContentControl
                {
                    Foreground = Color.Green,
                    Background = Color.Black,
                    Margin = labelMargins,
                    HorizontalAlignment = Alignment.End,
                    ContentString = "Right Aligned"
                },
                new ContentControl
                {
                    ContentString = "Centered",
                    Margin = labelMargins,
                    HorizontalAlignment = Alignment.Center,
                    Foreground = Color.Green,
                    Background = Color.Black
                },
                stretchedControl = new ContentControl
                {
                    ContentString = "Stretched",
                    Margin = labelMargins,
                    HorizontalAlignment = Alignment.Stretch,
                    Foreground = Color.Green,
                    Background = Color.Black
                },
                textBox = new TextBox
                {
                    Font = UIElement.DefaultFont,
                    FontSize = 48,
                    Foreground = Color.White,
                    Background = new Color(32, 32, 32, 255),
                    Size = new Vector2f(800, 400),
                    Text = "Default Text",
                });
            canvas = new Canvas
            {
                Background = ColorUtils.FromRgbA(.3f, .3f, .3f, 1f),
                Margin = 16f
            };
            canvas.AddChild(new ContentControl
            {
                Size = new Vector2f(120f, 10f),
                ContentString = "This content will not fit inside the control, yet it is not clipped.",
                Background = new Color(128, 64, 64, 255)
            });
            root.AddChild(stackPanel, canvas);
        }
    }
}