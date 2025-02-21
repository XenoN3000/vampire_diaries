using System.Security.Claims;
using API.Helpers;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetDeviceId()
    {
        return _httpContextAccessor.HttpContext!.User.FindFirstValue(Konstants.ClaimTypes.DeviceId.ToString());
    }

    public string GetUsername()
    {
        throw new NotImplementedException();
    }
}