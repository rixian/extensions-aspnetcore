// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.ApplicationInsights.Kubernetes
{
    using System;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Telemetry processor used to filter out K8s system telemetry.
    /// </summary>
    public class K8sNamespaceTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor next;
        private readonly IOptions<K8sApplicationInsightsConfig> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="K8sNamespaceTelemetryProcessor"/> class.
        /// </summary>
        /// <param name="next">The next TelemetryProcessor.</param>
        /// <param name="options">The Application Insights Configuration.</param>
        public K8sNamespaceTelemetryProcessor(ITelemetryProcessor next, IOptions<K8sApplicationInsightsConfig> options)
        {
            this.next = next;
            this.options = options;
        }

        /// <inheritdoc/>
        public void Process(ITelemetry item)
        {
            if (item is DependencyTelemetry dependency)
            {
                if (this.options.Value.IgnoredNamespaces.Any(e => dependency.Data?.Contains($"/api/v1/namespaces/{e}/pods") ?? false))
                {
                    return;
                }
            }

            this.next.Process(item);
        }
    }
}
