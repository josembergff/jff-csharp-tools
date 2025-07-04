using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Apresentation.Filters
{
    /// <summary>
    /// Action filter that implements the Unit of Work pattern for Entity Framework operations.
    /// Automatically saves changes to the database when an action completes successfully,
    /// or skips saving if an exception occurred during execution.
    /// </summary>
    /// <typeparam name="T">The Entity Framework DbContext type</typeparam>
    public class UnitOfWorkFilter<T> : IActionFilter where T : DbContext
    {
        /// <summary>
        /// The Entity Framework context instance for database operations
        /// </summary>
        private readonly T customContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWorkFilter
        /// </summary>
        /// <param name="customContext">The Entity Framework DbContext instance</param>
        public UnitOfWorkFilter(T customContext)
        {
            this.customContext = customContext;
        }

        /// <summary>
        /// Executes after the action method completes. 
        /// Automatically saves all pending changes to the database if no exception occurred.
        /// If an exception was thrown during action execution, changes are not saved (automatic rollback).
        /// </summary>
        /// <param name="context">The action executed context containing response and exception information</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                customContext.SaveChanges();
            }
        }

        /// <summary>
        /// Executes before the action method runs. Currently performs no operations.
        /// </summary>
        /// <param name="context">The action execution context containing request information</param>
        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}