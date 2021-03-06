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
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace iLynx.Common
{
    public abstract class BindingSource : IBindingSource
    {
        private readonly Dictionary<string, List<Delegate>> subscribers =
            new Dictionary<string, List<Delegate>>();

        public void AddPropertyChangedHandler<TValue>(string valueName, ValueChangedCallback<TValue> callback)
        {
            if (!subscribers.TryGetValue(valueName, out var list))
            {
                list = new List<Delegate>();
                subscribers.Add(valueName, list);
            }
            if (list.Contains(callback)) return;
            list.Add(callback);
        }

        public void RemovePropertyChangedHandler<TValue>(string valueName, ValueChangedCallback<TValue> callback)
        {
            if (subscribers.TryGetValue(valueName, out var list))
                list.Remove(callback);
            if (list?.Count == 0)
                subscribers.Remove(valueName);
        }

        protected virtual void OnPropertyChanged<TValue>(TValue oldValue, TValue newValue, [CallerMemberName]string propertyName = null)
        {
            if (null == propertyName) throw new ArgumentNullException(nameof(propertyName));
            if (!subscribers.TryGetValue(propertyName, out var handlers)) return;
            var e = new ValueChangedEventArgs<TValue>(oldValue, newValue);
            foreach (var handler in handlers.ToArray())
                handler?.DynamicInvoke(this, e);
        }
    }
}