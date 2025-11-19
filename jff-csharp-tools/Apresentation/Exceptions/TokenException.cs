using System;

namespace JffCsharpTools.Apresentation.Exceptions
{
    /// <summary>
    /// Custom exception class for token-related errors in the application.
    /// This exception is thrown when token validation, parsing, or authentication fails.
    /// Inherits from the base Exception class and provides specific error handling for token operations.
    /// </summary>
    public class TokenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of TokenException with a default error message.
        /// Default message: "Token not found."
        /// </summary>
        public TokenException() : base("Token not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of TokenException with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the token-related error</param>
        public TokenException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of TokenException with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the token-related error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public TokenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}