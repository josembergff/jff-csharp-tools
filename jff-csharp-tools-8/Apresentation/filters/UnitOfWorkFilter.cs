using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools8.Apresentation.Filters
{
    /// <summary>
    /// Action filter that implements the Unit of Work pattern for Entity Framework DbContext.
    /// This filter automatically saves changes to the database after successful action execution.
    /// If an exception occurs during action execution, changes are not saved, maintaining data consistency.
    /// This ensures that all database operations within a single action are treated as a single transaction.
    /// </summary>
    /// <typeparam name="T">The DbContext type that will be managed by this filter</typeparam>
    public class UnitOfWorkFilter<T> : IActionFilter where T : DbContext
    {
        /// <summary>
        /// The Entity Framework DbContext instance that will be managed by this filter
        /// </summary>
        private readonly T customContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWorkFilter class.
        /// </summary>
        /// <param name="customContext">The DbContext instance to be managed by this filter</param>
        public UnitOfWorkFilter(T customContext)
        {
            this.customContext = customContext;
        }

        /// <summary>
        /// Executes after the action method completes.
        /// Automatically saves all pending changes to the database if no exception occurred during action execution.
        /// This implements the Unit of Work pattern by treating the entire action as a single database transaction.
        /// </summary>
        /// <param name="context">The action executed context containing information about the completed action execution</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Only save changes if no exception occurred during action execution
            if (context.Exception == null)
            {
                customContext.SaveChanges();
            }
        }

        /// <summary>
        /// Executes before the action method is called.
        /// Currently performs no operations but is required by the IActionFilter interface.
        /// </summary>
        /// <param name="context">The action executing context containing information about the action about to be executed</param>
        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}