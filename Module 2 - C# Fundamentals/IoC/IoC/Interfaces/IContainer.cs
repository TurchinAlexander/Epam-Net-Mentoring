using System;
using System.Reflection;

namespace IoC.Interfaces
{
    public interface IContainer
    {
        void RegisterAssembly(Assembly assembly);

        void Register(Type type);

        void Register<T>();

        void Register(Type baseType, Type type);

        void Register<TBase, T>();

        object GetInstance(Type type);

        T GetInstance<T>();
    }
}