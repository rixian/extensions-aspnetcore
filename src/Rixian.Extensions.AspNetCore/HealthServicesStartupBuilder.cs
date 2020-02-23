using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Rixian.Extensions.AspNetCore.Api
{
    public class HealthServicesStartupBuilder : StartupBuilder
    {
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


        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());
        }
    }
}
