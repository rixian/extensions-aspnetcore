﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.Hosting
{
    using System;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Extensions for adding Data Protection services.
    /// </summary>
    public static class DataProtectionHostingExtensions
    {
        /// <summary>
        /// Adds the Data Protection services to the application.
        /// </summary>
        /// <param name="webBuilder">The WebHostBuilder.</param>
        /// <returns>The updated WebHostBuilder.</returns>
        public static IWebHostBuilder UseDataProtection(this IWebHostBuilder webBuilder)
        {
            if (webBuilder is null)
            {
                throw new ArgumentNullException(nameof(webBuilder));
            }

            return webBuilder
                .UseBasicExtensions(new Rixian.Extensions.AspNetCore.DataProtection.DataProtectionStartupBuilder());
        }
    }
}
