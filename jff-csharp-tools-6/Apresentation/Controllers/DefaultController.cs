using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using JffCsharpTools.Domain.Enums;
using JffCsharpTools.Domain.Model;
using JffCsharpTools6.Apresentation.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JffCsharpTools6.Apresentation.Controllers
{
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            this.logger = logger;
        }

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
                    logger.LogError("Error! The user id was not found in the token.");
                }
                return id;
            }
        }

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

        protected ActionResult<TRetorno> ReturnAction<TRetorno>(DefaultResponseModel<TRetorno> returnObj)
        {
            try
            {
                if (returnObj != null && returnObj.Sucesso)
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