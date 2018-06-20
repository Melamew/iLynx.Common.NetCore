using System;
using System.Text;
using System.Threading;
using iLynx.UI.Sfml.Controls;
using SFML.Graphics;

namespace iLynx.UI.Sfml
{
    public static class ColorUtils
    {
        private static byte FromNormalized(this float value)
        {
            return (byte)(byte.MaxValue * value);
        }

        public static Color FromRgbA(float r, float g, float b, float a)
        {
            return new Color(r.FromNormalized(), g.FromNormalized(), b.FromNormalized(), a.FromNormalized());
        }

        public static Color FromHsvA(float h, float s, float v, float a)
        {
            if (h >= 360f)
                h = 0f;
            else
                h /= 60f;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (s == 0)
            {
                // achromatic (grey)
                return FromRgbA(v, v, v, a);
            }
            var i = (int) Math.Floor(h);
            var f = h - i; // factorial part of h
            var p = (v*(1f - s));
            var q = (v*(1f - s*f));
            var t = (v*(1f - s*(1f - f)));
            switch (i)
            {
                case 0:
                    return FromRgbA(v, t, p, a);
                case 1:
                    return FromRgbA(q, v, p, a);
                case 2:
                    return FromRgbA(p, v, t, a);
                case 3:
                    return FromRgbA(p, q, v, a);
                case 4:
                    return FromRgbA(t, p, v, a);
                default:
                    return FromRgbA(v, p, q, a);
            }
        }
    }

    public class StatisticsElement : ContentControl
    {
        private TimeSpan frameTime;
        private TimeSpan animationFrameTime;
        private TimeSpan layoutTime;
        private readonly StringBuilder builder = new StringBuilder();
        private readonly object syncObj = new object();

        public StatisticsElement()
        {
            Background = new Color(0, 0, 0, 128);
            Foreground = new Color(255, 255, 255, 255);
        }

        private void GenContent()
        {
            if (!Monitor.TryEnter(syncObj)) return;
            try
            {
                builder.Clear();
                builder.Append($"FrameTime: {frameTime.TotalMilliseconds:f2} ms\n");
                builder.Append($"Animation FrameTime: {animationFrameTime.TotalMilliseconds:f2} ms\n");
                builder.Append($"Layout Time: {layoutTime.TotalMilliseconds:f2} ms");
                ContentString = builder.ToString();
            }
            finally
            {
                Monitor.Exit(syncObj);
            }
        }

        public TimeSpan FrameTime
        {
            get => frameTime;
            set
            {
                if (value == frameTime) return;
                var old = frameTime;
                frameTime = value;
                OnPropertyChanged(old, value);
                GenContent();
            }
        }

        public TimeSpan AnimationFrameTime
        {
            get => animationFrameTime;
            set
            {
                if (value == animationFrameTime) return;
                var old = animationFrameTime;
                animationFrameTime = value;
                OnPropertyChanged(old, value);
                GenContent();
            }
        }

        public TimeSpan LayoutTime
        {
            get => layoutTime;
            set
            {
                if (value == layoutTime) return;
                var old = layoutTime;
                layoutTime = value;
                OnPropertyChanged(old, value);
                GenContent();
            }
        }
    }
}