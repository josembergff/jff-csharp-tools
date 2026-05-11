using System.Net;
using JffCsharpTools.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JffCsharpTools8.Apresentation.Extensions;

public static class DefaultResponseModelExtension
{
    public static ActionResult<T> ReturnActionResult<T>(this Result<T> returnObj)
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

    public static IResult ReturnResult<T>(this Result<T> returnObj)
    {
        if (returnObj != null && returnObj.Success)
        {
            return Results.Ok(returnObj.Value);
        }
        else if (returnObj != null && returnObj.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Results.Unauthorized();
        }
        else
        {
            return Results.BadRequest(returnObj);
        }
    }
}