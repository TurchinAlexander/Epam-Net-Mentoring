using System;
using IoC.Exceptions;

namespace IoC.Interfaces
{
    public interface IContainer
    {
        /// <summary>
        /// Register the realization of <typeparamref name="TBase"/> type in <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="TBase">The <see cref="Type"/>.</typeparam>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        public void Register<TBase, T>()
            where T : TBase;

        /// <summary>
        /// Register the realization of <paramref name="baseType"/> type in <paramref name="type"/> type.
        /// </summary>
        /// <paramref name="baseType">The <see cref="Type"/>.</paramref>
        /// <paramref name="type">The <see cref="Type"/>.</paramref>
        /// <exception cref="NotInheritException"> occurs when <paramref name="type"/> is not inherited from <paramref name="baseType"/>.</exception>
        void Register(Type baseType, Type type);

        /// <summary>
        /// Register the type as a singleton.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        public void RegisterSingleton<T>();

        /// <summary>
        /// Get an instance of a <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>Instance of a <paramref name="type"/>.</returns>
        /// <exception cref="CircularReferenceException">occurs when an instance has a prop of its type and it should be initialized.</exception>
        /// <exception cref="NotRegisterException">occurs when here is no realization of an interface or abstract class.</exception>
        public object GetInstance(Type type);

        /// <summary>
        /// Get an instance of a <typeparam name="T"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        /// <returns>Instance of a <typeparam name="T"/>.</returns>
        /// <exception cref="CircularReferenceException">occurs when an instance has a prop of its type and it should be initialized.</exception>
        /// <exception cref="NotRegisterException">occurs when here is no realization of an interface or abstract class.</exception>
        public T GetInstance<T>();
    }
}