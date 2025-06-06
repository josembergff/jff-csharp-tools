using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools8.Apresentation.Filters
{
    public class UnitOfWorkFilter<T> : IActionFilter where T : DbContext
    {
        private readonly T customContext;

        public UnitOfWorkFilter(T customContext)
        {
            this.customContext = customContext;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                customContext.SaveChanges();
            }
        }
        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}