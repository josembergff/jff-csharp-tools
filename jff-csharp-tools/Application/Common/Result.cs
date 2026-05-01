
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
    }
}