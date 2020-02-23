using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rixian.Extensions.AspNetCore.Api
{
    public class ApiStartupBuilder : StartupBuilder
    {
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            ILogger logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<ApiStartupBuilder>();

            ApiConfig? options = context.Configuration.GetSection("Api")?.Get<ApiConfig>();
            DateTime? defaultVersion = null;

            if (options == null)
            {
                // Do nothing.
                logger.LogWarning("[API] No configuration section named 'Api' found.");
            }

            options?.EnsureRequiredValues();
            if (DateTime.TryParse(options?.DefaultVersion, out DateTime version))
            {
                defaultVersion = version;
                logger.LogInformation("[API] Found default api version. DefaultVersion: {DefaultVersion}", defaultVersion?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
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
