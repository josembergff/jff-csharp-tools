using System.Net;
using JffCsharpTools.Domain.Model;
using Microsoft.AspNetCore.Mvc;

public static class DefaultResponseModelExtension
{
    public static ActionResult<T> ReturnResult<T>(this DefaultResponseModel<T> returnObj)
    {
        if (returnObj != null && returnObj.Success)
        {
            return new OkObjectResult(returnObj.Result);
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