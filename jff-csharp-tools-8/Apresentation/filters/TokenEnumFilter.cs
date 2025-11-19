using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using JffCsharpTools8.Apresentation.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JffCsharpTools8.Apresentacao.Filters
{
    /// <summary>
    /// Action filter that provides role-based authorization using JWT tokens and enum-based role definitions.
    /// This filter validates JWT tokens and checks if the user has the required roles specified by the AttributeEnum attribute.
    /// It works with any enum type that defines roles or permissions in the system.
    /// The filter automatically validates token expiration and role membership before allowing action execution.
    /// </summary>
    /// <typeparam name="T">The enum type that defines the roles or permissions used for authorization</typeparam>
    public class TokenEnumFilter<T> : IActionFilter where T : Enum
    {
        /// <summary>
        /// Executes before the action method is called.
        /// Validates the JWT token and checks if the user has the required roles to access the action.
        /// Returns 401 Unauthorized if token is missing, invalid, or expired.
        /// Returns 403 Forbidden if user doesn't have the required roles.
        /// </summary>
        /// <param name="context">The action executing context containing request information and method metadata</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Collection to store required roles for the current action
            var rolesAction = new List<T>();

            // Get the action descriptor to access method information and custom attributes
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            // Look for the AttributeEnum<T> attribute on the action method
            var customAttribute = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AttributeEnum<T>), false).FirstOrDefault() as AttributeEnum<T>;

            if (customAttribute != null)
            {
                // Extract required roles from the attribute
                rolesAction = customAttribute.Roles.ToList();

                // Extract JWT token from Authorization header (Bearer token format)
                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                // Return 401 if no token is provided
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                try
                {
                    // Parse and validate the JWT token
                    var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                    // Check if token is null or expired
                    if (jwtToken == null || jwtToken.ValidTo.ToUniversalTime() <= DateTime.UtcNow)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    // Extract user roles from the token claims
                    var roles = jwtToken.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList();

                    // Check if user has any of the required roles
                    if (rolesAction?.Any() == true && !rolesAction.Any(r => roles.Contains(r.ToString())))
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
                catch
                {
                    // Return 401 for any token parsing or validation errors
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            return;
        }

        /// <summary>
        /// Executes after the action method completes.
        /// Currently does nothing but required by the IActionFilter interface.
        /// </summary>
        /// <param name="context">The action executed context containing the result of the action execution</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do nothing
        }
    }
}