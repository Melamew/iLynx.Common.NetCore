using System;
using static System.String;

namespace iLynx.Common
{
    public class Binding<TSource, TValue> where TSource : IValueBindingSource
    {
        private readonly TSource source;
        private readonly string valueName;
        public Binding(TSource source, string valueName)
        {
            if (source.Equals(null)) throw new ArgumentNullException(nameof(source));
            this.source = source;
            if (IsNullOrEmpty(valueName)) throw new ArgumentException(nameof(valueName));
            this.valueName = valueName;
        }


    }
}