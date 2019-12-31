﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.Hosting
{
    using System;
    using Microsoft.AspNetCore.Hosting;

    public static class ApiHostingExtensions
    {
        public static IWebHostBuilder UseApiSetup(this IWebHostBuilder webBuilder)
        {
            if (webBuilder is null)
            {
                throw new ArgumentNullException(nameof(webBuilder));
            }

            return webBuilder
                .UseBasicExtensions()
                .UseOpenIdConnect()
                .UseDataProtection()
                .UseStackExchangeRedis()
                .TryAddHostingStartupAssembly(typeof(Rixian.Extensions.AspNetCore.Api.Startup));
        }
    }
}
