using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CyberStoreSVC.Models.Common;

namespace CyberStoreSVC.Services.Cache
{
    public static class RedisKeyBuilder
	{
        public static string GeneratePostgresQueryRedisKey(string prefix, PostgresQuery? query)
        {
            var sortedQuery = new PostgresQuery
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                Operator = query.Operator,
                Sort = query.Sort,
                Criteria = query.Criteria.OrderBy(c => c.Field).ToList()
            };

            var jsonString = JsonSerializer.Serialize(sortedQuery, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            string hash = ComputeMD5Hash(jsonString);

            return $"{prefix}_{hash}";
        }

        private static string ComputeMD5Hash(string input)
        {
            using var md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}

