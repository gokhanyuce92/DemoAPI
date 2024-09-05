using Serilog.Core;
using Serilog.Events;

public class UserNameEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserNameEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (userName != null)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserName", userName));
        }
    }
}