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
using iLynx.Graphics.Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace iLynx.Graphics.TestBench
{
    public class MainWindow : GameWindow
    {
        private IView view2D;
        private IView view3D;

        private RectangleGeometry topLeft, topRight, bottomLeft, bottomRight;
        private Cuboid cuboid;
        private IRenderContext renderContext;

        public MainWindow(int width, int height, string title)
            : base(width, height, new GraphicsMode(new ColorFormat(32, 32, 32, 32), 32, 32), title, GameWindowFlags.Default, DisplayDevice.Default)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            renderContext = new RenderContext(Context, WindowInfo);
            view2D = new View(renderContext);
            view3D = new View(renderContext);

            topLeft = new RectangleGeometry(250f, 250f, new Color(255, 0, 0, 128)) { Origin = new Vector3(125f, 125f, 0f) };
            topRight = new RectangleGeometry(250f, 250f, new Color(255, 0, 0, 128)) { Origin = new Vector3(125f, 125f, 0f) };
            bottomLeft = new RectangleGeometry(250f, 250f, new Color(255, 0, 0, 128)) { Origin = new Vector3(125f, 125f, 0f) };
            bottomRight = new RectangleGeometry(250f, 250f, new Color(255, 0, 0, 128)) { Origin = new Vector3(125f, 125f, 0f) };
            cuboid = new Cuboid(Color.Lime, 1f, 1f, 1f) { Origin = new Vector3(0.5f) };
            view2D.AddDrawable(topLeft);
            view3D.AddDrawable(cuboid);

            Animator.Start(x => topLeft.Rotation = Quaternion.FromAxisAngle(new Vector3(0f, 0f, 1f), (float)(x * Math.PI * 2d)), TimeSpan.FromSeconds(2.5d), LoopMode.Restart, EasingFunctions.Linear);
            Animator.Start(x => topLeft.Origin = (float)x * new Vector3(topLeft.Width, topLeft.Height, 0f), TimeSpan.FromSeconds(3.33d), LoopMode.Reverse, EasingFunctions.QuadraticInOut);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(ClientRectangle);
            Console.WriteLine($"Resize to: {ClientRectangle}");
            view2D.Projection = Matrix4.CreateOrthographicOffCenter(0f, ClientRectangle.Width, ClientRectangle.Height, 0f, -1f, 1f);
            //view3D.Projection = Matrix4.CreateOrthographic(ClientRectangle.Width / 1000f, ClientRectangle.Height / 1000f, 0f, 10f);
            view3D.Projection = Matrix4.CreatePerspectiveFieldOfView(30f * (MathF.PI / 180),
                (float)ClientRectangle.Width / (float)ClientRectangle.Height, 1f, 1000f);
            view3D.Scale = new Vector3(1f, 1f, -1f);
            cuboid.Translation = new Vector3(0f, 0f, 10f);
            topLeft.Translation = new Vector3(topLeft.Width, topLeft.Height, 0f);
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
                    cuboid.Translate(0f, 0f, .1f);
                    break;
                case Key.Down:
                case Key.S:
                    cuboid.Translate(0f, 0f, -.1f);
                    break;
                case Key.Left:
                case Key.A:
                    cuboid.Translate(-.1f, 0f, 0f);
                    break;
                case Key.Right:
                case Key.D:
                    cuboid.Translate(.1f, 0f, 0f);
                    break;
                case Key.Q:
                    cuboid.Translate(0f, .1f, 0f);
                    break;
                case Key.Z:
                    cuboid.Translate(0f, -.1f, 0f);
                    break;
                case Key.KeypadAdd when e.Modifiers.HasFlag(KeyModifiers.Shift):
                    cuboid.Scale += new Vector3(0.1f);
                    break;
                case Key.KeypadSubtract when e.Modifiers.HasFlag(KeyModifiers.Shift):
                    cuboid.Scale -= new Vector3(0.1f);
                    break;
                case Key.KeypadAdd:
                    view3D.Scale += new Vector3(0.1f);
                    break;
                case Key.KeypadSubtract:
                    view3D.Scale -= new Vector3(0.1f);
                    break;
                case Key.Keypad4 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    cuboid.RotateAround(new Vector3(0f, 1f, 0f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad6 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    cuboid.RotateAround(new Vector3(0f, 1f, 0f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad2 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    cuboid.RotateAround(new Vector3(1f, 0f, 0f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad8 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    cuboid.RotateAround(new Vector3(1f, 0f, 0f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad7 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    cuboid.RotateAround(new Vector3(0f, 0f, 1f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad9 when e.Modifiers.HasFlag(KeyModifiers.Alt):
                    cuboid.RotateAround(new Vector3(0f, 0f, 1f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad4:
                    cuboid.RotateAroundGlobal(new Vector3(0f, 1f, 0f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad6:
                    cuboid.RotateAroundGlobal(new Vector3(0f, 1f, 0f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad2:
                    cuboid.RotateAroundGlobal(new Vector3(1f, 0f, 0f), -1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad8:
                    cuboid.RotateAroundGlobal(new Vector3(1f, 0f, 0f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad7:
                    cuboid.RotateAroundGlobal(new Vector3(0f, 0f, 1f), 1f * (MathF.PI / 180f));
                    break;
                case Key.Keypad9:
                    cuboid.RotateAroundGlobal(new Vector3(0f, 0f, 1f), -1f * (MathF.PI / 180f));
                    break;
                default: return;
            }

            Console.WriteLine($"Key: {e.Key} Modifiers: {e.Modifiers}");
            cuboid.Rotation.ToAxisAngle(out var axis, out var angle);
            Console.WriteLine($"Cuboid position: {cuboid.Translation}");
            Console.WriteLine($"Cuboid rotation: {axis} by {angle}");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Animator.Tick();
            view2D.PrepareRender();
            view3D.PrepareRender();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Transparent);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            view3D.Render();
            view2D.Render();
            SwapBuffers();
        }
    }
}
