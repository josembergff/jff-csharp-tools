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
    /// <summary>
    /// Action filter that provides role-based authorization using JWT tokens and enum-based roles
    /// Validates JWT tokens and checks if the user has the required roles to access the action
    /// Works in conjunction with AttributeEnum to define authorized roles for specific actions
    /// </summary>
    /// <typeparam name="T">The enum type representing the application roles</typeparam>
    public class TokenEnumFilter<T> : IActionFilter where T : Enum
    {
        /// <summary>
        /// Executes before the action method runs
        /// Validates JWT token and checks role-based authorization
        /// </summary>
        /// <param name="context">The action executing context containing request and action information</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var rolesAction = new List<T>();
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            // Get the AttributeEnum attribute from the action method to determine required roles
            var customAttribute = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AttributeEnum), false).FirstOrDefault() as AttributeEnum;
            if (customAttribute != null)
            {
                // Convert string role names to enum values
                rolesAction = customAttribute.Roles.Select(s => (T)Enum.Parse(typeof(T), s)).ToList();

                // Extract JWT token from Authorization header
                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                // Reject request if no token is provided
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                try
                {
                    // Parse and validate the JWT token
                    var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                    // Check if token is valid and not expired
                    if (jwtToken == null || jwtToken.ValidTo.ToUniversalTime() <= DateTime.UtcNow)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    // Extract user roles from JWT token claims
                    var roles = jwtToken.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList();

                    // Check if user has any of the required roles for this action
                    if (rolesAction?.Any() == true && !rolesAction.Any(r => roles.Contains(r.ToString())))
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
                catch
                {
                    // Return unauthorized if token parsing or validation fails
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            return;
        }

        /// <summary>
        /// Executes after the action method completes
        /// Currently performs no operations
        /// </summary>
        /// <param name="context">The action executed context containing result information</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do nothing
        }
    }
}