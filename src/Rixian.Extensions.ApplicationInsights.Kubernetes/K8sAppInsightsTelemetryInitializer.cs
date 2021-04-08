// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.ApplicationInsights.Kubernetes
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Telemetry initializer for setting up Kubernetes-related information.
    /// </summary>
    public class K8sAppInsightsTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IOptions<ApplicationInsightsConfig> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="K8sAppInsightsTelemetryInitializer"/> class.
        /// </summary>
        /// <param name="options">The Application Insights Configuration.</param>
        public K8sAppInsightsTelemetryInitializer(IOptions<ApplicationInsightsConfig> options)
        {
            this.options = options;
        }

        /// <inheritdoc/>
        public void Initialize(ITelemetry telemetry)
        {
            var cloudRoleName = this.options.Value.CloudRoleName;
            var cloudRoleInstance = this.options.Value.HostName;

            if (string.IsNullOrEmpty(cloudRoleName) == false)
            {
                telemetry.Context.Cloud.RoleName = cloudRoleName;
            }

            if (string.IsNullOrEmpty(cloudRoleInstance) == false)
            {
                telemetry.Context.Cloud.RoleInstance = cloudRoleInstance;
            }
        }
    }
}
