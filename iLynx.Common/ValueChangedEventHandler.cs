namespace iLynx.Common
{
    public delegate void ValueChangedEventHandler<in TSender, TValue>(TSender sender, ValueChangedEventArgs<TValue> args);
}