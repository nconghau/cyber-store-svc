using System.Threading.RateLimiting;

namespace CyberStoreSVC.Services.Security
{
    public static class RateLimitRegistration
    {
        public static void AddRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        ipAddress,
                        _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 5, // Max 5 requests
                            Window = TimeSpan.FromSeconds(10), // In 10 seconds
                            SegmentsPerWindow = 5, // 5 segments (each 2s)
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 5 // Allow 5 extra requests in queue
                        }
                    );
                });
            });
        }
    }
}

