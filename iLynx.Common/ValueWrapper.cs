using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

    public class PropertyWrapper<TValue>
    {
        private GetMethod getter;
        private SetMethod setter;

        private delegate TValue GetMethod();

        private delegate void SetMethod(TValue value);

        public PropertyWrapper(PropertyInfo property, object targetInstance)
        {
            CreateDelegates(property, targetInstance);
        }

        public PropertyWrapper(object targetInstance, string propertyName)
        {
            var sourceType = targetInstance?.GetType() ?? throw new ArgumentNullException(nameof(targetInstance));
            var sourceProperty = sourceType.GetProperty(propertyName) ??
                                 throw new InvalidTypeException(sourceType, propertyName);
            CreateDelegates(sourceProperty, targetInstance);
        }

        private void CreateDelegates(PropertyInfo property, object targetInstance)
        {
            if (property.PropertyType != typeof(TValue)) throw new InvalidTypeException();
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
    }

    public class TwoWayBinding<TValue> : IBinding<TValue>
    {
        private readonly PropertyWrapper<TValue> sourceWrapper;
        private readonly Dictionary<object, PropertyWrapper<TValue>> targets = new Dictionary<object, PropertyWrapper<TValue>>();

        public TwoWayBinding(object source, string memberName)
        {
            sourceWrapper = new PropertyWrapper<TValue>(source, memberName);
        }

        public void AddTarget(object target, string propertyName)
        {
            if (targets.ContainsKey(target)) return;
            targets.Add(target, new PropertyWrapper<TValue>(target, propertyName));
        }

        public void RemoveTarget(object target)
        {
            targets.Remove(target);
        }

        public void SetValue(TValue value)
        {
            sourceWrapper.SetValue(value);
            foreach (var target in targets.Values)
                target.SetValue(value);
        }

        public TValue GetValue()
        {
            return sourceWrapper.GetValue();
        }
    }

    public class InvalidTypeException : Exception
    {
        public InvalidTypeException(Type sourceType, string memberName)
            : base($"The type {sourceType} does not contain a valid binding member with the name {memberName}")
        {
            
        }
    }

    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    //    AllowMultiple = false,
    //    Inherited = false)]
    //public class Bindable : Attribute
    //{
    //}

    public interface IBinding<TValue>
    {
        void SetValue(TValue value);
        TValue GetValue();
    }
}
