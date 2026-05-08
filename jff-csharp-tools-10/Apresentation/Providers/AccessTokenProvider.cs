using JffCsharpTools10.Apresentation.Extensions;
using Microsoft.AspNetCore.Http;

namespace JffCsharpTools10.Apresentation.Providers;

public class AccessTokenProvider : IAccessTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccessTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetToken()
    {
        return _httpContextAccessor.GetBearerToken();
    }
}