using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using JffCsharpTools.Apresentation.Exceptions;
using JffCsharpTools.Domain.Enums;
using JffCsharpTools.Domain.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JffCsharpTools8.Apresentation.Controllers
{
    /// <summary>
    /// Base controller class that provides common functionality for authentication, token handling, and response formatting.
    /// This controller contains utilities for extracting user information from JWT tokens and managing API responses.
    /// All other controllers should inherit from this class to leverage the shared authentication and response handling logic.
    /// </summary>
    public class DefaultController : ControllerBase
    {
        /// <summary>
        /// Logger instance for logging errors, warnings, and informational messages
        /// </summary>
        private readonly ILogger<DefaultController> logger;

        /// <summary>
        /// Initializes a new instance of the DefaultController class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging throughout the controller</param>
        public DefaultController(ILogger<DefaultController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the current user ID from the JWT bearer token.
        /// First attempts to extract from custom claim 'idUser', then falls back to standard NameIdentifier claim.
        /// Returns 0 if no valid user ID is found and logs an error.
        /// </summary>
        /// <returns>The user ID as an integer, or 0 if not found</returns>
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
        /// Extracts a specific parameter value from the JWT bearer token using a string parameter name.
        /// </summary>
        /// <param name="parameter">The name of the claim/parameter to extract from the token</param>
        /// <returns>The parameter value as a string, or "n/a" if not found</returns>
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
        /// Extracts a specific parameter value from the JWT bearer token using a TokenParameterEnum.
        /// </summary>
        /// <param name="parameter">The enum value representing the claim/parameter to extract from the token</param>
        /// <returns>The parameter value as a string, or "n/a" if not found</returns>
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
        /// Gets the raw JWT token from the Authorization header.
        /// Extracts the token part after "Bearer " prefix.
        /// </summary>
        /// <returns>The JWT token string, or empty string if not found</returns>
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
        /// Gets all roles assigned to the current user from the JWT bearer token.
        /// Combines multiple role claims into a comma-separated string.
        /// </summary>
        /// <returns>Comma-separated string of user roles, or "n/a" if no roles found</returns>
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
        /// Standardizes API response formatting based on the success status and HTTP status code.
        /// Converts DefaultResponseModel into appropriate ActionResult with proper HTTP status codes.
        /// </summary>
        /// <typeparam name="TRetorno">The type of data being returned in the response</typeparam>
        /// <param name="returnObj">The response model containing success status, data, and error information</param>
        /// <returns>ActionResult with appropriate HTTP status code (200 OK, 400 Bad Request, or 401 Unauthorized)</returns>
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
        /// Extracts a specific parameter from the access token using TokenParameterEnum.
        /// Can enforce the parameter as required, throwing an exception if not found.
        /// </summary>
        /// <param name="parameterName">The enum value representing the parameter to extract</param>
        /// <param name="required">Whether the parameter is required (throws exception if true and parameter not found)</param>
        /// <returns>The parameter value as a string, or empty string if not found and not required</returns>
        /// <exception cref="TokenException">Thrown when required parameter is not found or token is missing</exception>
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
        /// Extracts the JWT token from the Authorization header of the current HTTP request.
        /// Validates the header format and extracts the token part after "Bearer " prefix.
        /// </summary>
        /// <returns>The JWT token string, or empty string if not found or invalid format</returns>
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