// <copyright file="UIDispatcher.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2016 All rights reserved</copyright>

namespace DroidFramework
{
    using System;

    using Android.Content;
    using Android.OS;
    using FrameworkInterfaces;
    using Java.Lang;

    /// <summary>
    /// Dispatcher that marshalls calls onto the UI thread
    /// </summary>
    public class UIDispatcher : IDispatchOnUIThread
    {
        /// <summary>
        /// The thread handler to the main event loop
        /// </summary>
        private readonly Handler handler;

        /// <summary>
        /// The UI thread
        /// </summary>
        private readonly Thread uiThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIDispatcher" /> class.
        /// </summary>
        /// <param name="context">The UI context</param>
        public UIDispatcher(Context context)
        {
            this.handler = new Handler(context.MainLooper);
            this.uiThread = this.handler.Looper.Thread;
        }

        /// <summary>
        /// Execute synchronous action on UI thread
        /// </summary>
        /// <param name="action">The action to execute</param>
        public void Invoke(Action action)
        {
            if (Thread.CurrentThread() != this.uiThread)
            {
                this.handler.Post(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}