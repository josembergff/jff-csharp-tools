using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using JffCsharpTools9.Apresentation.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JffCsharpTools9.Apresentation.Filters
{
    /// <summary>
    /// Action filter that performs enum-based role authorization by validating JWT tokens.
    /// Works in conjunction with AttributeEnum to restrict access based on user roles defined in enums.
    /// </summary>
    /// <typeparam name="T">The enum type that represents the roles or permissions</typeparam>
    public class TokenEnumFilter<T> : IActionFilter where T : Enum
    {
        /// <summary>
        /// Executes before the action method runs, performing role-based authorization checks.
        /// Validates the JWT token and ensures the user has the required roles specified by AttributeEnum.
        /// </summary>
        /// <param name="context">The action execution context containing request information</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var rolesAction = new List<T>();
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var customAttribute = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AttributeEnum<T>), false).FirstOrDefault() as AttributeEnum<T>;
            if (customAttribute != null)
            {
                rolesAction = customAttribute.Roles.ToList();


                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                try
                {
                    var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                    if (jwtToken == null || jwtToken.ValidTo.ToUniversalTime() <= DateTime.UtcNow)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    var roles = jwtToken.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList();

                    if (rolesAction?.Any() == true && !rolesAction.Any(r => roles.Contains(r.ToString())))
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
                catch
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            return;
        }

        /// <summary>
        /// Executes after the action method completes. Currently performs no operations.
        /// </summary>
        /// <param name="context">The action executed context containing response information</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do nothing
        }
    }
}