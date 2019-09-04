// <copyright file="IDispatchOnUIThread.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2016 All rights reserved</copyright>

namespace FrameworkInterfaces
{
    using System;

    /// <summary>
    /// Dispatcher that marshalls calls onto the UI thread
    /// </summary>
    public interface IDispatchOnUIThread
    {
        /// <summary>
        /// Execute synchronous action on UI thread
        /// </summary>
        /// <param name="action">The action to execute</param>
        void Invoke(Action action);
    }
}
