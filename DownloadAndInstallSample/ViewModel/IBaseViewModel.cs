// <copyright file="IBaseViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;

    /// <summary>
    /// Simple view model base class that implements INotifyPropertyChanged
    /// </summary>
    public interface IBaseViewModel : INotifyPropertyChanged, IDisposable
    {
    }
}
