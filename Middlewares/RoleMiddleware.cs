using System.Security.Claims;
using Demo.Entities;
using Demo.Interfaces;
using Microsoft.AspNetCore.Identity;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;
    
    public RoleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userId != null)
        {
        }

        await _next(context);
    }
}