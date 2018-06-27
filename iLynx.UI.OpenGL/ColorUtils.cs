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
using OpenTK;

namespace iLynx.UI.OpenGL
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
}