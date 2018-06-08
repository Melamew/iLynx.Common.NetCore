//using iLynx.UI.OpenGL;

using System;
using System.Collections.Generic;
using System.Linq;
using iLynx.Common;
using iLynx.UI.SFML.Controls;
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
            var window = new Window(1280, 720, "Test");
            window.AddChild(new Button());
            window.Show();
            //window.
        }
    }
}
