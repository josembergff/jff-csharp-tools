using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JffCsharpTools.Domain.Model
{
    /// <summary>
    /// Generic default response model for application operations.
    /// Centralizes error, success, and return data information.
    /// </summary>
    /// <typeparam name="T">Type of the result object to be returned</typeparam>
    public class DefaultResponseModel<T>
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
        public List<string> MessageList { get; set; } = new List<string>();

        /// <summary>
        /// Result data of the operation
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Indicates whether the operation was executed successfully.
        /// Checks if status is OK/NoContent and there are no error messages.
        /// </summary>
        public bool Success
        {
            get
            {
                var checkStatus = StatusCode == HttpStatusCode.OK || StatusCode == HttpStatusCode.NoContent;
                var checkMessage = string.IsNullOrEmpty(Message) && MessageList?.Any() != true;
                var checkResult = checkStatus && checkMessage;
                return checkResult;
            }
        }
    }
}