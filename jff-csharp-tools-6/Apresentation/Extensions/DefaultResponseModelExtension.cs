using System.Net;
using JffCsharpTools.Application.Common;
using JffCsharpTools.Domain.Common;
using Microsoft.AspNetCore.Mvc;

public static class DefaultResponseModelExtension
{
    public static ActionResult<T> ReturnResult<T>(this Result<T> returnObj)
    {
        if (returnObj != null && returnObj.Success)
        {
            return new OkObjectResult(returnObj.Value);
        }
        else if (returnObj != null && returnObj.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new UnauthorizedResult();
        }
        else
        {
            return new BadRequestObjectResult(returnObj);
        }
    }
}