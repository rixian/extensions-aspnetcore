// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.OpenIdConnect
{
    using System;
    using IdentityServer4.AccessTokenValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Logging;
    using Newtonsoft.Json;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Additional OpenID Connect startup services.
    /// </summary>
    public class OidcStartupBuilder : StartupBuilder
    {
        /// <inheritdoc/>
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ILogger<OidcStartupBuilder> logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<OidcStartupBuilder>();
            OpenIdConnectConfig? options = context.Configuration.GetSection("Identity")?.Get<OpenIdConnectConfig>();
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
                Errors.Result isValid = options.CheckRequiredValues(); // Provides the required null checks.

                if (isValid.IsSuccess)
                {
                    this.AddHealthCheckServices(services, logger, options.Authority!, options.AuthorityHealthEndpoint);
                    this.AddAuthenticationServices(services, logger, options.Api!.Name!, options.Api!.Secret, options.Authority!, context.HostingEnvironment);

                    logger.LogInformation(Properties.Resources.ConfigurationFoundMessage, options?.Api?.Name, options?.Authority);
                }
                else if (context.HostingEnvironment.IsDevelopment())
                {
                    // Do nothing.
                    logger.LogWarning(Properties.Resources.InvalidConfigurationMessage, JsonConvert.SerializeObject(isValid.Error, Formatting.Indented));
                }
                else
                {
                    throw new ErrorException(isValid.Error, Properties.Resources.ConfigurationRequiredMessage);
                }
            }
        }

        private void AddHealthCheckServices(IServiceCollection services, ILogger logger, string authority, string? healthEndpoint = null)
        {
            healthEndpoint ??= "/.well-known/openid-configuration";
            var endpointUri = new Uri(new Uri(authority), healthEndpoint);

            logger.LogInformation(Properties.Resources.ConfiguringHealthCheckInfoMessage, endpointUri);
            _ = services.AddHealthChecks()
                .AddUrlGroup(endpointUri, "oidc", tags: new[] { "services" });
        }

        private void AddAuthenticationServices(IServiceCollection services, ILogger logger, string apiName, string? apiSecret, string authority, IWebHostEnvironment hostEnvironment)
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

            logger.LogInformation(Properties.Resources.ShowPiiInfoMessage, IdentityModelEventSource.ShowPII);
        }
    }
}
