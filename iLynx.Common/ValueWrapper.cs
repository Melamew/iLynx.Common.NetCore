using System;

namespace iLynx.Common
{
    public class ValueWrapper<TValue> where TValue : IEquatable<TValue>
    {
        private TValue value;

        public ValueWrapper() { }

        public ValueWrapper(TValue initialValue)
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

        public static implicit operator ValueWrapper<TValue>(TValue value)
        {
            return new ValueWrapper<TValue>(value);
        }

        public static implicit operator TValue(ValueWrapper<TValue> wrapper)
        {
            return wrapper.value;
        }

        /// <summary>
        /// This method is called BEFORE the actual internal value of the wrapper changes.
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnValueChanging(TValue currentValue, TValue newValue)
        {
            ValueChanging?.Invoke(new ValueChangingEventArgs<TValue>(currentValue, newValue));
        }

        /// <summary>
        /// This method is called AFTER the actual internal value of the wrapper has changed.
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
}
