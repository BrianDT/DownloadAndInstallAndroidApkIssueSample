// <copyright file="IDependencyResolver.cs" company="Visual Software Systems Ltd.">Copyright (c) 2017 All rights reserved</copyright>

namespace FrameworkInterfaces
{
    using System;

    /// <summary>
    /// The interface to platform or framework specific dependency injection
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <typeparam name="InterfaceType">The registered interface type</typeparam>
        /// <returns>The mapped type</returns>
        InterfaceType Resolve<InterfaceType>();

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        object Resolve(Type interfaceType);

        /// <summary>
        /// Registers a class and its interface
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        void Register<T, U>() where T : class where U : class, T;

        /// <summary>
        /// Registers a class as a singleton
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        void RegisterSingleton<T, U>() where T : class where U : class, T;

        /// <summary>
        /// Gets the mapped type from the container given the registered type
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        Type GetMappedType(Type interfaceType);
    }
}
