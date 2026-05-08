using Microsoft.AspNetCore.Http;
using SINARC.Core.CrossCutting.Config.Extensions;

namespace JffCsharpTools6.Apresentation.Providers;

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