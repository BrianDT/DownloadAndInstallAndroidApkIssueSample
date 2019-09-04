// <copyright file="LamarDI.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace CoreFramework
{
    using System;
    using System.Linq;
    using FrameworkInterfaces;
    using Lamar;

    /// <summary>
    /// A Simple Injector implementation of the dependency injection resolution interface
    /// </summary>
    public class LamarDI : IDependencyResolver
    {
        /// <summary>
        /// The simple injector container
        /// </summary>
        private IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="LamarDI" /> class.
        /// </summary>
        public LamarDI()
        {
        }

        /// <summary>
        /// Injects the container after the facade has been mapped
        /// </summary>
        /// <param name="container">The simple injector container</param>
        public void Initialise(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <typeparam name="InterfaceType">The registered interface type</typeparam>
        /// <returns>The mapped type</returns>
        public InterfaceType Resolve<InterfaceType>()
        {
            return (InterfaceType)this.container.GetInstance<InterfaceType>();
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public object Resolve(Type interfaceType)
        {
            return this.container.GetInstance(interfaceType);
        }

        /// <summary>
        /// Registers a class and its interface
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void Register<T, U>() where T : class where U : class, T
        {
            var registry = new ServiceRegistry();
            registry.For<T>().Use<U>();
            this.container.Configure(registry);
        }

        /// <summary>
        /// Registers a class as a singleton
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void RegisterSingleton<T, U>() where T : class where U : class, T
        {
            var registry = new ServiceRegistry();
            registry.ForSingletonOf<T>().Use<U>();
            this.container.Configure(registry);
        }

        /// <summary>
        /// Gets the mapped type from the container given the registered type
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public Type GetMappedType(Type interfaceType)
        {
            if (this.container != null)
            {
                var instance = this.container.Model.For(interfaceType).Instances.FirstOrDefault();
                if (instance != null)
                {
                    return instance.ImplementationType;
                }

                return null;
                ////return this.container.Model.For(interfaceType).PluginType;
            }

            return null;
        }
    }
}
