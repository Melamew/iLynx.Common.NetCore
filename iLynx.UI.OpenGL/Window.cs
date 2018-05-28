using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL
{
    public class Window : GameWindow
    {
        private readonly Color background = Color.White;

        #region empty constructors
        public Window()
         : this(string.Empty) { }

        public Window(string title)
            : this(1920, 1080, title) { }

        public Window(int width, int height)
            : this(width, height, string.Empty) { }
        #endregion

        public Window(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title += $" OpenGL Version: {GL.GetString(StringName.Version)}";
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(background);
            SwapBuffers();
        }
    }
}