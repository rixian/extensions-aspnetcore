// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.ApplicationInsights
{
    using System;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Telemetry processor used to filter out health related telemetry.
    /// </summary>
    public class HealthCheckTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor next;
        private readonly IOptions<ApplicationInsightsConfig> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckTelemetryProcessor"/> class.
        /// </summary>
        /// <param name="next">The next TelemetryProcessor.</param>
        /// <param name="options">The Application Insights Configuration.</param>
        public HealthCheckTelemetryProcessor(ITelemetryProcessor next, IOptions<ApplicationInsightsConfig> options)
        {
            this.next = next;
            this.options = options;
        }

        /// <inheritdoc/>
        public void Process(ITelemetry item)
        {
            if (item is RequestTelemetry request && this.options.Value.IgnoredEndpoints is object)
            {
                if (this.options.Value.IgnoredEndpoints.Any(e => request.Url.AbsolutePath.Contains(e)))
                {
                    return;
                }
            }

            if (item is TraceTelemetry trace && this.options.Value.IgnoredEndpoints is object)
            {
                if (this.options.Value.IgnoredEndpoints.Any(e => trace.Context?.Operation?.Name?.Contains(e) ?? false))
                {
                    return;
                }
            }

            this.next.Process(item);
        }
    }
}
