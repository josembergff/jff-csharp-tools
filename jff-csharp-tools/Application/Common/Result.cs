
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
        /// Factory method to create a successful result with default values
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result Ok(T value) => new Result<T>() { Value = value };

        /// <summary>
        /// Factory method to create a successful result with a specified message.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result Ok(T value, string message) => new Result<T>() { Value = value, Message = message };
    }
}