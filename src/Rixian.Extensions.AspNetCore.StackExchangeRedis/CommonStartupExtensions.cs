using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rixian.Extensions.Errors;

namespace Rixian.Extensions.AspNetCore.StackExchangeRedis
{
    public static class CommonStartupExtensions
    {
        public static ICommonStartup AddRedis(this ICommonStartup startup)
        {
            RedisConfig? redisOptions = startup.Configuration.GetSection("Redis")?.Get<RedisConfig>();
            if (redisOptions == null)
            {
                if (startup.Environment.IsDevelopment())
                {
                    // Do nothing.
                    //logger.LogWarning(Properties.Resources.ConfigurationMissingMessage);
                }
                else
                {
                    throw new InvalidOperationException("");// Properties.Resources.RequiredConfigurationMissingMessage);
                }
            }
            else
            {
                Extensions.Errors.Result isValid = redisOptions.CheckRequiredValues(); // Provides the required null checks.

                if (isValid.IsSuccess)
                {
                    startup.Services.AddStackExchangeRedisCache(o =>
                    {
                        o.Configuration = redisOptions.Configuration;
                        o.InstanceName = redisOptions.InstanceName;
                    });

                    startup.Services
                        .AddHealthChecks()
                        .AddRedis(redisOptions.Configuration);

                    //logger.LogInformation(Properties.Resources.ConfigurationFoundMessage, redisOptions?.InstanceName);
                }
                else if (startup.Environment.IsDevelopment())
                {
                    //logger.LogWarning(Properties.Resources.InvalidConfigurationMessage, JsonConvert.SerializeObject(isValid.Error, Formatting.Indented));
                    startup.Services.AddDistributedMemoryCache();
                }
                else
                {
                    throw new ErrorException(isValid.Error, "");// Properties.Resources.ConfigurationRequiredMessage);
                }
            }

            return startup;
        }
    }
}
