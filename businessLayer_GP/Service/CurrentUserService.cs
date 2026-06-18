using GradProject.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                throw new Exception("User not authenticated");

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)
                              ?? user.FindFirst("nameid");

            if (userIdClaim == null)
                throw new Exception("UserId not found in token");

            return int.Parse(userIdClaim.Value);
        }
    }
}