using System;
using System.Collections.Generic;
using iLynx.UI.Controls;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using static System.Math;

namespace iLynx.UI.OpenGL.Controls
{
    public class Window : GameWindow, IWindow
    {
        private Color background = Color.White;
        private Color foreground = Color.Black;
        private readonly List<IUIElement> children = new List<IUIElement>();

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

        public ICollection<IUIElement> Children => children;

        public new float Width
        {
            get => base.Width;
            set => base.Width = (int)Round(value);
        }
        public new float Height
        {
            get => base.Height;
            set => base.Height = (int)Round(value);
        }

        // TODO: Cleanup
        public System.Numerics.Vector2 Position
        {
            get => new System.Numerics.Vector2(Bounds.Left, Bounds.Top);
            set => Bounds = new Rectangle((int)Round(value.X), (int)Round(value.Y), base.Width, base.Height);
        }
        public System.Drawing.Color Background
        {
            get => System.Drawing.Color.FromArgb(background.A, background.R, background.G, background.B);
            set => background = new Color(value.R, value.G, value.B, value.A);
        }
        
        public System.Drawing.Color Foreground
        {
            get => System.Drawing.Color.FromArgb(foreground.A, foreground.R, foreground.G, foreground.B);
            set => foreground = new Color(value.R, value.G, value.B, value.A);
        }
    }
}