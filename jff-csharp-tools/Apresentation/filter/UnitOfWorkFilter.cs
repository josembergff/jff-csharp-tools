using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace jff_csharp_tools.Apresentation.Filter
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