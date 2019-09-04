// <copyright file="DependencyHelper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2017 All rights reserved</copyright>

namespace CoreFramework
{
    using System;
    using FrameworkInterfaces;

    /// <summary>
    /// The dependency injection helper holding holds a static copy of the interface to the dependency resolver facade
    /// </summary>
    public class DependencyHelper
    {
        #region [ Private Fields ]

        #endregion

        #region [ Public static properties ]
        /// <summary>
        /// Gets or sets the container service container
        /// </summary>
        public static IDependencyResolver Container
        {
            get;
            set;
        }

        #endregion

        #region [ Public Static Methods ]

        /// <summary>
        /// Resolve a type from the dependency injection container
        /// </summary>
        /// <typeparam name="T">The type to be resolved</typeparam>
        /// <returns>The instance of T</returns>
        public static T Resolve<T>()
        {
            return DependencyHelper.Container.Resolve<T>();
        }

        /// <summary>
        /// Resolve a type from the dependency injection container
        /// </summary>
        /// <param name="interfaceType">The interface type</param>
        /// <returns>The instance of T</returns>
        public static object Resolve(Type interfaceType)
        {
            return DependencyHelper.Container.Resolve(interfaceType);
        }

        /// <summary>
        /// Gets the mapped type from the container given the registered type
        /// </summary>
        /// <param name="registeredType">The registered type</param>
        /// <returns>The mapped type</returns>
        public static Type GetMappedType(Type registeredType)
        {
            return DependencyHelper.Container.GetMappedType(registeredType);
        }

        #endregion
    }
}
