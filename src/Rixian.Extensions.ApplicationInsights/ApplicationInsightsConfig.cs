// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.ApplicationInsights
{
    using System.Collections.Generic;

    /// <summary>
    /// Configuration class for Application Insights.
    /// </summary>
    public class ApplicationInsightsConfig
    {
        /// <summary>
        /// Gets or sets the name of the service that will show up on the Application Map in App Insights.
        /// </summary>
        public string? CloudRoleName { get; set; }

        /// <summary>
        /// Gets or sets the hostname of the service.
        /// </summary>
        public string? HostName { get; set; }

        /// <summary>
        /// Gets or sets a list of relative urls that will be ignored.
        /// </summary>
        public IEnumerable<string>? IgnoredEndpoints { get; set; }
    }
}
