using System;

namespace iLynx.Common
{
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
}