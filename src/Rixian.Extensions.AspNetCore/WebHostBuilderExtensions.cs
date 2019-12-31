// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.Hosting
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder TryAddHostingStartupAssembly(this IWebHostBuilder webBuilder, Type type)
        {
            if (webBuilder is null)
            {
                throw new ArgumentNullException(nameof(webBuilder));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var assemblyName = type.Assembly.GetName().Name;
            return webBuilder.TryAddHostingStartupAssembly(assemblyName);
        }

        public static IWebHostBuilder TryAddHostingStartupAssembly(this IWebHostBuilder webBuilder, string assemblyName)
        {
            if (webBuilder is null)
            {
                throw new ArgumentNullException(nameof(webBuilder));
            }

            var setting = webBuilder.GetSetting(WebHostDefaults.HostingStartupAssembliesKey);
            bool assemblyAdded = setting?.Split(';', StringSplitOptions.RemoveEmptyEntries)?.Any(s => string.Equals(s, assemblyName, StringComparison.OrdinalIgnoreCase)) ?? false;
            if (assemblyAdded == false)
            {
                webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, string.Join(';', setting, assemblyName));
            }

            return webBuilder;
        }
    }
}
