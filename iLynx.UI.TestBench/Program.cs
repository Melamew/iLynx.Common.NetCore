//using iLynx.UI.OpenGL;

using SFML.Window;
using Window = iLynx.UI.Sfml.Window;

namespace iLynx.UI.TestBench
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var window = new Window(1280, 720, "Test");
            window.Show();
            //window.Closed += (s, e) => { window.Close(); };
            //while (window.IsOpen)
            //{
            //    window.DispatchEvents();
            //    window.Display();
            //}
        }
    }
}
