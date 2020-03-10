using System;

namespace IoC.Exceptions
{
    public class CircularReferenceException : Exception
    {
        public CircularReferenceException(Type type)
            : base($"The circular reference of type {type} is found!")
        { }
    }
}