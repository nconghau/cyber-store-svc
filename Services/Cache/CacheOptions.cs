﻿using Microsoft.Extensions.Caching.Distributed;

namespace CyberStoreSVC.Services.Cache
{
    public static class CacheOptions
    {
        public static DistributedCacheEntryOptions DefaultExpiration => new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
        };

        public static DistributedCacheEntryOptions Create(TimeSpan? expiration)
        {
            return expiration is not null
                ? new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration }
                : DefaultExpiration;
        }
    }
}
