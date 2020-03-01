// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Basic builder for configuring the startup process.
    /// </summary>
    public class StartupBuilder
    {
        /// <summary>
        /// Gets the list of service configurators.
        /// </summary>
        public ICollection<Action<WebHostBuilderContext, IServiceCollection>> ServiceConfigurators { get; } = new List<Action<WebHostBuilderContext, IServiceCollection>>();

        /// <summary>
        /// Gets or sets a delegate for configuring endpoints.
        /// </summary>
        public Action<IConfiguration, IWebHostEnvironment, IApplicationBuilder, IEndpointRouteBuilder>? ConfigureEndpoints { get; set; }

        /// <summary>
        /// Gets or sets a delegate to be executed prior to the UseRouting call.
        /// </summary>
        public Action<IConfiguration, IWebHostEnvironment, IApplicationBuilder>? PreviewUseRouting { get; set; }

        /// <summary>
        /// Gets or sets a delegate to be executed prior to the UseEndpoints call.
        /// </summary>
        public Action<IConfiguration, IWebHostEnvironment, IApplicationBuilder>? PreviewUseEndpoints { get; set; }

        /// <summary>
        /// Gets or sets a delegate to be executed prior to the UseAuthentication call.
        /// </summary>
        public Action<IConfiguration, IWebHostEnvironment, IApplicationBuilder>? PreviewUseAuthentication { get; set; }

        /// <summary>
        /// Gets or sets a delegate to be executed prior to the UseAuthorization call.
        /// </summary>
        public Action<IConfiguration, IWebHostEnvironment, IApplicationBuilder>? PreviewUseAuthorization { get; set; }

        /// <summary>
        /// Configures services for the web application.
        /// </summary>
        /// <param name="context">The WebHostBUilderContext.</param>
        /// <param name="services">The service collection.</param>
        public virtual void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
        }
    }
}
