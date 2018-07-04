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
using iLynx.Graphics.Animation;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.TestBench
{
    public class MainWindow : GameWindow
    {
        private readonly IDrawingContext target;
        private readonly RectangleGeometry geometry;
        //private readonly Text text = new Text("./Text/fonts/OpenSans-Regular.ttf");

        public MainWindow(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default)
        {
            target = new DrawingContext();
            geometry = new RectangleGeometry(500f, 500f, Color.Red, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Animator.Start(x => geometry.Rotation = Quaternion.FromAxisAngle(new Vector3(0f, 0f, 1f), (float)(x * Math.PI * 2d)), TimeSpan.FromSeconds(2.5d), LoopMode.Restart, EasingFunctions.Linear);
            Animator.Start(x => geometry.Origin = (float)x * new Vector3(geometry.Width, geometry.Height, 1f), TimeSpan.FromSeconds(2.5d), LoopMode.Reverse, EasingFunctions.QuadraticInOut);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(ClientRectangle);
            Console.WriteLine($"Resize to: {ClientRectangle}");
            target.ViewTransform = Matrix4.CreateTranslation(-.5f * ClientRectangle.Width, -.5f * ClientRectangle.Height, 0f);
            target.ViewTransform *= Matrix4.CreateScale(1f / (ClientRectangle.Width / 2f), -1f / (ClientRectangle.Height / 2f), 1.0f);
            geometry.Translation = new Vector3(ClientRectangle.Width / 2f, ClientRectangle.Height / 2f, 0f);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Animator.Tick();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            target.Draw(geometry);
            SwapBuffers();
        }
    }
}
