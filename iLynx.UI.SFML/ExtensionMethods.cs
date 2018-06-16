using System;
using System.Collections.Generic;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

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

        public static Vector2f Size(this FloatRect src)
        {
            return new Vector2f(src.Width, src.Height);
        }

        public static FloatRect Translate(this FloatRect rect, Vector2f distance)
        {
            return new FloatRect(rect.Left + distance.X, rect.Top + distance.Y, rect.Width, rect.Height);
        }

        public static Vector2f Position(this FloatRect rect)
        {
            return new Vector2f(rect.Left, rect.Top);
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }

        public static Vector2f Scale(this Vector2f vector, Vector2f scalar)
        {
            return new Vector2f(vector.X * scalar.X, vector.Y * scalar.Y);
        }

        //public static Transform Translate(this Transform transform, Vector2f distance)
        //{
        //    transform.Translate(distance);
        //    return transform;
        //}
    }

    public static class InputHandler
    {
        public static event EventHandler<MouseMoveEventArgs> MouseMove;

        public static void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            MouseMove?.Invoke(sender, e);
        }

        public static event EventHandler<MouseButtonEventArgs> MouseDown;

        public static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown?.Invoke(sender, e);
        }

        public static event EventHandler<MouseButtonEventArgs> MouseUp;

        public static void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUp?.Invoke(sender, e);
        }

        public static event EventHandler<KeyEventArgs> KeyDown;

        public static void OnKeyDown(object sender, KeyEventArgs e)
        {
            KeyDown?.Invoke(sender, e);
        }

        public static event EventHandler<KeyEventArgs> KeyUp;

        public static void OnKeyUp(object sender, KeyEventArgs e)
        {
            KeyUp?.Invoke(sender, e);
        }
    }
}