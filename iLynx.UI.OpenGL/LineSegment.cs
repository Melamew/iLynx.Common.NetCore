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

using System.Runtime.InteropServices;
using iLynx.Common.Maths;
using OpenTK;

namespace iLynx.UI.OpenGL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LineSegment
    {
        public Vector2 P1;
        public Vector2 P2;

        public LineSegment(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public LineSegment(float x1, float y1, float x2, float y2)
        {
            P1 = new Vector2(x1, y1);
            P2 = new Vector2(x2, y2);
        }

        public LineSegment(float x1, float y1, Vector2 p2)
        {
            P1 = new Vector2(x1, y1);
            P2 = p2;
        }

        public LineSegment(Vector2 p1, float x2, float y2)
        {
            P1 = p1;
            P2 = new Vector2(x2, y2);
        }

        public Intersect Intersects(LineSegment other)
        {
            return Math2D.AreIntersecting(P1.X, P1.Y, P2.X, P2.Y, other.P1.X, other.P1.Y, other.P2.X, other.P2.Y);
        }
    }
}