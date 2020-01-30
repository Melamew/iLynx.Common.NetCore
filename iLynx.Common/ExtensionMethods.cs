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

using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets a string representation of the specified <see name="IEnumerable{byte}"/> using the specified <paramref name="splitter"/> as a "splitter"
        /// </summary>
        /// <param name="val">The <see cref="IEnumerable{T}"/> to stringify</param>
        /// <param name="splitter">The splitter to use between bytes</param>
        /// <returns></returns>
        public static string ToString(this IEnumerable<byte> val, string splitter)
        {
            var ret = val.Aggregate(string.Empty, (current, v) => current + (v.ToString("X2") + splitter));
            if (ret.Length > 1)
                // Remove the superfluous splitter that was added during the aggregate.
                ret = ret.Remove(ret.Length - splitter.Length, splitter.Length);
            return ret;
        }

        /// <summary>
        /// Combines to string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string CombineToString<T>(this IEnumerable<T> val)
        {
            return val.Aggregate(string.Empty, (s, arg2) => s + arg2.ToString());
        }

        public static string ToString<T>(this IEnumerable<T> val, string splitter)
        {
            var ret = val.Aggregate(string.Empty, (current, v) => current + v.ToString() + splitter);
            if (ret.Length > 1)
                // Remove the superfluous splitter that was added during the aggregate.
                ret = ret.Remove(ret.Length - splitter.Length, splitter.Length);
            return ret;
        }

        /// <summary>
        /// Determines whether the specified value is within the specified range (Inclusively!).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns>
        ///   <c>true</c> if [is in range] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInRange(this short value, short min, short max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Determines whether the specified value is within the specified range (Inclusively!).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns>
        ///   <c>true</c> if [is in range] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Determines whether the specified value is within the specified range (Inclusively!).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns>
        ///   <c>true</c> if [is in range] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInRange(this long value, long min, long max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Determines whether the specified value is within the specified range (Inclusively!).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns>
        ///   <c>true</c> if [is in range] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInRange(this uint value, uint min, uint max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Determines whether the specified value is within the specified range (Inclusively!).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns>
        ///   <c>true</c> if [is in range] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInRange(this ushort value, ushort min, ushort max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Determines whether the specified value is within the specified range (Inclusively!).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns>
        ///   <c>true</c> if [is in range] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInRange(this ulong value, ulong min, ulong max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Determines whether [is power of two] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is power of two] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPowerOfTwo(this uint value)
        {
            return (value != 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Determines whether [is power of two] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is power of two] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPowerOfTwo(this ulong value)
        {
            return (value != 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Determines whether [is power of two] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is power of two] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPowerOfTwo(this ushort value)
        {
            return (value != 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Determines whether [is power of two] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is power of two] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPowerOfTwo(this int value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Determines whether [is power of two] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is power of two] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPowerOfTwo(this long value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Determines whether [is power of two] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is power of two] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPowerOfTwo(this short value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }
    }
}
