using System.Linq;

namespace iLynx.Common
{
    public interface IBindingSource
    {
        void AddPropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler);
        void RemovePropertyChangedHandler<TValue>(string valueName, ValueChangedHandler<TValue> handler);
    }

    public static class ExtensionMethods
    {
        public static IBinding<T> Bind<TTarget, T>(this IBindingSource source, params (TTarget target, string propertyName)[] targets) where TTarget : IBindingSource
        {
            return targets.Aggregate(new MultiBinding<T>(),
                (binding, tuple) => (MultiBinding<T>)binding.Bind(tuple.target, tuple.propertyName));
        }
    }
}