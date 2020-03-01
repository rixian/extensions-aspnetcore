// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.Api
{
    using System;
    using System.Globalization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Startup configuration for a standard web api.
    /// </summary>
    public class ApiStartupBuilder : StartupBuilder
    {
        /// <inheritdoc/>
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ILogger logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<ApiStartupBuilder>();

            ApiConfig? options = context.Configuration.GetSection("Api")?.Get<ApiConfig>();
            DateTime? defaultVersion = null;

            if (options == null)
            {
                // Do nothing.
                logger.LogWarning(Properties.Resources.ConfigurationNotFoundMessage);
            }

            options?.EnsureRequiredValues();
            if (DateTime.TryParse(options?.DefaultVersion, out DateTime version))
            {
                defaultVersion = version;
                logger.LogInformation(Properties.Resources.FoundDefaultVersionMessage, defaultVersion?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

            services.AddControllers();

            services
                    .AddApiExplorerServices()
                    .AddMvcServices()
                    .AddCorsServices()
                    .AddAuthorizationServices()
                    .AddApiVersioningServices(defaultVersion);
        }
    }
}
