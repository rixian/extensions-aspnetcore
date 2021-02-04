using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rixian.Extensions.Errors;

namespace Rixian.Extensions.AspNetCore.DataProtection
{
    public static class CommonStartupExtensions
    {
        public static ICommonStartup AddFullDataProtection(this ICommonStartup startup)
        {
            DataProtectionConfig? options = startup.Configuration.GetSection("DataProtection")?.Get<DataProtectionConfig>();
            if (options == null)
            {
                if (startup.Environment.IsDevelopment())
                {
                    // Do nothing.
                    //logger.LogWarning(Properties.Resources.ConfigurationMissingMessage);
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
                    startup.Services.AddFullDataProtection(options);

                    startup.Services
                        .AddHealthChecks()
                        .AddAzureBlobStorage(options.AzureStorage, name: "dataprotection-azblob")
                        .AddAzureKeyVault(
                            new Uri(options.KeyRing.KeyIdentifier?.GetLeftPart(UriPartial.Authority)),
                            new Azure.Identity.ClientSecretCredential(options.KeyRing.TenantId, options.KeyRing.ClientId, options.KeyRing.ClientSecret),
                            o => { },
                            name: "dataprotection-keyvault");

                    //logger.LogInformation(Properties.Resources.ConfigurationFoundMessage, options?.ApplicationDiscriminator, options?.KeyRing?.KeyIdentifier, options?.KeyRing?.KeyName);
                }
                else if (startup.Environment.IsDevelopment())
                {
                    startup.Services.AddDataProtection();
                    //logger.LogWarning(Properties.Resources.InvalidConfigurationMessage, JsonConvert.SerializeObject(isValid.Error, Formatting.Indented));
                }
                else
                {
                    throw new ErrorException(isValid.Error, Properties.Resources.CredentialsRequiredMessage);
                }
            }

            return startup;
        }
    }
}
