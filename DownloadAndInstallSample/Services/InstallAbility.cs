// <copyright file="InstallAbility.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample.Services
{
    /// <summary>
    /// An enumeration of download and install states
    /// </summary>
    public enum InstallAbility
    {
        /// <summary>
        /// The default state
        /// </summary>
        NoUpdateAvailable = 0,

        /// <summary>
        /// The version of the target matches the version of the version of the installed app
        /// </summary>
        AlreadyUptoDate = 1,

        /// <summary>
        /// Downloading the new version
        /// </summary>
        Downloading = 2,

        /// <summary>
        /// Can't locate the file at the given URL
        /// </summary>
        UpdateNotFound = 3,

        /// <summary>
        /// Ready to install the downloaded file
        /// </summary>
        ReadyToInstall = 4,

        /// <summary>
        /// Out of space or permissions issues
        /// </summary>
        UnableToStoreOnDevice = 5,

        /// <summary>
        /// The download failed for other reasons
        /// </summary>
        DownloadFailed = 6
    }
}