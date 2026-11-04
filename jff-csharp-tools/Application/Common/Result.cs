
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JffCsharpTools.Domain.Exceptions;

namespace JffCsharpTools.Application.Common
{
    /// <summary>
    /// Generic default response model for application operations.
    /// Centralizes error, success, and return data information.
    /// </summary>
    /// <typeparam name="T">Type of the result object to be returned</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// Result data of the operation
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Factory method to create a successful result with a specified message.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result<T> Ok(T value, string message = "") => new Result<T>() { Value = value, Message = message };

        public static Result<T> Fail(string error, List<string> errors = null, T value = default(T)) => new Result<T>()
        {
            Value = value,
            Error = error,
            Errors = errors
        };

        public static Result<T> Fail(Exception ex, HttpStatusCode code = HttpStatusCode.InternalServerError, T value = default(T)) => new Result<T>()
        {
            Error = ex.Message,
            BaseException = ex.InnerException?.Message,
            StackTrace = ex.StackTrace,
            StatusCode = code,
            Value = value
        };

        public static Result<T> Fail(DomainException ex, HttpStatusCode code = HttpStatusCode.UnprocessableEntity, T value = default(T)) => new Result<T>()
        {
            Error = ex.Message,
            Errors = ex.Errors.ToList(),
            BaseException = ex.InnerException?.Message,
            StackTrace = ex.StackTrace,
            StatusCode = code,
            Value = value
        };

    }
}