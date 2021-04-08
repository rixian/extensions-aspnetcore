// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.ApplicationInsights.Extensibility;
    using Rixian.Extensions.ApplicationInsights.Kubernetes;

    /// <summary>
    /// Basic extensions for IServiceCollection for AspNet Core.
    /// </summary>
    public static class K8sAppInsightsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the required Application Insights pieces for running on Kubernetes.
        /// </summary>
        /// <param name="services">The IServiceCollection.</param>
        /// <param name="config">The Application Insights Configuration.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddApplicationInsightsForKubernetes(IServiceCollection services, K8sApplicationInsightsConfig config)
        {
            services.Configure<K8sApplicationInsightsConfig>(o =>
            {
                o.HostName = config.HostName;
                o.CloudRoleName = config.CloudRoleName;
                o.IgnoredEndpoints = config.IgnoredEndpoints;
                o.IgnoredNamespaces = config.IgnoredNamespaces;
            });
            services.AddApplicationInsightsKubernetesEnricher();
            services.AddSingleton<ITelemetryInitializer, K8sAppInsightsTelemetryInitializer>();

            return services;
        }
    }
}
