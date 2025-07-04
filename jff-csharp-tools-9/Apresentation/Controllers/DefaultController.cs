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
        /// Extracts specific information from the Bearer token using a custom parameter name
        /// </summary>
        /// <param name="parameter">The name of the claim to retrieve from the token</param>
        /// <returns>The value of the specified claim or "n/a" if not found</returns>
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
        /// Extracts specific information from the Bearer token using a predefined token parameter enum
        /// </summary>
        /// <param name="parameter">The token parameter enum value to retrieve from the token</param>
        /// <returns>The value of the specified claim or "n/a" if not found</returns>
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
        /// Gets the raw JWT token from the Authorization Bearer header
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
        /// Gets all user roles from the Bearer token as a comma-separated string
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
        /// Standardizes API response formatting based on the DefaultResponseModel result
        /// </summary>
        /// <typeparam name="TRetorno">The type of data being returned</typeparam>
        /// <param name="returnObj">The response model containing success status, data, and error information</param>
        /// <returns>Formatted ActionResult with appropriate HTTP status code</returns>
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
        /// Extracts specific information from the access token by parsing JWT claims
        /// </summary>
        /// <param name="parameterName">The token parameter enum to extract from the JWT</param>
        /// <param name="required">Whether the parameter is required (throws exception if not found when true)</param>
        /// <returns>The value of the specified claim or empty string if not found</returns>
        /// <exception cref="TokenException">Thrown when required parameter is not found and required=true</exception>
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
        /// Extracts the Bearer token from the Authorization header
        /// </summary>
        /// <returns>The JWT token string without the "Bearer " prefix, or empty string if not found</returns>
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