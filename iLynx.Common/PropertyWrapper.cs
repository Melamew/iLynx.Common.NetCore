﻿#region LICENSE
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
using System.Reflection;

namespace iLynx.Common
{
    public class PropertyWrapper<TValue>
    {
        private GetMethod getter;
        private SetMethod setter;

        private delegate TValue GetMethod();
        private delegate void SetMethod(TValue value);

        private PropertyWrapper(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Creates a <see cref="PropertyWrapper{TValue}"/> for the specified property (<paramref name="propertyName"/>) from the instance of the specified object (<paramref name="targetInstance"/>)
        /// </summary>
        /// <typeparam name="TSource">The type of the property</typeparam>
        /// <param name="targetInstance">The object to create a <see cref="PropertyWrapper{TValue}"/> for</param>
        /// <param name="propertyName">The name of the property to wrap</param>
        /// <returns></returns>
        public static PropertyWrapper<TValue> Create<TSource>(TSource targetInstance, string propertyName)
            where TSource : IBindingSource
        {
            var sourceType = targetInstance?.GetType() ?? throw new ArgumentNullException(nameof(targetInstance));
            var sourceProperty = sourceType.GetProperty(propertyName) ??
                                 throw new InvalidBindingTypeException(sourceType, propertyName);
            return Create(targetInstance, sourceProperty);
        }

        /// <inheritdoc cref="Create{TSource}(TSource,string)"/>
        /// <param name="targetInstance"><inheritdoc cref="targetInstance"/></param>
        /// <param name="property">The <see cref="PropertyInfo"/> to use when creating a new instance of <see cref="PropertyWrapper{TValue}"/>
        /// <remarks>The <see cref="PropertyWrapper{TValue}"/> will be created with getter and setter methods extracted from this <see cref="PropertyInfo"/></remarks>
        /// </param>
        public static PropertyWrapper<TValue> Create<TSource>(TSource targetInstance, PropertyInfo property)
            where TSource : IBindingSource
        {
            var result = new PropertyWrapper<TValue>(property.Name);
            result.CreateDelegates(targetInstance, property);
            return result;
        }

        private void CreateDelegates(object targetInstance, PropertyInfo property)
        {
            if (property.PropertyType != typeof(TValue)) throw new InvalidBindingTypeException(typeof(TValue).DeclaringType, property.Name);
            var getMethod = property.GetGetMethod();
            var setMethod = property.GetSetMethod();
            getter = (GetMethod)getMethod.CreateDelegate(typeof(GetMethod), targetInstance);
            setter = (SetMethod)setMethod.CreateDelegate(typeof(SetMethod), targetInstance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(TValue value)
        {
            setter(value);
        }

        public TValue GetValue()
        {
            return getter();
        }

        public string PropertyName { get; }
    }
}