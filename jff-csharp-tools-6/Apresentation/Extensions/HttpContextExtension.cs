using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using JffCsharpTools.Apresentation.Exceptions;
using JffCsharpTools.Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

public static class HttpContextExtensions
{
    public static int CurrentUserId(this HttpContext context)
    {
        int id = 0;
        if (context.User != null && context.User.HasClaim(f => f.Type == TokenParameterEnum.idUser.ToString()))
        {
            id = int.TryParse(context.User.FindFirstValue(TokenParameterEnum.idUser.ToString()), out id) ? id : 0;
        }
        else if (context.User != null && context.User.HasClaim(f => f.Type == ClaimTypes.NameIdentifier))
        {
            id = int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out id) ? id : 0;
        }
        else
        {
            id = int.TryParse(context.User.FindFirstValue(TokenParameterEnum.sub.ToString()), out id) ? id : 0;
        }
        return id;
    }

    public static Guid CurrentGuidUserId(this HttpContext context)
    {
        Guid id = Guid.Empty;
        if (context.User != null && context.User.HasClaim(f => f.Type == TokenParameterEnum.idUser.ToString()))
        {
            id = Guid.TryParse(context.User.FindFirstValue(TokenParameterEnum.idUser.ToString()), out id) ? id : Guid.Empty;
        }
        else if (context.User != null && context.User.HasClaim(f => f.Type == ClaimTypes.NameIdentifier))
        {
            id = Guid.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out id) ? id : Guid.Empty;
        }
        else
        {
            id = Guid.TryParse(context.User.FindFirstValue(TokenParameterEnum.sub.ToString()), out id) ? id : Guid.Empty;
        }
        return id;
    }

    public static string GetInforCurrentUser(this HttpContext context, string parameter)
    {
        string name = string.Empty;
        if (context.User != null && context.User.HasClaim(f => f.Type == parameter))
        {
            name = context.User.FindFirstValue(parameter) ?? string.Empty;
        }
        return name;
    }

    public static string GetInforCurrentUser(this HttpContext context, TokenParameterEnum parameter)
    {
        string name = string.Empty;
        if (context.User != null && context.User.HasClaim(f => f.Type == parameter.ToString()))
        {
            name = context.User.FindFirstValue(parameter.ToString()) ?? string.Empty;
        }
        return name;
    }

    public static string GetTokenCurrentUser(this HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }
        return string.Empty;
    }

    public static string GetRolesCurrentUser(this HttpContext context)
    {
        string role = "n/a";
        if (context.User != null && context.User.HasClaim(f => f.Type == ClaimTypes.Role))
        {
            role = string.Join(',', context.User.FindAll(ClaimTypes.Role).Select(s => s.Value)) ?? "n/a";
        }
        return role;
    }

    public static string GetInforFromToken(this HttpContext context, TokenParameterEnum parameterName, bool required = false)
    {
        var accessToken = context.GetTokenAsync("access_token").Result;
        accessToken = string.IsNullOrEmpty(accessToken) ? GetTokenCurrentUser(context) : accessToken;
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
}