// <copyright file="Response.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2018 All rights reserved</copyright>

namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using FrameworkInterfaces;

    /// <summary>
    /// A method response containing error messages and status
    /// </summary>
    public class Response : IResponse
    {
        #region [ Private fields ]

        /// <summary>
        /// A value indicating whether the call was a success
        /// </summary>
        private bool isSuccess;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        public Response()
        {
            this.isSuccess = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        /// <param name="error">An error message</param>
        public Response(string error)
        {
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        /// <param name="exception">An exception</param>
        public Response(Exception exception)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        /// <param name="response">Another response object</param>
        public Response(IResponse response)
        {
            this.IsSuccess = response.IsSuccess;
            this.CopyError(response);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value indicating whether the call was a success
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return this.isSuccess;
            }

            set
            {
                this.isSuccess = value;
            }
        }

        /// <summary>
        /// Gets or sets any error
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the error should be treated as a warning
        /// </summary>
        public bool TreatAsWarning { get; set; }

        /// <summary>
        /// Gets or sets any exception
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets a values used to distinguish between different error or success conditions 
        /// </summary>
        public int Category { get; set; }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Rollup the error from another response
        /// </summary>
        /// <param name="from">The response to rollup</param>
        /// <returns>True if there was an error</returns>
        public bool CopyError(IResponse from)
        {
            if (!from.IsSuccess)
            {
                this.Error = from.Error;
                this.Exception = from.Exception;
                this.Category = from.Category;
                this.TreatAsWarning = from.TreatAsWarning;

                return true;
            }

            return false;
        }

        #endregion
    }

    /// <summary>
    /// A function response containing the results, error messages and status
    /// </summary>
    /// <typeparam name="T">The type of the returned result</typeparam>
    public class Response<T> : Response, IResponse<T>
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}" /> class.
        /// </summary>
        public Response()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}" /> class.
        /// </summary>
        /// <param name="result">A result object</param>
        public Response(T result)
        : base()
        {
            this.Result = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}" /> class.
        /// </summary>
        /// <param name="result">A result object</param>
        /// <param name="error">An error message</param>
        public Response(T result, string error)
        : base(error)
        {
            this.Result = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}" /> class.
        /// </summary>
        /// <param name="result">A result object</param>
        /// <param name="exception">An exception</param>
        public Response(T result, Exception exception)
        : base(exception)
        {
            this.Result = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}" /> class.
        /// </summary>
        /// <param name="response">Another response object</param>
        public Response(IResponse response) : base(response)
        {
        }

        #endregion

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public T Result { get; set; }
    }
}
