using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace iLynx.Common
{
    public class ValueBinding<TValue> where TValue : IEquatable<TValue>
    {
        private TValue value;

        public ValueBinding() { }

        public ValueBinding(TValue initialValue)
        {
            value = initialValue;
        }

        public TValue Value
        {
            get => value;
            set
            {
                if (this.value.Equals(value)) return;
                var oldValue = this.value;
                OnValueChanging(oldValue, value);
                this.value = value;
                OnValueChanged(oldValue, value);
            }
        }

        public static implicit operator ValueBinding<TValue>(TValue value)
        {
            return new ValueBinding<TValue>(value);
        }

        public static implicit operator TValue(ValueBinding<TValue> binding)
        {
            return binding.value;
        }

        /// <summary>
        /// This method is called BEFORE the actual internal value of the binding changes.
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnValueChanging(TValue currentValue, TValue newValue)
        {
            ValueChanging?.Invoke(new ValueChangingEventArgs<TValue>(currentValue, newValue));
        }

        /// <summary>
        /// This method is called AFTER the actual internal value of the binding has changed.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnValueChanged(TValue oldValue, TValue newValue)
        {
            ValueChanged?.Invoke(new ValueChangedEventArgs<TValue>(oldValue, newValue));
        }

        public event ValueChangingHandler<TValue> ValueChanging;

        public event ValueChangedHandler<TValue> ValueChanged;
    }

    public delegate void ValueChangedHandler<TValue>(ValueChangedEventArgs<TValue> e);

    public delegate void ValueChangingHandler<TValue>(ValueChangingEventArgs<TValue> e);

    public class ValueChangedEventArgs<TValue> : EventArgs
    {
        public ValueChangedEventArgs(TValue oldValue, TValue newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
        public TValue OldValue { get; }
        public TValue NewValue { get; }
    }

    public class ValueChangingEventArgs<TValue> : EventArgs
    {
        public ValueChangingEventArgs(TValue currentValue, TValue newValue)
        {
            CurrentValue = currentValue;
            NewValue = newValue;
        }
        public TValue CurrentValue { get; }
        public TValue NewValue { get; }
    }

    public class Binding<TSource, T>
    {
        public Binding(TSource source, Expression<Func<TSource, T>> memberExpression)
        {

        }
    }
}
