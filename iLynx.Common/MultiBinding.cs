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