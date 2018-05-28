using System;
using System.Collections.Generic;
using iLynx.UI.Controls;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Color = System.Drawing.Color;

namespace iLynx.UI.OpenGL.Controls
{
    public class Window : GameWindow, IWindow
    {
        private readonly Color backgroundColor;
        public Window() { }

        public Window(int width, int height)
            : base(width, height)
        {

        }

        public Window(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {

        }

        public Window(string title)
            : base(1920, 1080, GraphicsMode.Default, title)
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

            GL.ClearColor(BackgroundColor);
            SwapBuffers();
        }

        public ICollection<IUIElement> Children { get; }

        public Color BackgroundColor
        {
            get => backgroundColor;
            set => throw new NotImplementedException();
        }
    }
}