using System;
using System.Runtime.InteropServices;
using iLynx.Graphics.Rendering;
using OpenTK;

namespace iLynx.Graphics.TestBench
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"SizeOf<Vertex2>(): {Marshal.SizeOf<Vertex2>()}");
            Console.WriteLine($"SizeOf<Color>(): {Marshal.SizeOf<Color>()}");
            Console.WriteLine($"SizeOf<Vector2>(): {Marshal.SizeOf<Vector2>()}");
            Console.WriteLine($"SizeOf<Color>() + SizeOf<Vector2>() * 2: {Marshal.SizeOf<Color>() + Marshal.SizeOf<Vector2>() * 2}");
            Console.ReadKey();
        }
    }
}
