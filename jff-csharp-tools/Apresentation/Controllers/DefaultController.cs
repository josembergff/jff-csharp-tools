using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using JffCsharpTools.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Apresentation.Controllers
{
    public class DefaultController : ControllerBase
    {
        protected int GetCurrentIdUser_FromToken
        {
            get
            {
                int idReturn = -1;
                if (User != null && User.HasClaim(f => f.Type == "id"))
                {
                    int.TryParse(User.FindFirstValue("id"), out idReturn);
                }
                return idReturn;
            }
        }

        protected string GetCurrentNameUser_FromToken
        {
            get
            {
                string name = "n/a";
                if (User != null && User.HasClaim(f => f.Type == "name"))
                {
                    name = User.FindFirstValue("name") ?? "n/a";
                }
                return name;
            }
        }

        protected string GetCurrentToken_FromHeader
        {
            get
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    return authHeader.Substring("Bearer ".Length).Trim();
                }
                return string.Empty;
            }
        }

        protected string GetCurrentRole
        {
            get
            {
                string role = "n/a";
                if (User != null && User.HasClaim(f => f.Type == ClaimTypes.Role))
                {
                    role = string.Join(',', User.FindAll(ClaimTypes.Role).Select(s => s.Value)) ?? "n/a";
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
                var returnAction = new DefaultResponseModel<TRetorno>();
                returnAction.Message = "Error! An internal server error has occurred, please contact your system administrator.";
                returnAction.Error = ex.Message;
                return Problem(returnAction.Error, statusCode: (int)HttpStatusCode.InternalServerError, title: returnAction.Message);
            }
        }
    }
}