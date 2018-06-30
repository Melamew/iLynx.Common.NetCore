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

namespace iLynx.Graphics.Maths
{
    public static class Math2D
    {
        // Thanks to Mecki at https://stackoverflow.com/questions/217578/how-can-i-determine-whether-a-2d-point-is-within-a-polygon
        public static Intersect AreIntersecting(LineSegment line1, LineSegment line2)
        {
            // Convert vector 1 to a line (line 1) of infinite length.
            // We want the line in linear equation standard form: A*x + B*y + C = 0
            // See: http://en.wikipedia.org/wiki/Linear_equation
            float v1X1 = line1.P1.X,
                v1Y1 = line1.P1.Y,
                v1X2 = line1.P2.X,
                v1Y2 = line1.P2.Y,
                v2X1 = line2.P1.X,
                v2Y1 = line2.P1.Y,
                v2X2 = line2.P2.X,
                v2Y2 = line2.P2.Y;

            var a1 = v1Y2 - v1Y1;
            var b1 = v1X1 - v1X2;
            var c1 = (v1X2 * v1Y1) - (v1X1 * v1Y2);

            // Every point (x,y), that solves the equation above, is on the line,
            // every point that does not solve it, is not. The equation will have a
            // positive result if it is on one side of the line and a negative one 
            // if is on the other side of it. We insert (x1,y1) and (x2,y2) of vector
            // 2 into the equation above.
            var d1 = (a1 * v2X1) + (b1 * v2Y1) + c1;
            var d2 = (a1 * v2X2) + (b1 * v2Y2) + c1;

            // If d1 and d2 both have the same sign, they are both on the same side
            // of our line 1 and in that case no intersection is possible. Careful, 
            // 0 is a special case, that's why we don't test ">=" and "<=", 
            // but "<" and ">".
            if (d1 > 0 && d2 > 0) return Intersect.No;
            if (d1 < 0 && d2 < 0) return Intersect.No;

            // The fact that vector 2 intersected the infinite line 1 above doesn't 
            // mean it also intersects the vector 1. Vector 1 is only a subset of that
            // infinite line 1, so it may have intersected that line before the vector
            // started or after it ended. To know for sure, we have to repeat the
            // the same test the other way round. We start by calculating the 
            // infinite line 2 in linear equation standard form.
            var a2 = v2Y2 - v2Y1;
            var b2 = v2X1 - v2X2;
            var c2 = (v2X2 * v2Y1) - (v2X1 * v2Y2);

            // Calculate d1 and d2 again, this time using points of vector 1.
            d1 = (a2 * v1X1) + (b2 * v1Y1) + c2;
            d2 = (a2 * v1X2) + (b2 * v1Y2) + c2;

            // Again, if both have the same sign (and neither one is 0),
            // no intersection is possible.
            if (d1 > 0 && d2 > 0) return Intersect.No;
            if (d1 < 0 && d2 < 0) return Intersect.No;

            // If we get here, only two possibilities are left. Either the two
            // vectors intersect in exactly one point or they are collinear, which
            // means they intersect in any number of points from zero to infinite.
            if (Math.Abs((a1 * b2) - (a2 * b1)) < 0.01) return Intersect.Collinear;

            // If they are not collinear, they must intersect in exactly one point.
            return Intersect.Yes;
        }
    }
}
