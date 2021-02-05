// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license.

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using System.Net.Mime;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// Helper extensions for IApplicationBuilder.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Forces the incoming request to appear to use the 'https' scheme.
        /// This is important for applications that are behind a reverse proxy that performs SSL offloading.
        /// </summary>
        /// <param name="app">The IApplicationBuilder.</param>
        /// <returns>The updated IApplicationBuilder.</returns>
        public static IApplicationBuilder UseHttpsScheme(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next();
            });

            return app;
        }

        /// <summary>
        /// Adds the health check endpoint at the path "/self" for all checks named "self".
        /// </summary>
        /// <param name="app">The IApplicationBuilder.</param>
        /// <param name="path">The path to use for this health check.</param>
        /// <returns>The updated IApplicationBuilder.</returns>
        public static IApplicationBuilder UseSelfHealthEndpoint(this IApplicationBuilder app, string path = "/self")
        {
            app.UseHealthChecks(path, new HealthCheckOptions
            {
                AllowCachingResponses = true,
                Predicate = r => r.Name.Contains("self"),
                ResponseWriter = async (context, report) =>
                {
                    var result = System.Text.Json.JsonSerializer.Serialize(
                        new
                        {
                            status = report.Status.ToString(),
                            errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) }),
                        });
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                },
            });

            return app;
        }

        /// <summary>
        /// Adds the health check endpoint at the path "/ready" for all checks tagged "services".
        /// </summary>
        /// <param name="app">The IApplicationBuilder.</param>
        /// <param name="path">The path to use for this health check.</param>
        /// <returns>The updated IApplicationBuilder.</returns>
        public static IApplicationBuilder UseServiceHealthEndpoint(this IApplicationBuilder app, string path = "/ready")
        {
            app.UseHealthChecks(path, new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("services"),
                ResponseWriter = async (context, report) =>
                {
                    var result = System.Text.Json.JsonSerializer.Serialize(
                        new
                        {
                            status = report.Status.ToString(),
                            errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) }),
                        });
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                },
            });

            return app;
        }
    }
}
