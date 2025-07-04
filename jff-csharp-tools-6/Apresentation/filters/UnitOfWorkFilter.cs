using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools6.Apresentation.Filters
{
    /// <summary>
    /// Action filter that implements the Unit of Work pattern for database transactions
    /// Automatically saves changes to the database when an action completes successfully
    /// Ensures transactional integrity by only persisting changes if no exceptions occur
    /// </summary>
    /// <typeparam name="T">The DbContext type used for database operations</typeparam>
    public class UnitOfWorkFilter<T> : IActionFilter where T : DbContext
    {
        /// <summary>
        /// The database context instance used for managing database operations
        /// </summary>
        private readonly T customContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWorkFilter
        /// </summary>
        /// <param name="customContext">The database context instance to manage transactions for</param>
        public UnitOfWorkFilter(T customContext)
        {
            this.customContext = customContext;
        }

        /// <summary>
        /// Executes after the action method completes
        /// Saves changes to the database only if no exceptions occurred during action execution
        /// </summary>
        /// <param name="context">The action executed context containing result and exception information</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Only save changes if the action completed without exceptions
            if (context.Exception == null)
            {
                customContext.SaveChanges();
            }
            // If an exception occurred, changes will be automatically rolled back
            // when the context is disposed or when the transaction scope ends
        }

        /// <summary>
        /// Executes before the action method runs
        /// Currently performs no operations - could be extended for transaction setup
        /// </summary>
        /// <param name="context">The action executing context containing request information</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // No operations performed before action execution
            // This could be extended to begin explicit transactions if needed
        }
    }
}