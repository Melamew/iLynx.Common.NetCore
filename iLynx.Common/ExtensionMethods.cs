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

        #region 16 Bits

        /// <summary>
        /// Swaps the endianness.
        /// </summary>
        /// <param name="array">The array.</param>
        public static void SwapEndianness(this short[] array)
        {
            unsafe
            {
                fixed (void* arr = array)
                    Swap16((byte*)arr, 0, array.Length);
            }
        }

        public static short SwapEndianness(this short value)
        {
            unsafe
            {
                Swap16((byte*)&value, 0, 1);
            }
            return value;
        }

        public static ushort SwapEndianness(this ushort value)
        {
            unsafe
            {
                Swap16((byte*)&value, 0, 1);
            }
            return value;
        }

        /// <summary>
        /// Swaps the endianness.
        /// </summary>
        /// <param name="array">The array.</param>
        public static void SwapEndianness(this ushort[] array)
        {
            unsafe
            {
                fixed (void* arr = array)
                    Swap16((byte*)arr, 0, array.Length);
            }
        }

        /// <summary>
        /// Swaps the endianness of a 16 bit integer at the specified byte offset.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">offset</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">count</exception>
        public static void SwapInt16Endianness(this byte[] src, int offset = 0, int count = 1)
        {
            if (offset >= src.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + (count * 2) >= src.Length) throw new ArgumentOutOfRangeException(nameof(count));
            unsafe
            {
                fixed (byte* arr = src)
                    Swap16(arr, offset, count);
            }
        }

        /// <summary>
        /// Swaps the endianness of 16 bit integers starting at the specified byte offset, processing <param name="count"/> full 16 bit integers (Bytes X 2).
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void Swap16(byte* src, int offset, int count)
        {
            var p = src + offset;
            while (count-- > 0)
            {
                var a = *p;
                ++p;
                *(p - 1) = *p;
                *p = a;
            }
        }
        #endregion

        #region 32 Bits
        /// <summary>
        /// Swaps the endianness.
        /// </summary>
        /// <param name="array">The array.</param>
        public static void SwapEndianness(this int[] array)
        {
            unsafe
            {
                fixed (void* arr = array)
                    Swap32((byte*)arr, 0, array.Length);
            }
        }

        /// <summary>
        /// Swaps the endianness.
        /// </summary>
        /// <param name="array">The array.</param>
        public static void SwapEndianness(this uint[] array)
        {
            unsafe
            {
                fixed (void* arr = array)
                    Swap32((byte*)arr, 0, array.Length);
            }
        }

        public static int SwapEndianness(this int value)
        {
            unsafe
            {
                Swap32((byte*)&value, 0, 1);
            }
            return value;
        }

        public static uint SwapEndianness(this uint value)
        {
            unsafe
            {
                Swap32((byte*)&value, 0, 1);
            }
            return value;
        }

        /// <summary>
        /// Swaps the endianness of a 32 bit integer at the specified byte offset.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The number of integers to swap.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">offset</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">count</exception>
        public static void SwapInt32Endianness(this byte[] src, int offset = 0, int count = 1)
        {
            if (offset >= src.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + count * 4 >= src.Length) throw new ArgumentOutOfRangeException(nameof(count));
            unsafe
            {
                fixed (byte* arr = src)
                    Swap32(arr, offset, count);
            }
        }

        /// <summary>
        /// Swaps the endianness of 32 bit integers starting at the specified byte offset, processing <param name="count"/> full 32 bit integers (Bytes X 4).
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void Swap32(byte* src, int offset, int count)
        {
            var p = src + offset;
            while (count-- > 0)
            {
                var a = *p;
                ++p;
                var b = *p;
                *p = *(p + 1);
                ++p;
                *p = b;
                ++p;
                *(p - 3) = *p;
                *p = a;
            }
        }
        #endregion

        #region 64 Bits

        /// <summary>
        /// Swaps the endianness.
        /// </summary>
        /// <param name="array">The array.</param>
        public static void SwapEndianness(this long[] array)
        {
            unsafe
            {
                fixed (void* arr = array)
                    Swap64((byte*)arr, 0, array.Length);
            }
        }

        /// <summary>
        /// Swaps the endianness.
        /// </summary>
        /// <param name="array">The array.</param>
        public static void SwapEndianness(this ulong[] array)
        {
            unsafe
            {
                fixed (void* arr = array)
                    Swap64((byte*)arr, 0, array.Length);
            }
        }

        public static long SwapEndianness(this long value)
        {
            unsafe
            {
                Swap64((byte*)&value, 0, 1);
            }
            return value;
        }

        public static ulong SwapEndianness(this ulong value)
        {
            unsafe
            {
                Swap64((byte*)&value, 0, 1);
            }
            return value;
        }

        /// <summary>
        /// Swaps the endianness of a 64 bit integer at the specified byte offset.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">How many integers to swap.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">offset</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">count</exception>
        public static void SwapInt64Endianness(this byte[] src, int offset = 0, int count = 1)
        {
            if (offset >= src.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + count * 8 >= src.Length) throw new ArgumentOutOfRangeException(nameof(count));
            unsafe
            {
                fixed (byte* arr = src)
                    Swap64(arr, offset, count);
            }
        }

        /// <summary>
        /// Swaps the endianness of 64 bit integers starting at the specified byte offset, processing <param name="count"/> full 64 bit integers (Bytes X 8).
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public static unsafe void Swap64(byte* src, int offset, int count)
        {
            var p = src + offset;
            while (count-- > 0)
            {
                var a = *p;
                var b = *(p + 1);
                *p = *(p + 7);
                *(p + 7) = a;
                ++p;
                *p = *(p + 5);
                *(p + 5) = b;
                ++p;
                a = *p;
                b = *(p + 1);
                *p = *(p + 3);
                *(p + 3) = a;
                ++p;
                *p = *(p + 1);
                *(p + 1) = b;
            }
        }
        #endregion
    }
}
