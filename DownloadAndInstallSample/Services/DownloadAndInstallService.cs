// <copyright file="DownloadAndInstallService.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;
    using Framework;
    using FrameworkInterfaces;

    /// <summary>
    /// Downloads and installs apk files
    /// </summary>
    public class DownloadAndInstallService : IDownloadAndInstallService
    {
        /// <summary>
        /// The application context
        /// </summary>
        private readonly Context context;

        /// <summary>
        /// The android download manager
        /// </summary>
        private readonly DownloadManager downloadManager;

        /// <summary>
        /// The key to the download manager record
        /// </summary>
        private long downloadReference;

        /// <summary>
        /// The package name
        /// </summary>
        private string packageName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadAndInstallService" /> class.
        /// </summary>
        /// <param name="context">The application context</param>
        public DownloadAndInstallService(Context context)
        {
            this.context = context;
            this.downloadManager = this.context.GetSystemService(Context.DownloadService) as DownloadManager;
        }

        /// <summary>
        /// Gets the percentage of the file that has been downloaded 
        /// </summary>
        public double DownloadProgress
        {
            get
            {
                if (this.HasDownloadReference)
                {
                    var query = new DownloadManager.Query();
                    query.SetFilterById(this.downloadReference);

                    var cursor = this.downloadManager.InvokeQuery(query);
                    if (cursor.MoveToFirst())
                    {
                        var bytesDownloaded = cursor.GetInt(cursor.GetColumnIndex(DownloadManager.ColumnBytesDownloadedSoFar));
                        var bytesTotal = cursor.GetInt(cursor.GetColumnIndex(DownloadManager.ColumnTotalSizeBytes));

                        return (bytesDownloaded * 100.0D) / bytesTotal;
                    }
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is a download in progress
        /// </summary>
        protected bool HasDownloadReference
        {
            get
            {
                return this.downloadReference != 0;
            }
        }

        /// <summary>
        /// Gets the package name
        /// </summary>
        private string PackageName
        {
            get
            {
                if (this.packageName == null)
                {
                    this.packageName = this.context.ApplicationContext.PackageName;
                }

                return this.packageName;
            }
        }

        /// <summary>
        /// Download an apk from a given url and name it with the given version number
        /// </summary>
        /// <param name="urlOfTarget">The url of the apk</param>
        /// <param name="targetVersion">The version number to append</param>
        public void Download(string urlOfTarget, string targetVersion)
        {
            var uri = Android.Net.Uri.Parse(urlOfTarget);
            var request = new DownloadManager.Request(uri);
            var downloadName = this.PackageName + "." + targetVersion + ".apk";
            request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
            request.SetAllowedOverMetered(true);
            request.SetTitle(string.Format("App update, Version: {0}", targetVersion));
            request.SetDestinationInExternalFilesDir(this.context, Android.OS.Environment.DirectoryDownloads, downloadName);
            request.SetMimeType("application/vnd.android.package-archive");

            this.downloadReference = this.downloadManager.Enqueue(request);
        }

        /// <summary>
        /// Install an already downloaded update
        /// </summary>
        /// <returns>A method response containing error messages and status</returns>
        public IResponse InstallUpdate()
        {
            var response = new Response();
            if (this.GetDownloadState() == InstallAbility.ReadyToInstall)
            {
                var path = this.GetInstallPath();
                if (!string.IsNullOrWhiteSpace(path))
                {
                    Intent intent = null;
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                    {
                        Android.Net.Uri apkUri = null;
                        try
                        {
                            apkUri = this.GetContentPath(new Java.IO.File(path));
                        }
                        catch (Exception ex)
                        {
                            response.Error = "Failed to generate the centent url";
                            response.Exception = ex;
                            return response;
                        }

                        intent = new Intent(Intent.ActionInstallPackage);
                        intent.SetDataAndType(apkUri, "application/vnd.android.package-archive");
                        intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                    }
                    else
                    {
                        var file = Android.Net.Uri.Parse(path);
                        intent = new Intent(Intent.ActionView);
                        intent.SetDataAndType(file, "application/vnd.android.package-archive");
                        intent.SetFlags(ActivityFlags.NewTask);
                    }

                    this.context.StartActivity(intent);
                }

                response.IsSuccess = true;
            }

            return response;
        }

        /// <summary>
        /// Get the status of the download
        /// </summary>
        /// <returns>The download state</returns>
        public InstallAbility GetDownloadState()
        {
            InstallAbility installAbility = InstallAbility.UpdateNotFound;

            if (this.HasDownloadReference)
            {
                var query = new DownloadManager.Query();
                query.SetFilterById(this.downloadReference);

                var cursor = this.downloadManager.InvokeQuery(query);
                if (cursor.MoveToFirst())
                {
                    var status = (DownloadStatus)cursor.GetInt(cursor.GetColumnIndex(DownloadManager.ColumnStatus));

                    switch (status)
                    {
                        case DownloadStatus.Successful:
                            installAbility = InstallAbility.ReadyToInstall;
                            break;
                        case DownloadStatus.Paused:
                        case DownloadStatus.Pending:
                        case DownloadStatus.Running:
                            installAbility = InstallAbility.Downloading;
                            break;
                        case DownloadStatus.Failed:
                            var reasonCode = (DownloadError)cursor.GetInt(cursor.GetColumnIndex(DownloadManager.ColumnReason));
                            if (reasonCode == DownloadError.InsufficientSpace || reasonCode == DownloadError.FileError || reasonCode == DownloadError.DeviceNotFound)
                            {
                                installAbility = InstallAbility.UnableToStoreOnDevice;
                            }
                            else if (reasonCode == DownloadError.FileAlreadyExists)
                            {
                                installAbility = InstallAbility.ReadyToInstall;
                            }
                            else if ((int)reasonCode == 404)
                            {
                                installAbility = InstallAbility.UpdateNotFound;
                            }
                            else
                            {
                                installAbility = InstallAbility.DownloadFailed;
                            }

                            break;
                    }
                }
            }

            return installAbility;
        }

        /// <summary>
        /// Gets the install path from the download manager
        /// </summary>
        /// <returns>The install path</returns>
        private string GetInstallPath()
        {
            if (this.HasDownloadReference)
            {
                var query = new DownloadManager.Query();
                query.SetFilterById(this.downloadReference);

                var cursor = this.downloadManager.InvokeQuery(query);
                if (cursor.MoveToFirst())
                {
                    return cursor.GetString(cursor.GetColumnIndex(DownloadManager.ColumnLocalUri));
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the content url that matches the download path
        /// </summary>
        /// <param name="apkPath">The local path of the download file</param>
        /// <returns>The content url</returns>
        private Android.Net.Uri GetContentPath(Java.IO.File apkPath)
        {
            var providerName = this.PackageName + ".fileprovider";
            var uri = global::Android.Support.V4.Content.FileProvider.GetUriForFile(this.context, providerName, apkPath);
            return uri;
        }
    }
}