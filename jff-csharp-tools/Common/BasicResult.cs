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

        public static Result Ok() => new Result();

        public static Result Fail(string error, string message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new Result(error, message, statusCode);

        public Result()
        {
        }

        public Result(string error, string message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            Error = error;
            Message = message;
            StatusCode = statusCode;
        }

        public Result(DomainException ex)
        {
            Error = ex.Message;
            Errors = ex.Errors.ToList();
            BaseException = ex.InnerException?.Message;
            StackTrace = ex.StackTrace;
        }

        public Result(Exception ex)
        {
            Error = ex.Message;
            BaseException = ex.InnerException?.Message;
            StackTrace = ex.StackTrace;
        }
    }
}