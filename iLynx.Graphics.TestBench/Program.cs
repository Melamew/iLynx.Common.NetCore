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
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK;

namespace iLynx.Graphics.TestBench
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"SizeOf<Vertex>(): {Marshal.SizeOf<Vertex>()}");
            var win = new MainWindow(1280, 720, "Test")
            {
                Location = new Point(0,0)
            };
            win.Run(60d);
            //var arr = new Vertex[1024];
            //RuinEverything(arr);
            //Console.ReadKey();
        }

        private static void RuinEverything(Vertex[] arr)
        {
            var rnd = new Random();
            var cumulativeSkipTake = new TimeSpan();
            var cumulativeArraySegment = new TimeSpan();
            var cumulativeArrayCopy = new TimeSpan();
            const int loopCount = 5000;
            for (var i = 0; i < loopCount; ++i)
            {
                var r1 = rnd.Next(0, arr.Length - 1);
                var r2 = rnd.Next(0, arr.Length - 1);
                var start = Math.Min(r1, r2);
                var length = Math.Max(r1, r2) + 1 - start;
                var sw = Stopwatch.StartNew();
                var result = arr.Skip(start).Take(length).ToArray();
                sw.Stop();
                cumulativeSkipTake += sw.Elapsed;
                var segment = new ArraySegment<Vertex>(arr, start, length);
                sw.Reset();
                sw.Start();
                result = segment.ToArray();
                sw.Stop();
                cumulativeArraySegment += sw.Elapsed;
                sw.Reset();
                sw.Start();
                result = new Vertex[length];
                Array.Copy(arr, start, result, 0, length);
                sw.Stop();
                cumulativeArrayCopy += sw.Elapsed;
                Console.CursorLeft = 0;
                Console.Write($"Loop: {i + 1} / {loopCount}");
            }

            Console.Write(Environment.NewLine);
            Console.WriteLine($"SkipTake:     {cumulativeSkipTake.TotalMilliseconds / loopCount} ms");
            Console.WriteLine($"ArraySegment: {cumulativeArraySegment.TotalMilliseconds / loopCount} ms");
            Console.WriteLine($"Array.Copy:   {cumulativeArrayCopy.TotalMilliseconds / loopCount} ms");
        }
    }
}
