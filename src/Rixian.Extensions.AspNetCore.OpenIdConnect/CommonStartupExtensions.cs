using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Rixian.Extensions.Errors;

namespace Rixian.Extensions.AspNetCore.OpenIdConnect
{
    public static class CommonStartupExtensions
    {
        public static ICommonStartup AddOpenIdConnect(this ICommonStartup startup)
        {
            OpenIdConnectConfig? openIdConnectOptions = startup.Configuration.GetSection("Identity")?.Get<OpenIdConnectConfig>();
            if (openIdConnectOptions == null)
            {
                if (startup.Environment.IsDevelopment())
                {
                    // Do nothing.
                    //logger.LogWarning(Properties.Resources.ConfigurationMissingMessage);
                }
                else
                {
                    throw new InvalidOperationException("");// Properties.Resources.RequiredConfigurationMissingMessage);
                }
            }
            else
            {
                Extensions.Errors.Result isValid = openIdConnectOptions.CheckRequiredValues(); // Provides the required null checks.

                if (isValid.IsSuccess)
                {
                    AddHealthCheckServices(startup.Services, openIdConnectOptions.Authority!, openIdConnectOptions.AuthorityHealthEndpoint);
                    AddApiAuthenticationServices(startup.Services, openIdConnectOptions.Api!.Name!, openIdConnectOptions.Api!.Secret, openIdConnectOptions.Authority!, startup.Environment);

                    //logger.LogInformation(Properties.Resources.ConfigurationFoundMessage, openIdConnectOptions?.Api?.Name, openIdConnectOptions?.Authority);
                }
                else if (startup.Environment.IsDevelopment())
                {
                    // Do nothing.
                    //logger.LogWarning(Properties.Resources.InvalidConfigurationMessage, JsonConvert.SerializeObject(isValid.Error, Formatting.Indented));
                }
                else
                {
                    throw new ErrorException(isValid.Error, "");// Properties.Resources.ConfigurationRequiredMessage);
                }
            }

            return startup;
        }

        private static void AddHealthCheckServices(IServiceCollection services, string authority, string? healthEndpoint = null)
        {
            healthEndpoint ??= "/.well-known/openid-configuration";
            var endpointUri = new Uri(new Uri(authority), healthEndpoint);

            //logger.LogInformation(Properties.Resources.ConfiguringHealthCheckInfoMessage, endpointUri);
            _ = services.AddHealthChecks()
                .AddUrlGroup(endpointUri, "oidc", tags: new[] { "services" });
        }

        private static void AddApiAuthenticationServices(IServiceCollection services, string apiName, string? apiSecret, string authority, IWebHostEnvironment hostEnvironment)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = !hostEnvironment.IsDevelopment();
                    options.ApiName = apiName;
                    options.ApiSecret = apiSecret;
                });

            IdentityModelEventSource.ShowPII = hostEnvironment.IsDevelopment();

            //logger.LogInformation(Properties.Resources.ShowPiiInfoMessage, IdentityModelEventSource.ShowPII);
        }
    }
}
