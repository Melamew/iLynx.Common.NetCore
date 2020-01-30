using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using iLynx.Common.Collections;

namespace iLynx.Common.Collections
{
    /// <summary>
    /// ExtensionMethods
    /// </summary>
    public static class ExtensionMethods
    {

    }
}

namespace System.Collections.Generic
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Wraps an asynchronous enumerable source around this IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="maxPreBuffered">The max pre buffered.</param>
        /// <returns></returns>
        public static IEnumerable<T> WrapEnumerable<T>(this IEnumerable<T> source, int maxPreBuffered = 1)
        {
            return new AsynchronousEnumerableWrapper<T>(source, maxPreBuffered);
        }

        /// <summary>
        /// Wraps an asynchronous queryable source around this IQueryable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="maxPrebuffered">The maximum number of pre-buffered items.</param>
        /// <returns></returns>
        public static IQueryable<T> WrapQueryableAsync<T>(this IQueryable<T> source, int maxPrebuffered)
        {
            return new AsynchronourQueryableWrapper<T>(source, maxPrebuffered);
        }

        public static void AddRange<TKey, TValue>(this SortedList<TKey, TValue> target, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (var item in items)
                target.Add(item.Key, item.Value);
        }

        /// <inheritdoc cref="Add{TKey,TElement}(IDictionary{TKey, ICollection{TElement}}, TKey, TElement)"/>
        public static void Add<TKey, TElement>(this IDictionary<TKey, List<TElement>> target, TKey key, TElement value)
        {
            if (!target.TryGetValue(key, out var collection))
            {
                collection = new List<TElement>();
                target.Add(key, collection);
            }

            if (collection.Contains(value)) return;
            collection.Add(value);
        }

        /// <summary>
        /// Adds the specified value to the list in the dictionary <paramref name="target"/>
        /// <para/>
        /// If there is no list in the collection for the key <paramref name="key"/> a new list will be created and added to the dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="target">The dictionary to add the item to</param>
        /// <param name="key">The key to look for a list at</param>
        /// <param name="value">The value to be added</param>
        public static void Add<TKey, TElement>(this IDictionary<TKey, ICollection<TElement>> target, TKey key, TElement value)
        {
            if (!target.TryGetValue(key, out var collection))
            {
                collection = new List<TElement>();
                target.Add(key, collection);
            }

            if (collection.Contains(value)) return;
            collection.Add(value);
        }

        /// <summary>
        /// Removes the value at the specified key in the specified dictionary
        /// </summary>
        /// <typeparam name="TKey">The key type of the dictionary</typeparam>
        /// <typeparam name="TValue">The value type of the dictionary</typeparam>
        /// <param name="dict">The dictionary to perform the operation on</param>
        /// <param name="key">The key to remove the value for</param>
        /// <param name="value">If the operation succeeded, this will be set to the removed value</param>
        /// <returns>True if the value was removed, otherwise false</returns>
        public static bool Remove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, out TValue value)
        {
            return dict.TryGetValue(key, out value) && dict.Remove(key);
        }

        /// <summary>
        /// Adds the specified range of values to the specified <see cref="ObservableCollection{T}"/>
        /// <remarks>
        /// NOTE: This method is equivalent to manually adding each item individually in a loop
        /// </remarks>
        /// </summary>
        /// <typeparam name="T">The element type of the collection</typeparam>
        /// <param name="target">The collection to perform the operation on</param>
        /// <param name="items">The items to add</param>
        public static void AddRange<T>(this ObservableCollection<T> target, IEnumerable<T> items)
        {
            foreach (var item in items)
                target.Add(item);
        }

        /// <summary>
        /// Swaps the planes of the specified 2 dimensional array
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="input">The array to perform the operation on</param>
        /// <returns>A new 2 dimensional array where dimensions are x=y, y=x (swapped), populated with the corresponding values from the input array</returns>
        public static T[,] SwapPlanes<T>(this T[,] input)
        {
            var yDim = input.GetLength(1);
            var xDim = input.GetLength(0);
            var output = new T[input.GetLength(1), input.GetLength(0)];
            for (var y = 0; y < yDim; ++y)
            {
                for (var x = 0; x < xDim; ++x)
                {
                    output[y, x] = input[x, y];
                }
            }
            return output;
        }

        /// <summary>
        /// Normalizes all values in the array to the specified level.
        /// If level is <see cref="Double.NaN"/> the maximum value will first be computed and then used as the level.
        /// </summary>
        /// <param name="arr">The array to normalize</param>
        /// <param name="level">The level / value to normalize to</param>
        public static void Normalize(this double[] arr, double level = double.NaN)
        {
            // ReSharper disable LoopCanBeConvertedToQuery // Faster like this, ffs
            // ReSharper disable ForCanBeConvertedToForeach // Faster like this, ffs
            // TODO: Maybe find a way to make this go faster?
            if (double.IsNaN(level))
            {
                // Find the maximum value in the array
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
        /// Normalizes all values in the array to the specified level.
        /// If level is <see cref="float.NaN"/> the maximum value will first be computed and then used as the level.
        /// </summary>
        /// <param name="arr">The array to normalize</param>
        /// <param name="level">The level / value to normalize to</param>
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
        /// Transforms the specified arr (In place) and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">The arr.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static T[] Transform<T>(this T[] arr, Func<T, T> func)
        {
            for (var i = 0; i < arr.Length; ++i)
                arr[i] = func(arr[i]);
            return arr;
        }

        /// <summary>
        /// Transforms the specified arr.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK">The type of the K.</typeparam>
        /// <param name="arr">The arr.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static TK[] Transform<T, TK>(this T[] arr, Func<T, TK> func)
        {
            var res = new TK[arr.Length];
            for (var i = 0; i < arr.Length; ++i)
                res[i] = func(arr[i]);
            return res;
        }

        /// <summary>
        /// Creates a new array containing a slice of value from the source array between index <paramref name="offset"/> and "<paramref name="offset"/> + <paramref name="length"/>"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">The source array</param>
        /// <param name="offset">The offset to copy values from</param>
        /// <param name="length">The number of values to copy</param>
        /// <returns></returns>
        public static T[] Slice<T>(this T[] arr,
                                   int offset,
                                   int length)
        {
            if (offset + length > arr.Length) throw new ArgumentOutOfRangeException(nameof(length));
            var result = new T[length];
            Array.Copy(arr, offset, result, 0, length);
            //for (var i = offset; i < offset + length; ++i)
            //    result[i - offset] = arr[i];
            return result;
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
        /// Applies the specified type transformation on the array.
        /// </summary>
        /// <typeparam name="T">The input type</typeparam>
        /// <typeparam name="TK">The result type.</typeparam>
        /// <param name="arr">The array to perform the operation on.</param>
        /// <param name="func">The transform function to be applied to all values.</param>
        /// <returns></returns>
        public static TK[] TransformType<T, TK>(this T[] arr, Func<T, TK> func)
        {
            var res = new TK[arr.Length];
            for (var i = 0; i < arr.Length; ++i)
                res[i] = func(arr[i]);
            return res;
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

        /// <inheritdoc cref="To(uint,uint)"/>
        public static int[] To(this int from, int to)
        {
            var range = to - from;
            var result = new int[Math.Abs(range) + 1];
            var sign = Math.Sign(range);
            for (var i = 0; i < result.Length; ++i)
                result[i] = sign * i + from;
            return result;
        }

        /// <summary>
        /// Creates an array of values ranging from the specified number to the specified number
        /// </summary>
        /// <param name="from">The first number in the desired array</param>
        /// <param name="to">The last number in the desired array</param>
        /// <returns></returns>
        public static uint[] To(this uint from, uint to)
        {
            var range = to - (int)from;
            var result = new uint[Math.Abs(range) + 1];
            var sign = (uint)Math.Sign(range);
            for (var i = 0u; i < result.Length; ++i)
                result[i] = sign * i + @from;
            return result;
        }
    }
}
