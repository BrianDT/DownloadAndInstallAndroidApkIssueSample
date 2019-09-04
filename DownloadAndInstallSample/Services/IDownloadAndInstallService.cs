// <copyright file="IDownloadAndInstallService.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FrameworkInterfaces;

    /// <summary>
    /// Downloads and installs apk files
    /// </summary>
    public interface IDownloadAndInstallService
    {
        /// <summary>
        /// Gets the percentage of the file that has been downloaded 
        /// </summary>
        double DownloadProgress { get; }

        /// <summary>
        /// Get the status of the download
        /// </summary>
        /// <returns>The download state</returns>
        InstallAbility GetDownloadState();

        /// <summary>
        /// Download an apk from a given url and name it with the given version number
        /// </summary>
        /// <param name="urlOfTarget">The url of the apk</param>
        /// <param name="targetVersion">The version number to append</param>
        void Download(string urlOfTarget, string targetVersion);

        /// <summary>
        /// Install an already downloaded update
        /// </summary>
        /// <returns>A method response containing error messages and status</returns>
        IResponse InstallUpdate();
    }
}