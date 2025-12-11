using System;
using System.Net;
using JffCsharpTools.Apresentation.Exceptions;
using JffCsharpTools.Domain.Enums;
using JffCsharpTools.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JffCsharpTools9.Apresentation.Controllers
{
    /// <summary>
    /// Base controller class that provides common functionality for token handling, user identification, 
    /// and standardized response formatting for API controllers
    /// </summary>
    public class DefaultController : ControllerBase
    {
        /// <summary>
        /// Logger instance for logging errors and information
        /// </summary>
        private readonly ILogger<DefaultController> logger;

        /// <summary>
        /// Initializes a new instance of the DefaultController
        /// </summary>
        /// <param name="logger">Logger instance for error and information logging</param>
        public DefaultController(ILogger<DefaultController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the current user ID from the Bearer token claims.
        /// Looks for custom 'idUser' claim first, then falls back to standard NameIdentifier claim
        /// </summary>
        protected int CurrentIdUser_FromBearerToken
        {
            get
            {
                return HttpContext.CurrentUserId();
            }
        }

        protected Guid CurrentGuidUser_FromBearerToken
        {
            get
            {
                return HttpContext.CurrentGuidUserId();
            }
        }

        /// <summary>
        /// Extracts specific information from the Bearer token using a custom parameter name
        /// </summary>
        /// <param name="parameter">The name of the claim to retrieve from the token</param>
        /// <returns>The value of the specified claim or "n/a" if not found</returns>
        protected string GetInfor_FromBearerToken(string parameter)
        {
            return HttpContext.GetInforCurrentUser(parameter);
        }

        /// <summary>
        /// Extracts specific information from the Bearer token using a predefined token parameter enum
        /// </summary>
        /// <param name="parameter">The token parameter enum value to retrieve from the token</param>
        /// <returns>The value of the specified claim or "n/a" if not found</returns>
        protected string GetInfor_FromBearerToken(TokenParameterEnum parameter)
        {
            return HttpContext.GetInforCurrentUser(parameter);
        }

        /// <summary>
        /// Gets the raw JWT token from the Authorization Bearer header
        /// </summary>
        protected string CurrentToken_FromAuthorizationBearer
        {
            get
            {
                return HttpContext.GetTokenCurrentUser();
            }
        }

        /// <summary>
        /// Gets all user roles from the Bearer token as a comma-separated string
        /// </summary>
        protected string CurrentRole_FromBearerToken
        {
            get
            {
                return HttpContext.GetRolesCurrentUser();
            }
        }

        /// <summary>
        /// Standardizes API response formatting based on the DefaultResponseModel result
        /// </summary>
        /// <typeparam name="TRetorno">The type of data being returned</typeparam>
        /// <param name="returnObj">The response model containing success status, data, and error information</param>
        /// <returns>Formatted ActionResult with appropriate HTTP status code</returns>
        protected ActionResult<TRetorno> ReturnAction<TRetorno>(DefaultResponseModel<TRetorno> returnObj)
        {
            if (returnObj != null && returnObj.Success)
            {
                return Ok(returnObj.Result);
            }
            else if (returnObj != null && returnObj.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(returnObj);
            }
            else
            {
                return BadRequest(returnObj);
            }
        }

        /// <summary>
        /// Extracts specific information from the access token by parsing JWT claims
        /// </summary>
        /// <param name="parameterName">The token parameter enum to extract from the JWT</param>
        /// <param name="required">Whether the parameter is required (throws exception if not found when true)</param>
        /// <returns>The value of the specified claim or empty string if not found</returns>
        /// <exception cref="TokenException">Thrown when required parameter is not found and required=true</exception>
        protected string GetInfor_FromAccesToken(TokenParameterEnum parameterName, bool required = false)
        {
            return HttpContext.GetInforFromToken(parameterName, required);
        }

        /// <summary>
        /// Extracts the Bearer token from the Authorization header
        /// </summary>
        /// <returns>The JWT token string without the "Bearer " prefix, or empty string if not found</returns>
        protected string GetToken_FromAccesToken()
        {
            return HttpContext.GetTokenCurrentUser();
        }
    }
}