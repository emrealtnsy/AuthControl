using System.Threading.RateLimiting;
using AuthControl.Infrastructure.Helper;

namespace AuthControl.WebAPI.Extensions;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddOptionalRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy<string>("login-rate-limit", context =>
            {
                var ip = GetIpAddress(context);
                
                return RateLimitPartition.GetSlidingWindowLimiter(
                    ip, _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 2,
                        AutoReplenishment = true,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    })!;
            });

            options.AddPolicy<string>("register-rate-limit", context =>
            {
                var ip = GetIpAddress(context);

                return RateLimitPartition.GetSlidingWindowLimiter(
                    ip, _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 2,
                        AutoReplenishment = true,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    })!;
            });

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var ip = GetIpAddress(context);

                return RateLimitPartition.GetFixedWindowLimiter(
                    ip, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 50,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    })!;
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }
    
    private static string? GetIpAddress(HttpContext context)
    {
        var ipResolver = context.RequestServices.GetRequiredService<IpResolver>();
        return ipResolver.GetIpAddress();
    }
}