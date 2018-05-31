using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace iLynx.Common
{
    public abstract class BindingSource : IBindingSource
    {
        private readonly Dictionary<string, List<dynamic>> subscribers =
            new Dictionary<string, List<dynamic>>();

        public void AddPropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler)
        {
            if (!subscribers.TryGetValue(valueName, out var list))
            {
                list = new List<dynamic>();
                subscribers.Add(valueName, list);
            }
            if (list.Contains(handler)) return;
            list.Add(handler);
        }

        public void RemovePropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler)
        {
            if (subscribers.TryGetValue(valueName, out var list))
                list.Remove(handler);
            if (list?.Count == 0)
                subscribers.Remove(valueName);
        }

        protected virtual void OnPropertyChanged<TValue>(TValue oldValue, TValue newValue, [CallerMemberName]string propertyName = null)
        {
            if (null == propertyName) throw new ArgumentNullException(nameof(propertyName));
            if (!subscribers.TryGetValue(propertyName, out var handlers)) return;
            var e = new ValueChangedEventArgs<TValue>(oldValue, newValue);
            foreach (var handler in handlers.ToArray())
                handler?.Invoke(this, e);
        }
    }
}