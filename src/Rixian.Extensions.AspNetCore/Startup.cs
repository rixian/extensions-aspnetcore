// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

[assembly: Microsoft.AspNetCore.Hosting.HostingStartup(typeof(Rixian.Extensions.AspNetCore.Startup))]

namespace Rixian.Extensions.AspNetCore
{
    using System;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Additional basic startup services.
    /// </summary>
    public class Startup : IHostingStartup
    {
        /// <inheritdoc/>
        public void Configure(IWebHostBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
        }
    }
}
