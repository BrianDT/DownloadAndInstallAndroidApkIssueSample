// <copyright file="MainViewModel.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DownloadAndInstallSample.Services;
    using FrameworkInterfaces;

    /// <summary>
    /// The view model for the main page
    /// </summary>
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        /// <summary>
        /// The url of the file to download (in this case itself)
        /// </summary>
        private const string SampleUrl = "https://github.com/BrianDT/DownloadAndInstallAndroidApkIssueSample/DownloadFrom/sample.apk";

        /// <summary>
        /// The version number to suffix
        /// </summary>
        private const string SampleVersionNumber = "1.0.1";

        /// <summary>
        /// A cancellation token source
        /// </summary>
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// A service that downloads and installs apk files
        /// </summary>
        private IDownloadAndInstallService downloadAndInstallService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <param name="dispatcher">Dispatcher that marshals calls onto the UI thread</param>
        /// <param name="downloadAndInstallService">A service that downloads and installs apk files</param>
        public MainViewModel(IDispatchOnUIThread dispatcher, IDownloadAndInstallService downloadAndInstallService) : base(dispatcher)
        {
            this.downloadAndInstallService = downloadAndInstallService;
            this.UpdateState = InstallAbility.NoUpdateAvailable;
        }

        /// <summary>
        /// An event that notifies the completion of the install
        /// </summary>
        public event EventHandler<IResponse> InstallCompleted;

        /// <summary>
        /// Gets the status of the update
        /// </summary>
        public InstallAbility UpdateState { get; private set; }

        /// <summary>
        /// Gets the update manager download progress
        /// </summary>
        public double DownloadProgress => this.downloadAndInstallService.DownloadProgress;

        /// <summary>
        /// Download a sample file
        /// </summary>
        public void DownLoad()
        {
            this.downloadAndInstallService.Download(SampleUrl, SampleVersionNumber);
            this.MonitorDownLoad();
        }

        /// <summary>
        /// Dispose any residual objects
        /// </summary>
        public override void Dispose()
        {
            if (this.tokenSource != null)
            {
                this.tokenSource.Cancel();
                this.tokenSource.Dispose();
                this.tokenSource = null;
            }

            base.Dispose();
        }

        /// <summary>
        /// Monitor the download status at a given frequency
        /// </summary>
        /// <param name="frequencySeconds">The frequency i9n seconds</param>
        private void MonitorDownLoad(double frequencySeconds = 1.0)
        {
            this.UpdateState = InstallAbility.Downloading;
            this.OnPropertyChanged("UpdateState");

            if (this.tokenSource != null)
            {
                this.tokenSource.Cancel();
                this.tokenSource.Dispose();
            }

            this.tokenSource = new CancellationTokenSource();

            TimeSpan frequency = TimeSpan.FromSeconds(frequencySeconds);
            var cancellationToken = this.tokenSource.Token;

            Task.Run(
                async () =>
                {
                    while ((this.UpdateState = this.downloadAndInstallService.GetDownloadState()) == InstallAbility.Downloading)
                    {
                        await Task.Delay(frequency);
                        this.OnPropertyChanged("DownloadProgress");
                    }

                    this.OnPropertyChanged("UpdateState");

                    if (this.UpdateState == InstallAbility.ReadyToInstall)
                    {
                        var response = this.downloadAndInstallService.InstallUpdate();
                        this.InstallCompleted?.Invoke(this, response);
                    }
                },
            cancellationToken);
        }
    }
}