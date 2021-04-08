// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license.

#nullable enable
namespace Rixian.Extensions.AspNetCore
{
    using System;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extensions for registering Data Protection services.
    /// </summary>
    public static class DataProtectionServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the full Data Protection services to the DI container.
        /// </summary>
        /// <param name="services">The Service Collection.</param>
        /// <param name="appDiscriminator">The applicaiton discriminator.</param>
        /// <param name="blobConnectionString">The Azure Storage connection string.</param>
        /// <param name="keyName">The name of the KeyVault key.</param>
        /// <param name="keyIdentifier">The ID of the KeyVault key.</param>
        /// <param name="tenantId">The TenantID used to connect to KeyVault.</param>
        /// <param name="clientId">The ClientID used to connect to KeyVault.</param>
        /// <param name="clientSecret">The ClientSecret used to connect to KeyVault.</param>
        /// <returns>The updated Service Collection.</returns>
        public static IServiceCollection AddFullDataProtection(this IServiceCollection services, string? appDiscriminator, string? blobConnectionString, string? keyName, Uri? keyIdentifier, string? tenantId, string? clientId, string? clientSecret)
        {
            if (string.IsNullOrWhiteSpace(appDiscriminator))
            {
                throw new System.ArgumentException("A value must be provided.", nameof(appDiscriminator));
            }

            if (string.IsNullOrWhiteSpace(blobConnectionString))
            {
                throw new System.ArgumentException("A value must be provided.", nameof(blobConnectionString));
            }

            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new System.ArgumentException("A value must be provided.", nameof(keyName));
            }

            if (keyIdentifier == null)
            {
                throw new System.ArgumentException("A value must be provided.", nameof(keyIdentifier));
            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new System.ArgumentException("A value must be provided.", nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new System.ArgumentException("A value must be provided.", nameof(clientSecret));
            }

            // See: https://docs.microsoft.com/en-us/rest/api/storageservices/Naming-and-Referencing-Containers--Blobs--and-Metadata#container-names
            var containerName = $"dataprotection-{appDiscriminator}";

            IDataProtectionBuilder dataProtectionBuilder = services
                .AddDataProtection(o => o.ApplicationDiscriminator = appDiscriminator)
                .PersistKeysToAzureBlobStorage(blobConnectionString, containerName, keyName)
                .ProtectKeysWithAzureKeyVault(keyIdentifier, new Azure.Identity.ClientSecretCredential(tenantId, clientId, clientSecret));

            return services;
        }

        /// <summary>
        /// Adds the full Data Protection services to the DI container using the provided configuration.
        /// </summary>
        /// <param name="services">The Service Collection.</param>
        /// <returns>The updated Service Collection.</returns>
        public static IServiceCollection AddFullDataProtection(this IServiceCollection services)
        {
            ServiceProvider svc = services.BuildServiceProvider();
            IConfigurationSection dpSection = svc.GetRequiredService<IConfiguration>().GetSection("DataProtection");
            return services.AddFullDataProtection(dpSection);
        }

        /// <summary>
        /// Adds the full Data Protection services to the DI container.
        /// </summary>
        /// <param name="services">The Service Collection.</param>
        /// <param name="configuration">The configuration options.</param>
        /// <returns>The updated Service Collection.</returns>
        public static IServiceCollection AddFullDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new System.ArgumentNullException(nameof(configuration));
            }

            return services.AddFullDataProtection(configuration.Get<DataProtectionConfig>());
        }

        /// <summary>
        /// Adds the full Data Protection services to the DI container.
        /// </summary>
        /// <param name="services">The Service Collection.</param>
        /// <param name="options">The configuration options.</param>
        /// <returns>The updated Service Collection.</returns>
        public static IServiceCollection AddFullDataProtection(this IServiceCollection services, DataProtectionConfig options)
        {
            if (options == null)
            {
                throw new System.ArgumentNullException(nameof(options));
            }

            if (options.KeyRing == null)
            {
                throw new System.ArgumentOutOfRangeException(nameof(options), "KeyRing must be provided.");
            }

            services.AddFullDataProtection(
                options.ApplicationDiscriminator,
                options.AzureStorage,
                options.KeyRing.KeyName,
                options.KeyRing.KeyIdentifier,
                options.KeyRing.TenantId,
                options.KeyRing.ClientId,
                options.KeyRing.ClientSecret);

            return services;
        }
    }
}
