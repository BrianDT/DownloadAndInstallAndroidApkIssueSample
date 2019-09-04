// <copyright file="BaseViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;
    using FrameworkInterfaces;

    /// <summary>
    /// Simple view model base class that implements INotifyPropertyChanged
    /// </summary>
    public abstract class BaseViewModel : IBaseViewModel
    {
        /// <summary>
        /// Dispatcher that marshals calls onto the UI thread
        /// </summary>
        private IDispatchOnUIThread dispatcher;

        /// <summary>
        /// True if the view model has been disposed
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel" /> class.
        /// </summary>
        /// <param name="dispatcher">Dispatcher that marshals calls onto the UI thread</param>
        public BaseViewModel(IDispatchOnUIThread dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether the view model has been disposed
        /// </summary>
        protected bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }

        #region [ IDisposable Members ]

        /// <summary>
        /// Dispose of any retained objects
        /// </summary>
        public virtual void Dispose()
        {
            this.isDisposed = true;
        }

        #endregion

        /// <summary>
        /// Notifies a property change
        /// </summary>
        /// <param name="propertyName">The property name</param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (this.dispatcher != null)
            {
                this.dispatcher.Invoke(() =>
                {
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                });
            }
            else
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
