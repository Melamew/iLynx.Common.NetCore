using System;

namespace iLynx.Common
{
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
}