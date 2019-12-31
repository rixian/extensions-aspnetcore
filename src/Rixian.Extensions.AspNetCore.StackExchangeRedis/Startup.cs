// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

[assembly: Microsoft.AspNetCore.Hosting.HostingStartup(typeof(Rixian.Extensions.AspNetCore.StackExchangeRedis.Startup))]

namespace Rixian.Extensions.AspNetCore.StackExchangeRedis
{
    /// <summary>
    /// Additional Redis startup services.
    /// </summary>
    public class Startup : IHostingStartup
    {
        /// <inheritdoc/>
        public void Configure(IWebHostBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder
                .ConfigureServices((context, services) =>
                {
                    ILogger<Startup> logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Startup>();

                    RedisConfig options = context.Configuration.GetSection("Redis").Get<RedisConfig>();

                    if (!string.IsNullOrWhiteSpace(options?.Configuration))
                    {
                        services.AddStackExchangeRedisCache(o =>
                        {
                            o.Configuration = options.Configuration;
                            o.InstanceName = options.InstanceName;
                        });

                        services
                            .AddHealthChecks()
                            .AddRedis(options.Configuration);
                    }
                    else if (context.HostingEnvironment.IsDevelopment())
                    {
                        logger.LogWarning("[REDIS] No Redis configuration specified, and running in Development. The In-Memory distributed cache will be enabled.");
                        services.AddDistributedMemoryCache();
                    }
                    else
                    {
                        throw new InvalidOperationException("Redis credentials must be provided for non-Development applications.");
                    }
                });
        }
    }
}
