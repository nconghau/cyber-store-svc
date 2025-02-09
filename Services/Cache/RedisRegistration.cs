using CyberStoreSVC.Services.Cache;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace CyberStoreSVC.Services.Redis
{
    public static class RedisRegistration
	{
        private static readonly string _redisConnectionString = "redis";
        public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheConnectionString = configuration.GetConnectionString(_redisConnectionString)!;
            try
            {
                IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(cacheConnectionString);
                services.TryAddSingleton(connectionMultiplexer);

                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
                });
            }
            catch
            {
                // HACK: Allows application to run without a Redis server.
                services.AddDistributedMemoryCache();
            }

            services.TryAddSingleton<ICacheService, CacheService>();
            return services;
        }
    }
}

