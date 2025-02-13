using CyberStoreSVC.Services.Cache;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace CyberStoreSVC.Services.Redis
{
    public static class RedisRegistration
	{
        private static readonly string _redisConnectionString = "14.225.204.163:6379,password=cyber_store";
        public static IServiceCollection AddRedisServices(this IServiceCollection services)
        {
            //var cacheConnectionString = configuration.GetConnectionString(_redisConnectionString)!;
            try
            {
                IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(_redisConnectionString);
                services.TryAddSingleton(connectionMultiplexer);

                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
                });
            }
            catch(Exception ex)
            {
                // HACK: Allows application to run without a Redis server.
                Console.WriteLine("AddRedisServices::" + ex.Message);
                services.AddDistributedMemoryCache();
            }

            services.TryAddSingleton<ICacheService, CacheService>();
            return services;
        }
    }
}

