// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.Hosting
{
    using System;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Extensions for adding Redis services.
    /// </summary>
    public static class StackExchangeRedisHostingExtensions
    {
        /// <summary>
        /// Adds Redis services to the application.
        /// </summary>
        /// <param name="webBuilder">The WebHostBuilder.</param>
        /// <returns>The updated WebHostBuilder.</returns>
        public static IWebHostBuilder UseStackExchangeRedis(this IWebHostBuilder webBuilder)
        {
            if (webBuilder is null)
            {
                throw new ArgumentNullException(nameof(webBuilder));
            }

            return webBuilder
                .UseBasicExtensions()
                .TryAddHostingStartupAssembly(typeof(Rixian.Extensions.AspNetCore.StackExchangeRedis.Startup));
        }
    }
}
