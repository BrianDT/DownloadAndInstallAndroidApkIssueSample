// <copyright file="BootStrapper.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample.Initialisation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Android.Content;
    using CoreFramework;
    using DownloadAndInstallSample.Services;
    using DownloadAndInstallSample.ViewModel;
    using FrameworkInterfaces;
    using Lamar;

    /// <summary>
    /// IOC container management
    /// </summary>
    public static class BootStrapper
    {
        /// <summary>
        /// Configures the IOC container
        /// </summary>
        /// <param name="registerPlatformDependencies">A callback used to register platform dependencies</param>
        /// <returns>The interface to the dependency injection facade</returns>
        public static IDependencyResolver BootStrap(Action<ServiceRegistry> registerPlatformDependencies)
        {
            LamarDI dependencyInjectionFacade = new LamarDI();
            var registry = new ServiceRegistry();
            registry.For<IDependencyResolver>().Use(dependencyInjectionFacade);

            // Services
            registry.ForSingletonOf<IDownloadAndInstallService>().Use<DownloadAndInstallService>();

            // Platform dependencies
            registerPlatformDependencies(registry);

            // View models
            registry.For<IMainViewModel>().Use<MainViewModel>();

            var container = new Container(registry);

            dependencyInjectionFacade.Initialise(container);
            DependencyHelper.Container = dependencyInjectionFacade;

            return dependencyInjectionFacade;
        }
    }
}