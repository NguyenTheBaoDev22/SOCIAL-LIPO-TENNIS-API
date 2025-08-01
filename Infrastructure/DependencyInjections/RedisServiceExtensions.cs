using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjections
{
    public static class RedisServiceExtensions
    {
        public static IServiceCollection AddSmartRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = configuration.GetConnectionString("Redis");
            var useRedis = configuration.GetValue<bool>("UseRedis");

            if (useRedis && !string.IsNullOrWhiteSpace(redisConfig))
            {
                var configOptions = ConfigurationOptions.Parse(redisConfig);
                configOptions.ConnectTimeout = 500; // Giảm timeout xuống 500ms

                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConfigurationOptions = configOptions;
                    options.InstanceName = "ZenShopRedis:";
                });

                services.AddHostedService<RedisConnectionCheckerService>();

                Console.WriteLine("✅ Redis configured. Anti-spam will use Redis.");
            }
            else
            {
                services.AddDistributedMemoryCache();
                Console.WriteLine("⚠️ Redis not available – fallback to in-memory cache.");
            }

            return services;
        }
    }

}
