// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore
{
    using System;
    using System.Linq;
    using System.Net.Mime;
    using System.Text.Json;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// The StartupBuilder for health endpoints.
    /// </summary>
    public class HealthServicesStartupBuilder : StartupBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthServicesStartupBuilder"/> class.
        /// </summary>
        public HealthServicesStartupBuilder()
        {
            this.PreviewUseEndpoints = (configuration, environment, app) =>
            {
                app
                  .UseHealthChecks("/self", new HealthCheckOptions
                  {
                      Predicate = r => r.Name.Contains("self", StringComparison.OrdinalIgnoreCase),
                      ResponseWriter = async (context, report) =>
                      {
                          var result = JsonSerializer.Serialize(
                              new
                              {
                                  status = report.Status.ToString(),
                                  errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) }),
                              });
                          context.Response.ContentType = MediaTypeNames.Application.Json;
                          await context.Response.WriteAsync(result).ConfigureAwait(false);
                      },
                  })
                  .UseHealthChecks("/ready", new HealthCheckOptions
                  {
                      Predicate = r => r.Tags.Contains("services"),
                      ResponseWriter = async (context, report) =>
                      {
                          var result = JsonSerializer.Serialize(
                              new
                              {
                                  status = report.Status.ToString(),
                                  errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) }),
                              });
                          context.Response.ContentType = MediaTypeNames.Application.Json;
                          await context.Response.WriteAsync(result).ConfigureAwait(false);
                      },
                  });
            };
        }

        /// <inheritdoc/>
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());
        }
    }
}
