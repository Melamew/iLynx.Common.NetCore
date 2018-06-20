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
using System.Diagnostics;
using System.Linq;

namespace iLynx.Common
{
    public class MultiBinding<TValue> : IBinding<TValue>
    {
        private readonly Dictionary<object, PropertyWrapper<TValue>> targets = new Dictionary<object, PropertyWrapper<TValue>>();
        private bool changing = false;

        public IBinding<TValue> Bind<TTarget>(TTarget target, string propertyName) where TTarget : IBindingSource
        {
            if (targets.ContainsKey(target)) return this;
            var wrapper = PropertyWrapper<TValue>.Create(target, propertyName); //new PropertyWrapper<TValue>(target, propertyName);
            targets.Add(target, wrapper);
            target.AddPropertyChangedHandler<TValue>(propertyName, OnPropertyChanged);
            return this;
        }

        private void OnPropertyChanged(object source, ValueChangedEventArgs<TValue> e)
        {
            if (changing) return;
            changing = true;
            foreach (var target in targets.Where(x => x.Key != source))
                target.Value.SetValue(e.NewValue);
            changing = false;
        }

        public IBinding<TValue> Unbind<TTarget>(TTarget target) where TTarget : IBindingSource
        {
            if (targets.Remove(target, out var propertyWrapper))
                target.RemovePropertyChangedHandler<TValue>(propertyWrapper.PropertyName, OnPropertyChanged);
            return this;
        }

        public void SetValue(TValue value)
        {
            foreach (var target in targets.Values)
                target.SetValue(value);
        }

        public TValue GetValue()
        {
            var first = targets.Values.FirstOrDefault();
            return null == first ? default(TValue) : first.GetValue();
        }
    }
}