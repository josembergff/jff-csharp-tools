
using System;
using System.Collections.Generic;
using JffCsharpTools.Application.Common;

namespace Jff.Csharp.Tools.Domain.Exceptions
{
    public class DomainException : Exception
    {
        /// <summary>
        /// Gets the list of error messages associated with this domain exception.
        /// </summary>
        public IReadOnlyList<string> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the DomainException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        public DomainException(string message)
            : base(message)
        {
            Errors = new List<string> { message };
        }

        /// <summary>
        /// Initializes a new instance of the DomainException class with a specified error message and inner exception.
        /// </summary>
        /// <param name="errors">The list of error messages associated with this domain exception</param>
        public DomainException(List<string> errors)
            : base("Domain validation failed")
        {
            Errors = errors;
        }

        /// <summary>
        /// Initializes a new instance of the DomainException class with a specified error message and inner exception.
        /// </summary>
        /// <param name="result">The Result object that contains error details</param>
        public DomainException(Result result)
            : base(result?.Error ?? "An error domain occurred")
        {
            Errors = result?.Errors ?? new List<string> { result?.Error ?? "Domain validation failed" };
        }

        /// <summary>
        /// Initializes a new instance of the DomainException class with a specified error message and inner exception.
        /// </summary>
        /// <param name="result">The Result object that contains error details</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public DomainException(Result result, Exception innerException)
            : base(result?.Error ?? "An error domain occurred", innerException)
        {
            Errors = result?.Errors ?? new List<string> { result?.Error ?? "Domain validation failed" };
        }
    }
}