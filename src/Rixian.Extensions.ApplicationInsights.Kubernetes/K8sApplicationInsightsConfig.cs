// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.ApplicationInsights.Kubernetes
{
    using System.Collections.Generic;

    /// <summary>
    /// Configuration class for Application Insights.
    /// </summary>
    public class K8sApplicationInsightsConfig : ApplicationInsightsConfig
    {
        /// <summary>
        /// Gets or sets a list of Kubernetes namespaces that will be ignored for health-related information.
        /// </summary>
        public IEnumerable<string>? IgnoredNamespaces { get; set; }
    }
}
