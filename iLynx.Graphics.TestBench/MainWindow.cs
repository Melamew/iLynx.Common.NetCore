using System;
using iLynx.Graphics.Rendering;
using iLynx.Graphics.Scene;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.TestBench
{
    public class MainWindow : GameWindow
    {
        private readonly IScene scene = new Scene.Scene();
        private readonly IRenderContext context = new OpenGlRenderContext();

        public MainWindow(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default)
        {

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            scene.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            scene.Display(context);
        }
    }
}
