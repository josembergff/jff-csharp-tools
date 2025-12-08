using System.Net;
using JffCsharpTools.Domain.Model;
using Microsoft.AspNetCore.Http;

public static class DefaultResponseModelExtension
{
    public static IResult ReturnResult<T>(this DefaultResponseModel<T> returnObj)
    {
        if (returnObj != null && returnObj.Success)
        {
            return Results.Ok(returnObj.Result);
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