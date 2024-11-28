using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using JffCsharpTools.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apresentation.Controllers
{
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> logger;

        DefaultController(ILogger<DefaultController> logger)
        {
            this.logger = logger;
        }

        protected int CurrentIdUser_FromBearerToken
        {
            get
            {
                int idReturn = -1;
                if (User != null && User.HasClaim(f => f.Type == "id"))
                {
                    int.TryParse(User.FindFirstValue("id"), out idReturn);
                }
                else
                {
                    logger.LogError("Error! The user id was not found in the token.");
                }
                return idReturn;
            }
        }

        protected string CurrentNameUser_FromBearerToken
        {
            get
            {
                string name = "n/a";
                if (User != null && User.HasClaim(f => f.Type == "name"))
                {
                    name = User.FindFirstValue("name") ?? "n/a";
                }
                else
                {
                    logger.LogError("Error! The user name was not found in the token.");
                }
                return name;
            }
        }

        protected string CurrentEmailUser_FromBearerToken
        {
            get
            {
                string name = "n/a";
                if (User != null && User.HasClaim(f => f.Type == "name"))
                {
                    name = User.FindFirstValue("name") ?? "n/a";
                }
                else
                {
                    logger.LogError("Error! The user email was not found in the token.");
                }
                return name;
            }
        }

        protected string GetCurrentInforUser_FromBearerToken(string parameterName)
        {
            string name = "n/a";
            if (User != null && User.HasClaim(f => f.Type == parameterName.ToLower()))
            {
                name = User.FindFirstValue(parameterName.ToLower()) ?? "n/a";
            }
            else
            {
                logger.LogError($"Error! The user {parameterName} was not found in the token.");
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
    }
}