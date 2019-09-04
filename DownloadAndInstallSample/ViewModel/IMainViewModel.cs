// <copyright file="IMainViewModel.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample.ViewModel
{
    using System;
    using DownloadAndInstallSample.Services;
    using FrameworkInterfaces;

    /// <summary>
    /// The view model for the main page
    /// </summary>
    public interface IMainViewModel : IBaseViewModel
    {
        /// <summary>
        /// An event that notifies the completion of the install
        /// </summary>
        event EventHandler<IResponse> InstallCompleted;

        /// <summary>
        /// Gets the status of the update
        /// </summary>
        InstallAbility UpdateState { get; }

        /// <summary>
        /// Gets the update manager download progress
        /// </summary>
        double DownloadProgress { get; }

        /// <summary>
        /// Download a sample file
        /// </summary>
        void DownLoad();
    }
}