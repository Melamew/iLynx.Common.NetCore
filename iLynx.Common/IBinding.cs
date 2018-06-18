namespace iLynx.Common
{
    public interface IBinding<TValue>
    {
        void SetValue(TValue value);
        TValue GetValue();

        IBinding<TValue> Bind<TTarget>(TTarget target, string propertyName) where TTarget : IBindingSource;

        IBinding<TValue> Unbind<TTarget>(TTarget target) where TTarget : IBindingSource;
    }
}