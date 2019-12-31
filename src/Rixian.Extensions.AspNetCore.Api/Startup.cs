// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

[assembly: Microsoft.AspNetCore.Hosting.HostingStartup(typeof(Rixian.Extensions.AspNetCore.Api.Startup))]

namespace Rixian.Extensions.AspNetCore.Api
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Additional api startup services.
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

                    ApiConfig options = context.Configuration.GetSection("Api").Get<ApiConfig>();

                    DateTime? defaultVersion = null;
                    if (DateTime.TryParse(options?.DefaultVersion, out DateTime version))
                    {
                        defaultVersion = version;
                    }

                    services
                        .AddApiExplorerServices()
                        .AddMvcServices()
                        .AddCorsServices()
                        .AddAuthorizationServices()
                        .AddApiVersioningServices(defaultVersion);

                    services.AddTransient<IStartupFilter, CorsStartupFilter>();
                });
        }
    }
}
