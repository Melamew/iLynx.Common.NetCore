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

namespace iLynx.Common
{
    public static class ExtensionMethods
    {
        public static IBinding<T> Bind<TTarget, T>(this IBindingSource source, params (TTarget target, string propertyName)[] targets) where TTarget : IBindingSource
        {
            return targets.Aggregate(new MultiBinding<T>(),
                (binding, tuple) => (MultiBinding<T>)binding.Bind(tuple.target, tuple.propertyName));
        }

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
    }
}