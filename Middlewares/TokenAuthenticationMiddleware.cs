using System.Security.Claims;
using Demo.Entities;
using Microsoft.AspNetCore.Identity;

public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    
    public TokenAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, UserManager<AppUser> userManager)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
        if (token != null)
        {
            var userNameClaim = context.User.Claims.FirstOrDefault(c => c.Type == "Username");
            if (userNameClaim != null)
            {
                var userName = userNameClaim.Value;
                var isValid = await userManager.VerifyUserTokenAsync(
                    await userManager.FindByNameAsync(userName),
                    "DataProtectorTokenProvider<AppUser>",
                    "Token",
                    token);
                if (!isValid)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }
            }
        }
        await _next(context);
    }
}