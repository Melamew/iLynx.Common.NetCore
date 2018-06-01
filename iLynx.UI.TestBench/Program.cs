//using iLynx.UI.OpenGL;

using System;
using System.Collections.Generic;
using System.Linq;
using iLynx.Common;
using SFML.Graphics;
using Window = iLynx.UI.Sfml.Window;

namespace iLynx.UI.TestBench
{
    public class Foo : BindingSource
    {
        private string a;

        public string A
        {
            get => a;
            set
            {
                if (value == a) return;
                var oldValue = a;
                a = value;
                OnPropertyChanged(oldValue, value);
            }
        }
    }

    public class Bar : BindingSource
    {
        private string b;

        public string B
        {
            get => b;
            set
            {
                if (value == b) return;
                var oldValue = b;
                b = value;
                OnPropertyChanged(oldValue, value);
            }
        }
    }

    public static class Program
    {
        static void Main(string[] args)
        {
            //var foo = new Foo();
            //var bar = new Bar();
            //var binding = new MultiBinding<string>()
            //    .Bind(foo, nameof(Foo.A))
            //    .Bind(bar, nameof(Bar.B));
            //var foos = new List<Foo>();
            //for (var i = 0; i < 10; ++i)
            //    foos.Add(new Foo());
            //foos.Aggregate(binding, (b, x) => b.Bind(x, nameof(Foo.A)));
            //binding.Bind(bar, "B");
            //Console.WriteLine($"A: {foo.A}");
            //foo.A = "Something";
            //Console.WriteLine($"A: {foo.A}");
            //Console.WriteLine($"B: {bar.B}");
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
