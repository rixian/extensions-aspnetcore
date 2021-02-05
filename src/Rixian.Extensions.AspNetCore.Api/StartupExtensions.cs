// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;

    /// <summary>
    /// Extensions for registering api methods with the dependency injection container.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Adds the Api Explorer to the DI container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddApiExplorerServices(this IServiceCollection services)
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            services
                .AddVersionedApiExplorer(
                    options =>
                    {
                        options.GroupNameFormat = "'v'VVV";

                        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        // options.SubstituteApiVersionInUrl = true;
                    });

            return services;
        }

        /// <summary>
        /// Adds the Api Versioning to the DI container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="defaultVersion">The default version to use.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddApiVersioningServices(this IServiceCollection services, DateTime? defaultVersion = null)
        {
            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new MediaTypeApiVersionReader());
                options.AssumeDefaultVersionWhenUnspecified = true;

                if (defaultVersion != null)
                {
                    options.DefaultApiVersion = new ApiVersion(defaultVersion.Value);
                }

                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.ReportApiVersions = true;
            });

            return services;
        }
    }
}
