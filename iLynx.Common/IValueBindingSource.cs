namespace iLynx.Common
{
    public interface IValueBindingSource
    {
        void AddValueChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler);
        void RemoveValueChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler);
    }
}