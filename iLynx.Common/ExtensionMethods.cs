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
using System.Linq;

namespace iLynx.Common
{
    public static class ExtensionMethods
    {
        public static void AddOrUpdateMany<TKey, TElement>(this IDictionary<TKey, ICollection<TElement>> target, TKey key, TElement value)
        {
            if (!target.TryGetValue(key, out var collection))
            {
                collection = new List<TElement>();
                target.Add(key, collection);
            }

            collection.Remove(value);
            collection.Add(value);
        }

        public static void AddOrUpdateMany<TKey, TElement>(this IDictionary<TKey, List<TElement>> target, TKey key, TElement value)
        {
            if (!target.TryGetValue(key, out var collection))
            {
                collection = new List<TElement>();
                target.Add(key, collection);
            }

            collection.Remove(value);
            collection.Add(value);
        }

        public static bool Remove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, out TValue value)
        {
            return dict.TryGetValue(key, out value) && dict.Remove(key);
        }

        /// <summary>
        /// Gets a string representation of the specified <see name="IEnumerable{byte}"/> using the specified <paramref name="splitter"/> as a "splitter"
        /// </summary>
        /// <param name="val">The <see cref="IEnumerable{T}"/> to stringify</param>
        /// <param name="splitter">The splitter to use between bytes</param>
        /// <returns></returns>
        public static string ToString(this IEnumerable<byte> val, string splitter)
        {
            var ret = val.Aggregate(string.Empty, (current, v) => current + (v.ToString("X2") + splitter));
            // Remove the superflous splitter that was added during the aggregate.
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

        /// <summary>
        /// Normalizes the specified arr (In place, and returns it).
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <param name="level">The value that the array is to be normalized against - IE. the value that each element in the array is divided by. If float.NaN is supplied, the maximum value in the array is calculated automatically</param>
        public static float[] Normalize(this float[] arr, float level = float.NaN)
        {
            if (null == arr) return null;
            // ReSharper disable LoopCanBeConvertedToQuery // Faster like this, ffs
            // ReSharper disable ForCanBeConvertedToForeach // Faster like this, ffs
            // TODO: Maybe find a way to make this go faster?
            if (float.IsNaN(level))
            {
                level = float.MinValue;
                for (var i = 0; i < arr.Length; ++i)
                    level = arr[i] > level ? arr[i] : level;
            }
            for (var i = 0; i < arr.Length; ++i)
                arr[i] /= level;
            return arr;
            // ReSharper restore ForCanBeConvertedToForeach
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        /// <summary>
        /// Transforms the specified arr (In place).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">The arr.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static void Transform<T>(this T[] arr, Func<T, T> func)
        {
            for (var i = 0; i < arr.Length; ++i)
                arr[i] = func(arr[i]);
        }

        /// <summary>
        /// Transforms the specified "sub region / slice" of the specified array (In place).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">The array to be "operated" on</param>
        /// <param name="func">The transform function to be run for each element</param>
        /// <param name="offset">The offset to from at</param>
        /// <param name="length">The number of elements to transform</param>
        public static void Transform<T>(this T[] arr, Func<T, T> func, int offset, int length)
        {
            if (offset + length > arr.Length) throw new IndexOutOfRangeException(nameof(offset));
            for (var i = offset; i < length; ++i)
                arr[i + offset] = func(arr[i + offset]);
        }

        /// <summary>
        /// Transforms the specified arr.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK">The type of the K.</typeparam>
        /// <param name="arr">The arr.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static TK[] TransformType<T, TK>(this T[] arr, Func<T, TK> func)
        {
            var res = new TK[arr.Length];
            for (var i = 0; i < arr.Length; ++i)
                res[i] = func(arr[i]);
            return res;
        }

        public static T[] Slice<T>(this T[] arr,
            int offset,
            int length)
        {
            if (offset + length > arr.Length) throw new ArgumentOutOfRangeException(nameof(length));
            var result = new T[length];
            Array.Copy(arr, offset, result, 0, length);
            return result;
        }

        /// <summary>
        /// Normalizes the specified arr.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <param name="level"></param>
        public static void Normalize(this double[] arr, double level = double.NaN)
        {
            // ReSharper disable LoopCanBeConvertedToQuery // Faster like this, ffs
            // ReSharper disable ForCanBeConvertedToForeach // Faster like this, ffs
            // TODO: Maybe find a way to make this go faster?
            if (double.IsNaN(level))
            {
                level = double.MinValue;
                for (var i = 0; i < arr.Length; ++i)
                    level = arr[i] > level ? arr[i] : level;
            }
            for (var i = 0; i < arr.Length; ++i)
                arr[i] /= level;
            // ReSharper restore ForCanBeConvertedToForeach
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        /// <summary>
        /// Repeats the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="times">The times.</param>
        /// <returns></returns>
        public static T[] Repeat<T>(this T obj, int times)
        {
            var res = new T[times];
            for (var i = 0; i < times; ++i)
                res[i] = obj;
            return res;
        }

        /// <summary>
        /// Repeats the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="times">The times.</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Repeat(this string str, int times, string separator = "")
        {
            var res = string.Empty;
            while (times-- > 0)
                res += str + separator;
            // Remove trailing separator
            return res.Remove(res.Length - separator.Length);
        }

        public static int[] To(this int @from, int to)
        {
            var result = new int[to - @from + 1];
            for (var i = from; i <= to; ++i)
                result[i - from] = i;
            return result;
        }

        //public static T[] Repeat<T>(this T source, Func<int, T> incrementor, int times)
        //{
        //    var result = new T[times];
        //    for (var i = 0; i < times; ++i)
        //        result[i] = incrementor(i);
        //    return result;
        //}
    }
}