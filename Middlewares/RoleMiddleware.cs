using System.Security.Claims;
using Demo.Entities;
using Demo.Interfaces;
using Microsoft.AspNetCore.Identity;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRedisCacheService _redisCacheService;
    
    public RoleMiddleware(RequestDelegate next, IRedisCacheService redisCacheService)
    {
        _next = next;
        _redisCacheService = redisCacheService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var redisKey = $"user:roles:{userId.Value}";
            var roles = await _redisCacheService.SetMembersAsync(redisKey);
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    context.User.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, role) }));
                }
            }
        }

        await _next(context);
    }
}