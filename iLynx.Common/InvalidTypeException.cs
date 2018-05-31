using System;
using System.Reflection;

namespace iLynx.Common
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException(Type sourceType, string memberName)
            : base($"The type {sourceType} does not contain a valid binding member with the propertyName {memberName}")
        {

        }

        public InvalidTypeException(Type expectedType, PropertyInfo property) :
            base($"The property {property.Name} is not of the expected type {expectedType}")
        { }
    }
}