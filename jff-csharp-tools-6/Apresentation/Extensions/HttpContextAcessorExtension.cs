using System;
using Microsoft.AspNetCore.Http;

namespace JffCsharpTools6.Apresentation.Extensions
{
    /// <summary>
    /// Extension for IHttpContextAccessor to retrieve the Bearer token from the Authorization header of the HTTP context.
    /// </summary>
    public static class HttpContextAcessorExtension
    {
        /// <summary>
        /// Retrieves the Bearer token from the Authorization header of the HTTP context.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <returns>The Bearer token, or an empty string if not present.</returns>
        public static string GetBearerToken(this IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return string.Empty;
            }

            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return authorizationHeader["Bearer ".Length..].Trim();
        }
    }
}