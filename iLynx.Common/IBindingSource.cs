namespace iLynx.Common
{
    public interface IBindingSource
    {
        void AddPropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler);
        void RemovePropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler);
    }
}