// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

[assembly: Microsoft.AspNetCore.Hosting.HostingStartup(typeof(Rixian.Extensions.AspNetCore.DataProtection.Startup))]

namespace Rixian.Extensions.AspNetCore.DataProtection
{
    public class Startup : IHostingStartup
    {
        /// <inheritdoc/>
        public void Configure(IWebHostBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder
                .ConfigureServices((context, services) =>
                {
                    ILogger<Startup> logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Startup>();

                    DataProtectionConfig options = context.Configuration.GetSection("DataProtection").Get<DataProtectionConfig>();

                    if (!string.IsNullOrWhiteSpace(options?.KeyRing?.KeyIdentifier))
                    {
                        services.AddFullDataProtection(options);

                        services
                            .AddHealthChecks()
                            .AddAzureBlobStorage(options.AzureStorage, name: "dataprotection-azblob")
                            .AddAzureKeyVault(
                                o =>
                                {
                                    o.UseKeyVaultUrl(new Uri(options.KeyRing.KeyIdentifier).GetLeftPart(UriPartial.Authority));
                                    o.UseClientSecrets(options.KeyRing.ClientId, options.KeyRing.ClientSecret);
                                },
                                name: "dataprotection-keyvault");
                    }
                    else if (context.HostingEnvironment.IsDevelopment())
                    {
                        // Do nothing.
                        logger.LogWarning("[DATA_PROTECTION] No KeyIdentifier specified, and running in Development. DataProtection will not be enabled.");
                    }
                    else
                    {
                        throw new InvalidOperationException("DataProtection credentials must be provided for non-Development applications.");
                    }
                });
        }
    }
}
