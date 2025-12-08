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
            id = Convert.ToInt32(context.User.FindFirstValue(TokenParameterEnum.idUser.ToString()));
        }
        else
        {
            if (context.User != null && context.User.HasClaim(f => f.Type == ClaimTypes.NameIdentifier))
            {
                int idUser = 0;
                if (int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out idUser))
                {
                    id = idUser;
                }
            }
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