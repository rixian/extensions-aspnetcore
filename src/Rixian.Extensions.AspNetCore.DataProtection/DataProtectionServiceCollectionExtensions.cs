// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.DataProtection
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
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
        /// <param name="clientId">The ClientID used to connect to KeyVault.</param>
        /// <param name="clientSecret">The ClientSecret used to connect to KeyVault.</param>
        /// <returns>The updated Service Collection.</returns>
        public static IServiceCollection AddFullDataProtection(this IServiceCollection services, string? appDiscriminator, string? blobConnectionString, string? keyName, string? keyIdentifier, string? clientId, string? clientSecret)
        {
            if (string.IsNullOrWhiteSpace(appDiscriminator))
            {
                throw new System.ArgumentException(Properties.Resources.MissingValueErrorMessage, nameof(appDiscriminator));
            }

            if (string.IsNullOrWhiteSpace(blobConnectionString))
            {
                throw new System.ArgumentException(Properties.Resources.MissingValueErrorMessage, nameof(blobConnectionString));
            }

            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new System.ArgumentException(Properties.Resources.MissingValueErrorMessage, nameof(keyName));
            }

            if (string.IsNullOrWhiteSpace(keyIdentifier))
            {
                throw new System.ArgumentException(Properties.Resources.MissingValueErrorMessage, nameof(keyIdentifier));
            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new System.ArgumentException(Properties.Resources.MissingValueErrorMessage, nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new System.ArgumentException(Properties.Resources.MissingValueErrorMessage, nameof(clientSecret));
            }

            IDataProtectionBuilder dataProtectionBuilder = services.AddDataProtection(o => o.ApplicationDiscriminator = appDiscriminator);

            if (CloudStorageAccount.TryParse(blobConnectionString, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                var containerName = $"dataprotection-{appDiscriminator}"; // See: https://docs.microsoft.com/en-us/rest/api/storageservices/Naming-and-Referencing-Containers--Blobs--and-Metadata#container-names
                CloudBlobContainer appContainer = blobClient.GetContainerReference(containerName);
                _ = appContainer.CreateIfNotExists();

                dataProtectionBuilder = dataProtectionBuilder.PersistKeysToAzureBlobStorage(appContainer, keyName);
            }

            if (!string.IsNullOrWhiteSpace(keyIdentifier) && !string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
            {
                dataProtectionBuilder = dataProtectionBuilder.ProtectKeysWithAzureKeyVault(keyIdentifier, clientId, clientSecret);
            }

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
            var dpSection = svc.GetRequiredService<IConfiguration>().GetSection("DataProtection");
            return services.AddFullDataProtection(dpSection);
        }

        public static IServiceCollection AddFullDataProtection(this IServiceCollection services, IConfiguration configuration) =>
            services.AddFullDataProtection(configuration.Get<DataProtectionConfig>());

        public static IServiceCollection AddFullDataProtection(this IServiceCollection services, DataProtectionConfig options)
        {
            if (options == null)
            {
                throw new System.ArgumentNullException(nameof(options));
            }

            services.AddFullDataProtection(
                options.ApplicationDiscriminator,
                options.AzureStorage,
                options.KeyRing.KeyName,
                options.KeyRing.KeyIdentifier,
                options.KeyRing.ClientId,
                options.KeyRing.ClientSecret);

            return services;
        }
    }
}
