// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.Hosting
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Extensions for registering an assembly for hosting startup.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Trys adding the assembly containing the given type as a hosting startup assembly.
        /// </summary>
        /// <param name="webBuilder">The WebHostBuilder.</param>
        /// <param name="type">The type to register.</param>
        /// <returns>The updated WebHostBuilder.</returns>
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
            if (assemblyName is null)
            {
                throw new ArgumentOutOfRangeException(nameof(type), Rixian.Extensions.AspNetCore.Properties.Resources.UnableToFindAssemblyNameErrorMessage);
            }

            return webBuilder.TryAddHostingStartupAssembly(assemblyName);
        }

        /// <summary>
        /// Trys adding the assembly containing the given type as a hosting startup assembly.
        /// </summary>
        /// <param name="webBuilder">The WebHostBuilder.</param>
        /// <param name="assemblyName">The name of the assembly to register. Just the name, no '.dll' or version number.</param>
        /// <returns>The updated WebHostBuilder.</returns>
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
                webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, string.Join(';', setting, assemblyName).Trim().Trim(';'));
            }

            return webBuilder;
        }
    }
}
