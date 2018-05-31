using System;
using System.Reflection;

namespace iLynx.Common
{
    public class PropertyWrapper<TValue>
    {
        private GetMethod getter;
        private SetMethod setter;

        private delegate TValue GetMethod();
        private delegate void SetMethod(TValue value);

        private PropertyWrapper(string propertyName)
        {
            PropertyName = propertyName;
        }

        public static PropertyWrapper<TValue> Create<TSource>(TSource targetInstance, string propertyName)
            where TSource : IBindingSource
        {
            var sourceType = targetInstance?.GetType() ?? throw new ArgumentNullException(nameof(targetInstance));
            var sourceProperty = sourceType.GetProperty(propertyName) ??
                                 throw new InvalidTypeException(sourceType, propertyName);
            return Create(targetInstance, sourceProperty);
        }

        public static PropertyWrapper<TValue> Create<TSource>(TSource targetInstance, PropertyInfo property)
            where TSource : IBindingSource
        {
            var result = new PropertyWrapper<TValue>(property.Name);
            result.CreateDelegates(targetInstance, property);
            return result;
        }

        private void CreateDelegates(object targetInstance, PropertyInfo property)
        {
            if (property.PropertyType != typeof(TValue)) throw new InvalidTypeException(typeof(TValue).DeclaringType, property.Name);
            var getMethod = property.GetGetMethod();
            var setMethod = property.GetSetMethod();
            getter = (GetMethod)getMethod.CreateDelegate(typeof(GetMethod), targetInstance);
            setter = (SetMethod)setMethod.CreateDelegate(typeof(SetMethod), targetInstance);
        }

        public void SetValue(TValue value)
        {
            setter(value);
        }

        public TValue GetValue()
        {
            return getter();
        }

        public string PropertyName { get; }
    }
}