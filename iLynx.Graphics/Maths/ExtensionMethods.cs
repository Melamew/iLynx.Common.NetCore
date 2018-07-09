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
using System.Collections.Generic;
using iLynx.Graphics.Drawing;
using OpenTK;

namespace iLynx.Graphics.Maths
{
    public static class ExtensionMethods
    {
        public static bool Intersects(this Rectangle src, RectangleF rect, out RectangleF overlap)
        {
            var right = src.Left + src.Width;
            var bottom = src.Top + src.Height;
            var rectPrecompRight = rect.Left + rect.Width;
            var rectPrecompBottom = rect.Top + rect.Height;
            var srcLeft = Math.Min(src.Left, right);
            var srcRight = Math.Max(src.Left, right);
            var srcTop = Math.Min(src.Top, bottom);
            var srcBottom = Math.Max(src.Top, bottom);
            var rectLeft = Math.Min(rect.Left, rectPrecompRight);
            var rectRight = Math.Max(rect.Left, rectPrecompRight);
            var rectTop = Math.Min(rect.Top, rectPrecompBottom);
            var rectBottom = Math.Max(rect.Top, rectPrecompBottom);

            var innerLeft = Math.Max(srcLeft, rectLeft);
            var innerRight = Math.Min(srcRight, rectRight);
            var innerTop = Math.Max(srcTop, rectTop);
            var innerBottom = Math.Min(srcBottom, rectBottom);
            if (innerLeft < innerRight && innerTop < innerBottom)
            {
                overlap = new RectangleF(innerLeft, innerTop, innerRight - innerLeft, innerBottom - innerTop);
                return true;
            }

            overlap = new RectangleF();
            return false;
        }

        public static bool Intersects(this Rectangle src, RectangleF rect)
        {
            return src.Intersects(rect, out _);
        }

        public static bool Intersects(this RectangleF src, Rectangle rect, out RectangleF overlap)
        {
            var right = src.Left + src.Width;
            var bottom = src.Top + src.Height;
            var rectPrecompRight = rect.Left + rect.Width;
            var rectPrecompBottom = rect.Top + rect.Height;
            var srcLeft = Math.Min(src.Left, right);
            var srcRight = Math.Max(src.Left, right);
            var srcTop = Math.Min(src.Top, bottom);
            var srcBottom = Math.Max(src.Top, bottom);
            var rectLeft = Math.Min(rect.Left, rectPrecompRight);
            var rectRight = Math.Max(rect.Left, rectPrecompRight);
            var rectTop = Math.Min(rect.Top, rectPrecompBottom);
            var rectBottom = Math.Max(rect.Top, rectPrecompBottom);

            var innerLeft = Math.Max(srcLeft, rectLeft);
            var innerRight = Math.Min(srcRight, rectRight);
            var innerTop = Math.Max(srcTop, rectTop);
            var innerBottom = Math.Min(srcBottom, rectBottom);
            if (innerLeft < innerRight && innerTop < innerBottom)
            {
                overlap = new RectangleF(innerLeft, innerTop, innerRight - innerLeft, innerBottom - innerTop);
                return true;
            }
            overlap = new RectangleF();
            return false;
        }

        public static bool Intersects(this RectangleF src, Rectangle rect)
        {
            return src.Intersects(rect, out _);
        }

        public static RectangleF Translate(this RectangleF rect, Vector2 distance)
        {
            return new RectangleF(rect.Left + distance.X, rect.Top + distance.Y, rect.Width, rect.Height);
        }

        public static SizeF Scale(this SizeF size, Vector2 scalar)
        {
            return new SizeF(size.Width * scalar.X, size.Height * scalar.Y);
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }

        public static Vector2 Scale(this Vector2 vector, Vector2 scalar)
        {
            return new Vector2(vector.X * scalar.X, vector.Y * scalar.Y);
        }

        public static bool HitTest(this IGeometry geometry, Vector2 point)
        {
            return false;
            //var boundingBox = s.GetLocalBounds();
            //if (!boundingBox.Contains(point.X, point.Y)) return false;
            //var ray = new LineSegment(boundingBox.Left - 10f, boundingBox.Top - 10f, point);
            //var count = s.GetPointCount();
            //if (count < 3) return false;
            //var intersections = 0;
            //for (uint i = 1; i < count; ++i)
            //{
            //    LineSegment line;
            //    line.P1 = s.GetPoint(i - 1);
            //    line.P2 = s.GetPoint(i);
            //    intersections += (int)line.Intersects(ray);
            //}

            //return (intersections & 1) == 1;
        }

        //public static IAnimation StartAnimation(this IUIElement element, Canvas canvas, Vector2 from,
        //    Vector2 to)
        //{

        //}
    }
}