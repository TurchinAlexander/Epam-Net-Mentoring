using System;

namespace IoC.Exceptions
{
    public class NotInheritException : Exception
    {
        public NotInheritException(Type baseType, Type type)
            : base($"The is type {type} is not subclass of type {baseType}")
        { }
    }
}