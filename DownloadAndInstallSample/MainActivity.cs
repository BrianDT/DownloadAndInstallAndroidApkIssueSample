// <copyright file="MainActivity.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample
{
    using System;
    using System.ComponentModel;
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;
    using Android.Support.Design.Widget;
    using Android.Support.V7.App;
    using Android.Support.V7.Widget;
    using Android.Views;
    using Android.Widget;
    using DownloadAndInstallSample.Initialisation;
    using DownloadAndInstallSample.UI;
    using DownloadAndInstallSample.ViewModel;
    using DroidFramework;
    using FrameworkInterfaces;
    using Lamar;

    /// <summary>
    /// The main view
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        #region [ Private Fields ]

        /// <summary>
        /// The view model for the page
        /// </summary>
        private IMainViewModel viewModel;

        /// <summary>
        /// The action button control
        /// </summary>
        private FloatingActionButton fab;

        /// <summary>
        /// The control for the initial text
        /// </summary>
        private AppCompatTextView initialText;

        /// <summary>
        /// The layout of the download section
        /// </summary>
        private LinearLayout downloadContainer;

        /// <summary>
        /// The download progress control
        /// </summary>
        private ProgressBar progressDownload;

        /// <summary>
        /// The layout of the install section
        /// </summary>
        private LinearLayout installingContainer;

        /// <summary>
        /// The install status text
        /// </summary>
        private AppCompatTextView installStatus;

        /// <summary>
        /// The install error text
        /// </summary>
        private AppCompatTextView installError;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Initialize the contents of the Activity's standard options menu. 
        /// </summary>
        /// <param name="menu">The options menu in which you place your items.</param>
        /// <returns>True if created</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        /// <summary>
        /// This hook is called whenever an item in your options menu is selected. 
        /// </summary>
        /// <param name="item">The menu item that was selected.</param>
        /// <returns>True if handled</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// Callback for the result from requesting permissions. 
        /// </summary>
        /// <param name="requestCode">The request code passed in</param>
        /// <param name="permissions">The requested permissions</param>
        /// <param name="grantResults">The grant results for the corresponding permissions which is either PERMISSION_GRANTED or PERMISSION_DENIED</param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion

        #region [ Protected Methods ]

        /// <summary>
        /// Called when the activity is starting. 
        /// </summary>
        /// <param name="savedInstanceState">If the activity is being re-initialized after previously being shut down then this Bundle contains the data it most recently supplied in OnSaveInstanceState(Bundle)</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (this.viewModel == null)
            {
                IDependencyResolver dependencyResolver = BootStrapper.BootStrap(this.RegisterPlatformDependencies);
                this.viewModel = dependencyResolver.Resolve<IMainViewModel>();
                this.viewModel.PropertyChanged += this.OnViewModelPropertyChanged;
                this.viewModel.InstallCompleted += this.OnInstallCompleted;
            }

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            this.SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            this.SetSupportActionBar(toolbar);

            this.fab = this.FindViewById<FloatingActionButton>(Resource.Id.fab);
            this.fab.Click += this.FabOnClick;

            this.initialText = this.FindViewById<AppCompatTextView>(Resource.Id.initialText);
            this.downloadContainer = this.FindViewById<LinearLayout>(Resource.Id.downloadContainer);
            this.progressDownload = this.FindViewById<ProgressBar>(Resource.Id.progressDownload);
            this.installingContainer = this.FindViewById<LinearLayout>(Resource.Id.installingContainer);
            this.installStatus = this.FindViewById<AppCompatTextView>(Resource.Id.installStatus);
            this.installError = this.FindViewById<AppCompatTextView>(Resource.Id.installError);
        }

        /// <summary>
        /// Perform any final cleanup before an activity is destroyed. 
        /// The activity is destroyed, in this case the view model is not reused.
        /// </summary>
        protected override void OnDestroy()
        {
            if (this.viewModel != null)
            {
                this.viewModel.PropertyChanged -= this.OnViewModelPropertyChanged;
                this.viewModel.InstallCompleted -= this.OnInstallCompleted;
                this.viewModel.Dispose();
                this.viewModel = null;
            }

            base.OnDestroy();
        }

        #endregion

        #region [ Private methods ]

        /// <summary>
        /// An event handler for clicking on the action button
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="eventArgs">The event args</param>
        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            this.fab.Visibility = ViewStates.Gone;
            View view = (View)sender;
            Snackbar.Make(view, "Download", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            this.viewModel.DownLoad();
        }

        /// <summary>
        /// A callback used to register platform dependencies
        /// </summary>
        /// <param name="registry">The DI registry being populated</param>
        private void RegisterPlatformDependencies(ServiceRegistry registry)
        {
            // Framework
            registry.ForSingletonOf<IDispatchOnUIThread>().Use<UIDispatcher>();

            // Platform specific
            registry.For<Context>().Use(this.ApplicationContext);
        }

        /// <summary>
        /// The event handler for view model property changed
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "UpdateState":
                    this.AdjustView();
                    break;

                case "DownloadProgress":
                    this.UpdateProgress((float)this.viewModel.DownloadProgress);
                    break;
            }
        }

        /// <summary>
        /// An event handler for the completion of the install request
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="response">The response from the service</param>
        private void OnInstallCompleted(object sender, IResponse response)
        {
            this.RunOnUiThread(() =>
            {
                if (response.IsSuccess)
                {
                    this.installStatus.Text = "Install requested";
                }
                else
                {
                    this.installStatus.Text = response.Error;
                    this.installError.Text = response.Exception.ToString();
                }
            });
        }

        /// <summary>
        /// Update the view based on the status
        /// </summary>
        private void AdjustView()
        {
            var state = this.viewModel.UpdateState;
            switch (state)
            {
                case Services.InstallAbility.NoUpdateAvailable:
                    this.initialText.Visibility = ViewStates.Visible;
                    this.downloadContainer.Visibility = ViewStates.Gone;
                    this.installingContainer.Visibility = ViewStates.Gone;
                    break;

                case Services.InstallAbility.Downloading:
                    this.downloadContainer.Visibility = ViewStates.Visible;
                    this.initialText.Visibility = ViewStates.Gone;
                    this.installingContainer.Visibility = ViewStates.Gone;
                    break;

                case Services.InstallAbility.ReadyToInstall:
                    this.installingContainer.Visibility = ViewStates.Visible;
                    this.initialText.Visibility = ViewStates.Gone;
                    this.downloadContainer.Visibility = ViewStates.Gone;
                    break;

                default:
                    this.initialText.Text = state.ToString();
                    this.initialText.Visibility = ViewStates.Visible;
                    this.downloadContainer.Visibility = ViewStates.Gone;
                    this.installingContainer.Visibility = ViewStates.Gone;
                    break;
            }
        }

        /// <summary>
        /// Update the progress bar
        /// </summary>
        /// <param name="to">The progress percentage</param>
        private void UpdateProgress(float to)
        {
            this.RunOnUiThread(() =>
            {
                if (this.progressDownload == null)
                {
                    return;
                }

                this.progressDownload.StartAnimation(new ProgressBarAnimation(this.progressDownload, this.progressDownload.Progress, to)
                {
                    Duration = 1000
                });
            });
        }

        #endregion
    }
}
