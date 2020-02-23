﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.Hosting
{
    using System;
    using System.Globalization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Rixian.Extensions.AspNetCore;
    using Rixian.Extensions.AspNetCore.Api;

    /// <summary>
    /// Extensions for adding api services.
    /// </summary>
    public static class ApiHostingExtensions
    {
        /// <summary>
        /// Adds api services to the application, including OpenId Connect, Data Protection, and Redis caching.
        /// </summary>
        /// <param name="webBuilder">The WebHostBuilder.</param>
        /// <returns>The updated WebHostBuilder.</returns>
        public static IWebHostBuilder UseCompleteApiSetup(this IWebHostBuilder webBuilder)
        {
            if (webBuilder is null)
            {
                throw new ArgumentNullException(nameof(webBuilder));
            }

            return webBuilder
                .UseBasicExtensions(new ApiStartupBuilder())
                .UseOpenIdConnect()
                .UseDataProtection()
                .UseStackExchangeRedis();
                //.TryAddHostingStartupAssembly(typeof(Rixian.Extensions.AspNetCore.Api.Startup));
        }

        ///// <summary>
        ///// Adds basic api services to the application.
        ///// </summary>
        ///// <param name="webBuilder">The WebHostBuilder.</param>
        ///// <returns>The updated WebHostBuilder.</returns>
        //public static IWebHostBuilder UseBasicApiSetup(this IWebHostBuilder webBuilder)
        //{
        //    if (webBuilder is null)
        //    {
        //        throw new ArgumentNullException(nameof(webBuilder));
        //    }

        //    return webBuilder
        //        .UseBasicExtensions()
        //        .TryAddHostingStartupAssembly(typeof(Rixian.Extensions.AspNetCore.Api.Startup));
        //}
    }
}
