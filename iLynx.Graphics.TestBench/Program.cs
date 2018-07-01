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
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using iLynx.Graphics.Geometry;
using iLynx.Graphics.Rendering;
using iLynx.Graphics.Rendering.Geometry;
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
            //Console.WriteLine($@"{typeof(Vertex2)
            //    .GetFields(BindingFlags.Instance | BindingFlags.Public)
            //    .Aggregate("", (s, f) => s += $" {f.Name}")}");
            Console.ReadKey();
        }
    }
}
