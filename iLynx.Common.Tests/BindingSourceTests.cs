using System;
using iLynx.Common.Threading;
using Xunit;

namespace iLynx.Common.Tests
{
    public class BindingSourceTests
    {
        public class DummyImpl : BindingSource
        {
            public void DoRaisePropertyChanged(string name)
            {
                OnPropertyChanged(0, 1, name);
            }

            public void DoRaisePropertyChanged<T>(string name, T oldVal, T newVal)
            {
                OnPropertyChanged(oldVal, newVal, name);
            }
        }

        [Fact]
        public void WhenPropertyChangedRaisedWithDifferentTypesThenCorrectHandlerStillCalled()
        {
            // Even though this shouldn't /really/ be possible, we'll still test for it.
            var impl = new DummyImpl();
            const int expectedInt = 734095;
            const string expectedString = "Expected";
            const string property = "Property";
            impl.AddPropertyChangedHandler<int>(
                property,
                (o, e) =>
                {
                    Assert.Equal(property, e.PropertyName);
                    Assert.Equal(expectedInt, e.NewValue);
                }
            );
            impl.AddPropertyChangedHandler<string>(
                property,
                (o, e) =>
                {
                    Assert.Equal(property, e.PropertyName);
                    Assert.Equal(expectedString, e.NewValue);
                }
            );
            impl.DoRaisePropertyChanged(property, expectedInt, expectedInt);
        }

        [Fact]
        public void WhenPropertyChangedThenCorrectHandlerInvoked()
        {
            var impl = new DummyImpl();
            const string property = "Some property";
            const string otherProperty = "Some Other Property";
            impl.AddPropertyChangedHandler<int>(
                property,
                (o, e) =>
                {
                    Assert.Equal(property, e.PropertyName);
                    Assert.NotEqual(otherProperty, e.PropertyName);
                }
            );
            impl.DoRaisePropertyChanged(property);
            impl.DoRaisePropertyChanged(otherProperty);
        }

        [Fact]
        public void WhenPropertChangedCalledWithNullNameThenExceptionThrown()
        {
            var impl = new DummyImpl();
            Assert.Throws<ArgumentNullException>(() => impl.DoRaisePropertyChanged(null));
        }
    }
}
