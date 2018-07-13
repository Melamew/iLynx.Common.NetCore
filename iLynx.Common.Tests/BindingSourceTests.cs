using Xunit;

namespace iLynx.Common.Tests {
    public class BindingSourceTests {
        public class DummyImpl : BindingSource {
            public void DoRaisePropertyChanged (string name) {
                OnPropertyChanged (0, 1, name);
            }

            public void DoRaisePropertyChanged<T> (string name, T oldVal, T newVal) {
                OnPropertyChanged (oldVal, newVal, name);
            }
        }

        [Fact]
        public void WhenPropertyChangedThenCorrectHandlerInvoked () {
            var impl = new DummyImpl ();
            const string Property = "Some property";
            const string OtherProperty = "Some Other Property";
            impl.AddPropertyChangedHandler<int> (Property, (o, e) => {
                Assert.Equal (Property, e.PropertyName);
                Assert.NotEqual (OtherProperty, e.PropertyName);
            });
            impl.DoRaisePropertyChanged (Property);
            impl.DoRaisePropertyChanged (OtherProperty);
        }

        [Fact]
        public void WhenPropertyChangedRaisedWithDifferentTypesThenCorrectHandlerStillCalled () {
            // Even though this shouldn't /really/ be possible, we'll still test for it.
            var impl = new DummyImpl ();
            const int ExpectedInt = 734095;
            const string ExpectedString = "Expected";
            const string Property = "Property";
            impl.AddPropertyChangedHandler<int> (Property, (o, e) => {
                Assert.Equal (Property, e.PropertyName);
                Assert.Equal (ExpectedInt, e.NewValue);
                // Assert.NotEqual (ExpectedString, e.NewValue);
            });
            impl.AddPropertyChangedHandler<string> (Property, (o, e) => {
                Assert.Equal (Property, e.PropertyName);
                Assert.Equal (ExpectedString, e.NewValue);
                // Assert.NotEqual (ExpectedInt, e.NewValue);
            });
            impl.DoRaisePropertyChanged (Property, ExpectedInt, ExpectedInt);
        }
    }
}
