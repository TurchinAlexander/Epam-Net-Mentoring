using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IoC.Attributes;
using IoC.Exceptions;
using IoC.Interfaces;

namespace IoC
{
    /// <summary>
    /// Represent and IoC container.
    /// </summary>
    public class Container : IContainer
    {
        private readonly Dictionary<Type, Type> registeredTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, object> singletonTypes = new Dictionary<Type, object>();
        private readonly Stack<Type> trace = new Stack<Type>();
        
        public void Register<TBase, T>()
        where T : TBase
        {
            Register(typeof(TBase), typeof(T));
        }

        public void Register(Type baseType, Type type)
        {
             if ((baseType.IsClass) 
                 && !type.IsSubclassOf(baseType)
                 || (baseType.IsInterface) 
                 && !type.GetInterfaces().Contains(baseType))
             {
                 throw new NotInheritException(baseType, type);
             }

             registeredTypes[baseType] = type;
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

            if (trace.Contains(type))
            {   
                throw new CircularReferenceException(type);
            }
            
            trace.Push(type);
            
            if (registeredTypes.TryGetValue(type, out var resultType))
            {
                return GetInstance(resultType);
            }

            if (type.IsInterface
                || type.IsClass && type.IsAbstract)
            {
                throw new NotRegisterException(type);
            }

            var result = CreateInstance(type);
            
            trace.Pop();

            return result;
        }

        public T GetInstance<T>()
        {
            return (T) GetInstance(typeof(T));
        }

        private object CreateInstance(Type type)
        {
            var args = type.GetConstructorWithMostParams()
                .GetParamsList(this);

            var instance = Activator.CreateInstance(type, args);

            Initialize(instance);

            return instance;
        }

        private void Initialize(object obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.NeedInitialization())
                {
                    prop.SetValue(obj, GetInstance(prop.PropertyType));
                }
                
            }
        }
    }

    static class HelpClass
    {
        public static ConstructorInfo GetConstructorWithMostParams(this Type type)
        {
            return type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();
        }

        public static object[] GetParamsList(this ConstructorInfo constructor, Container container)
        {
            return constructor.GetParameters()
                .Select(p => container.GetInstance(p.ParameterType))
                .ToArray();
        }
        
        public static bool NeedInitialization(this PropertyInfo prop)
            => prop.GetCustomAttributes(typeof(InitAttribute), false).Length > 0;
    }
}