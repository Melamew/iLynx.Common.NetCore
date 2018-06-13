using System;
using SFML.Graphics;

namespace iLynx.UI.Sfml
{
    public static class ExtensionMethods
    {
        public static bool Intersects(this IntRect src, FloatRect rect, out FloatRect overlap)
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
                overlap.Left = innerLeft;
                overlap.Top = innerTop;
                overlap.Width = innerRight - innerLeft;
                overlap.Height = innerBottom - innerTop;
                return true;
            }
            overlap.Left = 0;
            overlap.Top = 0;
            overlap.Height = 0;
            overlap.Width = 0;
            return false;
        }

        public static bool Intersects(this IntRect src, FloatRect rect)
        {
            return src.Intersects(rect, out _);
        }

        public static bool Intersects(this FloatRect src, IntRect rect, out FloatRect overlap)
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
                overlap.Left = innerLeft;
                overlap.Top = innerTop;
                overlap.Width = innerRight - innerLeft;
                overlap.Height = innerBottom - innerTop;
                return true;
            }
            overlap.Left = 0;
            overlap.Top = 0;
            overlap.Height = 0;
            overlap.Width = 0;
            return false;
        }

        public static bool Intersects(this FloatRect src, IntRect rect)
        {
            return src.Intersects(rect, out _);
        }
    }
}