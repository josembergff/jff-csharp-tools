using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using JffCsharpTools.Apresentation.Exceptions;
using JffCsharpTools.Domain.Enums;
using JffCsharpTools.Domain.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JffCsharpTools6.Apresentation.Controllers
{
    /// <summary>
    /// Base controller providing common functionality for API controllers
    /// Includes JWT token parsing, user identity extraction, and standardized response handling
    /// </summary>
    public class DefaultController : ControllerBase
    {
        /// <summary>
        /// Logger instance for recording application events and errors
        /// </summary>
        private readonly ILogger<DefaultController> logger;

        /// <summary>
        /// Initializes a new instance of the DefaultController
        /// </summary>
        /// <param name="logger">Logger instance for recording application events and errors</param>
        public DefaultController(ILogger<DefaultController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the current user ID from the JWT bearer token
        /// Attempts to extract user ID from custom claim or standard NameIdentifier claim
        /// </summary>
        protected int CurrentIdUser_FromBearerToken
        {
            get
            {
                int id = 0;
                if (User != null && User.HasClaim(f => f.Type == TokenParameterEnum.idUser.ToString()))
                {
                    id = Convert.ToInt32(User.FindFirstValue(TokenParameterEnum.idUser.ToString()));
                }
                else
                {
                    if (User != null && User.HasClaim(f => f.Type == ClaimTypes.NameIdentifier))
                    {
                        int idUser = 0;
                        if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out idUser))
                        {
                            id = idUser;
                        }
                    }
                    else
                    {
                        logger.LogError("Error! The user id was not found in the token.");
                    }
                }
                return id;
            }
        }

        /// <summary>
        /// Extracts information from the JWT bearer token using a string parameter name
        /// </summary>
        /// <param name="parameter">The claim type to search for in the token</param>
        /// <returns>The claim value if found, "n/a" otherwise</returns>
        protected string GetInfor_FromBearerToken(string parameter)
        {
            string name = "n/a";
            if (User != null && User.HasClaim(f => f.Type == parameter))
            {
                name = User.FindFirstValue(parameter) ?? "n/a";
            }
            else
            {
                logger.LogError($"Error! The user {parameter} was not found in the token.");
            }
            return name;
        }

        /// <summary>
        /// Extracts information from the JWT bearer token using a TokenParameterEnum
        /// </summary>
        /// <param name="parameter">The token parameter enum value to search for in the token</param>
        /// <returns>The claim value if found, "n/a" otherwise</returns>
        protected string GetInfor_FromBearerToken(TokenParameterEnum parameter)
        {
            string name = "n/a";
            if (User != null && User.HasClaim(f => f.Type == parameter.ToString()))
            {
                name = User.FindFirstValue(parameter.ToString()) ?? "n/a";
            }
            else
            {
                logger.LogError($"Error! The user {parameter} was not found in the token.");
            }
            return name;
        }

        /// <summary>
        /// Gets the current JWT token from the Authorization header
        /// Extracts the token from "Bearer {token}" format
        /// </summary>
        protected string CurrentToken_FromAuthorizationBearer
        {
            get
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    return authHeader.Substring("Bearer ".Length).Trim();
                }
                else
                {
                    logger.LogError("Error! The token was not found in the header.");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the current user roles from the JWT bearer token
        /// Combines multiple role claims into a comma-separated string
        /// </summary>
        protected string CurrentRole_FromBearerToken
        {
            get
            {
                string role = "n/a";
                if (User != null && User.HasClaim(f => f.Type == ClaimTypes.Role))
                {
                    role = string.Join(',', User.FindAll(ClaimTypes.Role).Select(s => s.Value)) ?? "n/a";
                }
                else
                {
                    logger.LogError("Error! The user role was not found in the token.");
                }
                return role;
            }
        }

        /// <summary>
        /// Standardized method for returning API responses based on the service result
        /// Handles success, unauthorized, and error scenarios with appropriate HTTP status codes
        /// </summary>
        /// <typeparam name="TRetorno">The type of data being returned in the response</typeparam>
        /// <param name="returnObj">The service response model containing result data and metadata</param>
        /// <returns>Appropriate ActionResult with correct HTTP status code and response body</returns>
        protected ActionResult<TRetorno> ReturnAction<TRetorno>(DefaultResponseModel<TRetorno> returnObj)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Error! An internal server error has occurred, please contact your system administrator.");
                var returnAction = new DefaultResponseModel<TRetorno>();
                returnAction.Message = "Error! An internal server error has occurred, please contact your system administrator.";
                returnAction.Error = ex.Message;
                return Problem(returnAction.Error, statusCode: (int)HttpStatusCode.InternalServerError, title: returnAction.Message);
            }
        }

        /// <summary>
        /// Extracts information from the access token using a TokenParameterEnum
        /// Can throw TokenException if token is required but not found
        /// </summary>
        /// <param name="parameterName">The token parameter enum value to search for</param>
        /// <param name="required">Whether the token is required (throws exception if missing and required)</param>
        /// <returns>The claim value if found, empty string otherwise</returns>
        protected string GetInfor_FromAccesToken(TokenParameterEnum parameterName, bool required = false)
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            accessToken = string.IsNullOrEmpty(accessToken) ? GetToken_FromAccesToken() : accessToken;
            if (string.IsNullOrEmpty(accessToken))
            {
                if (required)
                {
                    throw new TokenException("Token não informado.");
                }
                else
                {
                    return string.Empty;
                }
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var parameter = jwtToken.Claims.FirstOrDefault(claim => claim.Type == parameterName.ToString())?.Value ?? "";
            return parameter;
        }

        /// <summary>
        /// Extracts the access token from the Authorization header
        /// Removes the "Bearer " prefix and returns the clean token
        /// </summary>
        /// <returns>The access token string or empty string if not found</returns>
        protected string GetToken_FromAccesToken()
        {
            if (HttpContext.Request != null && HttpContext.Request.Headers != null)
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.ToString().StartsWith("Bearer "))
                {
                    var accessToken = authorizationHeader.ToString().Substring("Bearer ".Length).Trim();
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        return accessToken;
                    }
                }
            }
            return string.Empty;
        }
    }
}