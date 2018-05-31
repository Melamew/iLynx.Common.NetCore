//using iLynx.UI.OpenGL;

using System;
using iLynx.Common;
using SFML.Window;
using Window = iLynx.UI.Sfml.Window;

namespace iLynx.UI.TestBench
{
    public class Foo
    {
        public string A
        {
            get;
            set;
        }
    }

    public class Bar
    {
        public string B
        {
            get;
            set;
        }
    }

    public static class Program
    {
        static void Main(string[] args)
        {
            var foo = new Foo();
            var bar = new Bar();
            var binding = new TwoWayBinding<string>(foo, "A");
            binding.AddTarget(bar, "B");
            Console.WriteLine($"A: {foo.A}");
            //binding.SetValue("Something");
            foo.A = "Something";
            Console.WriteLine($"A: {foo.A}");
            Console.WriteLine($"B: {bar.B}");
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
