// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using Rixian.Extensions.ApplicationInsights;

    /// <summary>
    /// Basic extensions for IServiceCollection for AspNet Core.
    /// </summary>
    public static class AppInsightsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the basic required Application Insights pieces for running an Asp.Net site.
        /// </summary>
        /// <param name="services">The IServiceCollection.</param>
        /// <param name="config">The Application Insights Configuration.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddApplicationInsights(IServiceCollection services, ApplicationInsightsConfig config)
        {
            services.Configure<ApplicationInsightsConfig>(o =>
            {
                o.HostName = config.HostName;
                o.CloudRoleName = config.CloudRoleName;
                o.IgnoredEndpoints = config.IgnoredEndpoints;
            });
            services.AddApplicationInsightsTelemetryWorkerService();
            services.AddApplicationInsightsTelemetryProcessor<HealthCheckTelemetryProcessor>();

            return services;
        }
    }
}
