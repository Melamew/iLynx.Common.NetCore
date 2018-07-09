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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using iLynx.Graphics.Animation;
using iLynx.Graphics.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;

namespace iLynx.Graphics.TestBench
{
    public class MainWindow : GameWindow
    {
        private Cuboid m_cuboid;
        private IView m_view2D;
        private IView m_view3D;
        private readonly IRenderContext m_renderContext;

        private RectangleGeometry m_topLeft;//, topRight, bottomLeft, bottomRight;

        public MainWindow(int width, int height, string title)
            : base(width, height, new GraphicsMode(new ColorFormat(32, 32, 32, 32), 32, 32), title,
                GameWindowFlags.Default, DisplayDevice.Default)
        {
            m_renderContext = new RenderContext(Context, WindowInfo);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            m_renderContext.AddView(m_view2D = new View());
            m_renderContext.AddView(m_view3D = new View());

            m_topLeft = new RectangleGeometry(250f, 250f, new Color32(1f, 1f, 1f, .5f))
            {
                Origin = new Vector3(125f, 125f, 0f),
            };

            m_cuboid = new Cuboid(Color32.Lime, 1f, 1f, 1f) { Origin = new Vector3(0.5f) };
            m_view2D.AddDrawable(m_topLeft);
            m_view3D.AddDrawable(m_cuboid);
            m_renderContext.Animator.Start(
                x => m_topLeft.Rotation = Quaternion.FromAxisAngle(new Vector3(0f, 0f, 1f), (float)(x * Math.PI * 2d)),
                TimeSpan.FromSeconds(2.5d), LoopMode.Restart, EasingFunctions.Linear);
            m_renderContext.Animator.Start(x => m_topLeft.Origin = (float)x * new Vector3(m_topLeft.Width, m_topLeft.Height, 0f),
                TimeSpan.FromSeconds(3.33d), LoopMode.Reverse, EasingFunctions.QuadraticInOut);
            LoadTestTexture();
        }

        private void LoadTestTexture()
        {
            Task.Run(
                () =>
                {
                    const string testImage = @"C:\Users\melan\Google Drive\Dev Work\test_image.png";
                    Image<Color32> img;
                    using (var stream = File.OpenRead(testImage))
                        img = Image.Load<Color32>(stream);
                    m_renderContext.QueueForSync(i => m_topLeft.Texture = Texture.FromImage(i), img);
                }
            );
            //Texture.FromFile(testImage).ContinueWith(task => m_topLeft.Texture = task.Result);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(ClientRectangle);
            Console.WriteLine($"Resize to: {ClientRectangle}");
            m_view2D.Projection =
                Matrix4.CreateOrthographicOffCenter(0f, ClientRectangle.Width, ClientRectangle.Height, 0f, -1f, 1f);
            //view3D.Projection = Matrix4.CreateOrthographic(ClientRectangle.Width / 1000f, ClientRectangle.Height / 1000f, 0f, 10f);
            m_view3D.Projection = Matrix4.CreatePerspectiveFieldOfView(30f * (MathF.PI / 180),
                ClientRectangle.Width / (float)ClientRectangle.Height, 1f, 1000f);
            m_view3D.Scale = new Vector3(1f, 1f, -1f);
            m_cuboid.Translation = new Vector3(0f, 0f, 10f);
            m_topLeft.Translation = new Vector3(m_topLeft.Width, m_topLeft.Height, 0f);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Key.Escape:
                case Key.F4 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                case Key.Q when e.Modifiers.HasFlag(KeyModifiers.Control):
                case Key.W when e.Modifiers.HasFlag(KeyModifiers.Control):
                case Key.X when e.Modifiers.HasFlag(KeyModifiers.Control):
                    Close();
                    return;
                case Key.Up:
                case Key.W:
                    m_cuboid.Translate(0f, 0f, .1f);
                    break;
                case Key.Down:
                case Key.S:
                    m_cuboid.Translate(0f, 0f, -.1f);
                    break;
                case Key.Left:
                case Key.A:
                    m_cuboid.Translate(-.1f, 0f, 0f);
                    break;
                case Key.Right:
                case Key.D:
                    m_cuboid.Translate(.1f, 0f, 0f);
                    break;
                case Key.Q:
                    m_cuboid.Translate(0f, .1f, 0f);
                    break;
                case Key.Z:
                    m_cuboid.Translate(0f, -.1f, 0f);
                    break;
                case Key.KeypadAdd when e.Modifiers.HasFlag(KeyModifiers.Shift):
                    m_cuboid.Scale += new Vector3(0.1f);
                    break;
                case Key.KeypadSubtract when e.Modifiers.HasFlag(KeyModifiers.Shift):
                    m_cuboid.Scale -= new Vector3(0.1f);
                    break;
                case Key.KeypadAdd:
                    m_view3D.Scale += new Vector3(0.1f);
                    break;
                case Key.KeypadSubtract:
                    m_view3D.Scale -= new Vector3(0.1f);
                    break;
                case Key.Keypad4 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    m_cuboid.RotateAround(new Vector3(0f, 1f, 0f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad6 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    m_cuboid.RotateAround(new Vector3(0f, 1f, 0f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad2 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    m_cuboid.RotateAround(new Vector3(1f, 0f, 0f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad8 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    m_cuboid.RotateAround(new Vector3(1f, 0f, 0f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad7 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    m_cuboid.RotateAround(new Vector3(0f, 0f, 1f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad9 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    m_cuboid.RotateAround(new Vector3(0f, 0f, 1f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad4:
                    m_cuboid.RotateAroundGlobal(new Vector3(0f, 1f, 0f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad6:
                    m_cuboid.RotateAroundGlobal(new Vector3(0f, 1f, 0f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad2:
                    m_cuboid.RotateAroundGlobal(new Vector3(1f, 0f, 0f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad8:
                    m_cuboid.RotateAroundGlobal(new Vector3(1f, 0f, 0f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad7:
                    m_cuboid.RotateAroundGlobal(new Vector3(0f, 0f, 1f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad9:
                    m_cuboid.RotateAroundGlobal(new Vector3(0f, 0f, 1f), -1f * (MathF.PI / 180f));
                    break;
                default: return;
            }

            Console.WriteLine($"Key: {e.Key} Modifiers: {e.Modifiers}");
            m_cuboid.Rotation.ToAxisAngle(out var axis, out var angle);
            Console.WriteLine($"Cuboid position: {m_cuboid.Translation}");
            Console.WriteLine($"Cuboid rotation: {axis} by {angle}");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            m_renderContext.ProcessSyncQueue();
            m_renderContext.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            m_renderContext.Render();
            SwapBuffers();
        }
    }
}