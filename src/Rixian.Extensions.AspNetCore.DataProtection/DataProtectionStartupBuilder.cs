// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.DataProtection
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Additional Data Protection startup services.
    /// </summary>
    public class DataProtectionStartupBuilder : StartupBuilder
    {
        /// <inheritdoc/>
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ILogger<DataProtectionStartupBuilder> logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<DataProtectionStartupBuilder>();

            DataProtectionConfig? options = context.Configuration.GetSection("DataProtection")?.Get<DataProtectionConfig>();
            if (options == null)
            {
                if (context.HostingEnvironment.IsDevelopment())
                {
                    // Do nothing.
                    logger.LogWarning(Properties.Resources.ConfigurationMissingMessage);
                }
                else
                {
                    throw new InvalidOperationException(Properties.Resources.RequiredConfigurationMissingMessage);
                }
            }
            else
            {
                Errors.Result isValid = options.CheckRequiredValues();

                if (isValid.IsSuccess)
                {
                    services.AddFullDataProtection(options);

                    services
                        .AddHealthChecks()
                        .AddAzureBlobStorage(options.AzureStorage, name: "dataprotection-azblob")
                        .AddAzureKeyVault(
                            o =>
                            {
                                o.UseKeyVaultUrl(new Uri(options.KeyRing!.KeyIdentifier!).GetLeftPart(UriPartial.Authority));
                                o.UseClientSecrets(options.KeyRing.ClientId, options.KeyRing.ClientSecret);
                            },
                            name: "dataprotection-keyvault");

                    logger.LogInformation(Properties.Resources.ConfigurationFoundMessage, options?.ApplicationDiscriminator, options?.KeyRing?.KeyIdentifier, options?.KeyRing?.KeyName);
                }
                else if (context.HostingEnvironment.IsDevelopment())
                {
                    services.AddDataProtection();
                    logger.LogWarning(Properties.Resources.InvalidConfigurationMessage, JsonConvert.SerializeObject(isValid.Error, Formatting.Indented));
                }
                else
                {
                    throw new ErrorException(isValid.Error, Properties.Resources.CredentialsRequiredMessage);
                }
            }
        }
    }
}
