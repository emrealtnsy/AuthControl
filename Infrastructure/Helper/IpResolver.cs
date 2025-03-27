using System.Net;

namespace AuthControl.Infrastructure.Helper;

public class IpResolver(IHttpContextAccessor httpContextAccessor)
{
    private static readonly string[] IpHeaders = ["X-Forwarded-For", "X-Real-IP", "Forwarded"];
    
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext 
                                                ?? throw new InvalidOperationException("HttpContext is null.");    
    public string? GetIpAddress()
    {
        return GetRemoteIp(_httpContext) ?? GetLocalIp(_httpContext) ?? null;
    }
    
    private string? GetLocalIp(HttpContext httpContext)
    {
        var ipAddress = httpContext.Connection.RemoteIpAddress;
        
        if (!IPAddress.IsLoopback(ipAddress!)) return null;
        var localIp = httpContext.Connection.LocalIpAddress?.ToString() ?? null;
        var localPort = httpContext.Connection.LocalPort;
        return $"{localIp}:{localPort}";
    }

    private string? GetRemoteIp(HttpContext context)
    {
        foreach (var header in IpHeaders)
        {
            if (context.Request.Headers.TryGetValue(header, out var values))
            {
                var ip = values.ToString()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(ipPart => ipPart.Trim())
                    .FirstOrDefault(IsValidIp);

                if (!string.IsNullOrWhiteSpace(ip))
                    return ip;
            }
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? null;
    }

    private bool IsValidIp(string ip)
    {
        return IPAddress.TryParse(ip, out var parsedIp) && 
               parsedIp.AddressFamily switch
               {
                   System.Net.Sockets.AddressFamily.InterNetwork => true,
                   System.Net.Sockets.AddressFamily.InterNetworkV6 => true,
                   _ => false
               };
    }
}