using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Jff.Csharp.Tools.Domain.Exceptions;

namespace JffCsharpTools.Common
{
    /// <summary>
    /// Generic default response model for application operations.
    /// Centralizes error, success, and return data information.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Main error message when the operation fails
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Base exception information that caused the error
        /// </summary>
        public string BaseException { get; set; }

        /// <summary>
        /// Complete stack trace of the error for debugging purposes
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Descriptive message about the operation result
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// HTTP status code of the response (default: 200 OK)
        /// </summary>
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        /// <summary>
        /// List of additional messages (validations, warnings, etc.)
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();

        /// <summary>
        /// List of additional messages (validations, warnings, etc.)
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Indicates whether the operation was executed successfully.
        /// Checks if status is OK/NoContent and there are no error messages.
        /// </summary>
        public bool Success
        {
            get
            {
                var checkStatus = StatusCode == HttpStatusCode.OK || StatusCode == HttpStatusCode.NoContent;
                var checkMessage = string.IsNullOrEmpty(Error) && Errors?.Any() != true && string.IsNullOrEmpty(BaseException) && string.IsNullOrEmpty(StackTrace);
                var checkResult = checkStatus && checkMessage;
                return checkResult;
            }
        }

        /// <summary>
        /// Factory method to create a successful result with default values.
        /// Sets StatusCode to OK and leaves other properties empty.
        /// </summary>
        /// <returns>A successful result with default values</returns>
        public static Result Ok() => new Result();

        /// <summary>
        /// Factory method to create a failed result with a specified error message, optional detailed message, and HTTP status code (default: BadRequest).
        /// </summary>
        /// <param name="error">The main error message</param>
        /// <param name="message">Optional detailed message</param>
        /// <param name="statusCode">HTTP status code (default: BadRequest)</param>
        /// <returns>A failed result with the specified error information</returns>
        public static Result Fail(string error, string message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new Result(error, message, statusCode);

        public Result()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Result class with a specified error message, optional detailed message, and HTTP status code (default: BadRequest).
        /// </summary>
        /// <param name="error">The main error message</param>
        /// <param name="message">Optional detailed message</param>
        /// <param name="statusCode">HTTP status code (default: BadRequest)</param>
        public Result(string error, string message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            Error = error;
            Message = message;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the Result class based on a DomainException, extracting error details, base exception, and stack trace for comprehensive error reporting.
        /// </summary>
        /// <param name="ex">The DomainException instance</param>
        public Result(DomainException ex)
        {
            Error = ex.Message;
            Errors = ex.Errors.ToList();
            BaseException = ex.InnerException?.Message;
            StackTrace = ex.StackTrace;
        }

        /// <summary>
        /// Initializes a new instance of the Result class based on a general Exception, extracting the main error message, base exception, and stack trace for comprehensive error reporting.
        /// </summary>
        /// <param name="ex">The Exception instance</param>
        public Result(Exception ex)
        {
            Error = ex.Message;
            BaseException = ex.InnerException?.Message;
            StackTrace = ex.StackTrace;
        }
    }
}