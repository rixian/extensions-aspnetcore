// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.StackExchangeRedis
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Additional Redis startup services.
    /// </summary>
    public class StackExchangeRedisStartupBuilder : StartupBuilder
    {
        /// <inheritdoc/>
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ILogger<StackExchangeRedisStartupBuilder> logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<StackExchangeRedisStartupBuilder>();
            RedisConfig? options = context.Configuration.GetSection("Redis")?.Get<RedisConfig>();
            if (options == null)
            {
                if (context.HostingEnvironment.IsDevelopment())
                {
                    // Do nothing.
                    logger.LogWarning(Properties.Resources.ConfigurationMissingMessage);
                }
                else
                {
                    throw new InvalidOperationException(Properties.Resources.RequiredConfigurationMissingMessage);
                }
            }
            else
            {
                Errors.Result isValid = options.CheckRequiredValues(); // Provides the required null checks.

                if (isValid.IsSuccess)
                {
                    services.AddStackExchangeRedisCache(o =>
                    {
                        o.Configuration = options.Configuration;
                        o.InstanceName = options.InstanceName;
                    });

                    services
                        .AddHealthChecks()
                        .AddRedis(options.Configuration);

                    logger.LogInformation(Properties.Resources.ConfigurationFoundMessage, options?.InstanceName);
                }
                else if (context.HostingEnvironment.IsDevelopment())
                {
                    logger.LogWarning(Properties.Resources.InvalidConfigurationMessage, JsonConvert.SerializeObject(isValid.Error, Formatting.Indented));
                    services.AddDistributedMemoryCache();
                }
                else
                {
                    throw new ErrorException(isValid.Error, Properties.Resources.ConfigurationRequiredMessage);
                }
            }
        }
    }
}
