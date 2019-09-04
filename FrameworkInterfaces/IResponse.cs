// <copyright file="IResponse.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2018 All rights reserved</copyright>

namespace FrameworkInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A method response containing error messages and status
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Gets a value indicating whether the call was a success
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Gets any error
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Gets any exception
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// Gets or sets a values used to distinguish between different error or success conditions 
        /// </summary>
        int Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the error should be treated as a warning
        /// </summary>
        bool TreatAsWarning { get; set; }

        /// <summary>
        /// Rollup the error from another response
        /// </summary>
        /// <param name="from">The response to rollup</param>
        /// <returns>True if there was an error</returns>
        bool CopyError(IResponse from);
    }

    /// <summary>
    /// A method response containing error messages and status
    /// </summary>
    /// <typeparam name="T">The type of the returned result</typeparam>
    public interface IResponse<T> : IResponse
    {
        /// <summary>
        /// Gets or sets the result
        /// </summary>
        T Result { get; set; }
    }
}
