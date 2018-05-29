using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace iLynx.Common
{
    public class ValueBindingSource : IValueBindingSource
    {
        private readonly Dictionary<string, List<dynamic>> subscribers =
            new Dictionary<string, List<dynamic>>();

        public void AddValueChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler)
        {
            if (!subscribers.TryGetValue(valueName, out var list))
            {
                list = new List<dynamic>();
                subscribers.Add(valueName, list);
            }
            if (list.Contains(handler)) return;
            list.Add(handler);
        }

        public void RemoveValueChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler)
        {
            if (subscribers.TryGetValue(valueName, out var list))
                list.Remove(handler);
            if (list?.Count == 0)
                subscribers.Remove(valueName);
        }

        protected virtual void OnValueChanged<TValue>(string name, TValue oldValue, TValue newValue)
        {
            if (!subscribers.TryGetValue(name, out List<dynamic> handlers)) return;
            var e = new ValueChangedEventArgs<TValue>(oldValue, newValue);
            foreach (var handler in handlers.ToArray().Select(x => x as ValueChangedHandler<TValue>))
                handler?.Invoke(e);
        }
    }
}
