using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, MyDbContext dbContext)
    {
        var controllerName = context.Request.RouteValues["controller"]?.ToString();
        var actionName = context.Request.RouteValues["action"]?.ToString();
        var userRoles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        var authorizedRoles = await dbContext.ControllerActionRoles
            .Where(car => car.ControllerName == controllerName && car.ActionName == actionName)
            .Select(car => car.Role)
            .ToListAsync();

        IEnumerable<string> roleNames = authorizedRoles.Select(r => r.Name);
        if (authorizedRoles.Any() && userRoles.Intersect(roleNames).Any())
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = 403;
            return;
        }
    }
}