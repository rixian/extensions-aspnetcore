// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rixian.Extensions.AspNetCore.DataProtection
{

    public static class DataProtectionServiceCollectionExtensions
    {
        public static IServiceCollection AddFullDataProtection(this IServiceCollection services, string appDiscriminator, string blobConnectionString, string keyName, string keyIdentifier, string clientId, string clientSecret)
        {
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
