using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using JffCsharpTools6.Apresentation.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JffCsharpTools6.Apresentation.Filters
{
    public class TokenEnumFilter<T> : IActionFilter where T : Enum
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var rolesAction = new List<T>();
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var customAttribute = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AttributeEnum), false).FirstOrDefault() as AttributeEnum;
            if (customAttribute != null)
            {
                rolesAction = customAttribute.Roles.Select(s => (T)Enum.Parse(typeof(T), s)).ToList();


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

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do nothing
        }
    }
}