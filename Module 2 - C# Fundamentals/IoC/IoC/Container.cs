using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IoC.Exceptions;
using IoC.Interfaces;

namespace IoC
{
    public class Container //: IContainer
    {
        private readonly Dictionary<Type, Type> registeredTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, object> singletonTypes = new Dictionary<Type, object>();
        
        public void Register<TBase, T>()
        where T : TBase
        {
            Type baseType = typeof(TBase);
            Type type = typeof(T);

            // if ((baseType.IsClass) 
            //     && !type.IsSubclassOf(baseType)
            //     || (baseType.IsInterface) 
            //     && !type.GetInterfaces().Contains(baseType))
            // {
            //     throw new NotInheritException(baseType, type);
            // }
            
            registeredTypes[typeof(TBase)] = typeof(T);
        }

        public void RegisterSingleton<T>()
        {
            singletonTypes[typeof(T)] = GetInstance(typeof(T));
        }
        
        public object GetInstance(Type type)
        {
            
            if (singletonTypes.TryGetValue(type, out var singleton))
            {
                return singleton;
            }
            
            if (registeredTypes.TryGetValue(type, out var resultType))
            {
                return GetInstance(resultType);
            }
            
            var constructor = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            var args = constructor.GetParameters()
                .Select(p => GetInstance(p.ParameterType))
                .ToArray();
            
            return Activator.CreateInstance(type, args);
        }

        public T GetInstance<T>()
        {
            return (T) GetInstance(typeof(T));
        }
    }
}