using System.Security.Claims;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

public class IsOwnerRequirement : IAuthorizationRequirement
{
}

public class IsOwnerRequirementHandler : AuthorizationHandler<IsOwnerRequirement>
{
    private readonly DataContext _dataContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsOwnerRequirementHandler(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
    {
        _dataContext = dataContext;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerRequirement requirement)
    {
        var userId = context.User.FindFirstValue(Konstants.ClaimTypes.DeviceId.ToString());

        if (userId is null) return Task.CompletedTask;

        var diaryId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString()!);

        var diary = _dataContext.Diaries.AsNoTracking()
            .Include(o => o.Owner)
            .FirstOrDefaultAsync(x => x.OwnerId == userId && x.Id == diaryId).Result;

        var owner = diary.Owner;
        
        if (owner is not null) context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}