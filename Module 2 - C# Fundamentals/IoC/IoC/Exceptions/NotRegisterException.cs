using System;

namespace IoC.Exceptions
{
    public class NotRegisterException : Exception
    {
        public NotRegisterException(Type type)
            : base ($"The type {type} is not registered in the container!")
        { }
    }
}