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
using static System.Math;

namespace iLynx.UI.Sfml.Animation
{

    public static class EasingFunctions
    {
        public static double Linear(double t)
        {
            return t;
        }

        #region These functions are based on samples provided by https://gist.github.com/lindell at https://gist.github.com/gre/1650294

        public static double EaseIn(double t, double power)
        {
            return Pow(t, power);
        }

        public static double EaseOut(double t, double power)
        {
            return 1d - Abs(Pow(t - 1d, power));
        }

        public static double EaseInOut(double t, double power)
        {
            return t < .5d ? EaseIn(t * 2d, power) / 2d : EaseOut(t * 2d - 1d, power) / 2d + .5d;
        }

        #endregion

        public static double QuadraticIn(double t)
        {
            return EaseIn(t, 2d);
        }

        public static double QuadraticOut(double t)
        {
            return EaseOut(t, 2d);
        }

        public static double QuadraticInOut(double t)
        {
            return EaseInOut(t, 2d);
        }

        public static double CubicIn(double t)
        {
            return EaseIn(t, 3d);
        }

        public static double CubicOut(double t)
        {
            return EaseOut(t, 3d);
        }

        public static double CubicInOut(double t)
        {
            return EaseInOut(t, 3d);
        }
        //public static double QuadraticIn(double t)
        //{
        //    return t * t;
        //}

        //public static double QuadraticOut(double t)
        //{
        //    return t * (2 - t);
        //}

        //public static double QuadraticInOut(double t)
        //{
        //    return t > .5d ? -1d + (4d - 2d * t) * t : 2d * t * t;
        //}

        //public static double CubicIn(double t)
        //{
        //    return t * t * t;
        //}

        //public static double CubicOut(double t)
        //{
        //    return (t - 1d) * t * t + 1d;
        //}

        //public static double CubicInOut(double t)
        //{
        //    return t < .5d ? 4d * t * t * t : (t - 1d) * (2d * t - 2d) * (2d * t - 2d) + 1d;
        //}
    }
}